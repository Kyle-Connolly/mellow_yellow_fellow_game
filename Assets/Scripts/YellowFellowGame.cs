using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class YellowFellowGame : MonoBehaviour
{
    [SerializeField]
    GameObject highScoreUI;

    [SerializeField]
    GameObject mainMenuUI;

    [SerializeField]
    GameObject gameUI;

    [SerializeField]
    GameObject winUI;

    [SerializeField]
    Fellow playerObject;

    GameObject[] pellets;

    [SerializeField]
    HighScoreTable highScoreTable;

    enum GameMode
    {
        InGame,
        MainMenu,
        HighScores
    }

    GameMode gameMode = GameMode.MainMenu;

    // Start is called before the first frame update
    void Start()
    {
        StartMainMenu();
        pellets = GameObject.FindGameObjectsWithTag("Pellet");
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (gameMode)
        {
            case GameMode.MainMenu:     UpdateMainMenu(); break;
            case GameMode.HighScores:   UpdateHighScores(); break;
            case GameMode.InGame:       UpdateMainGame(); break;
        }

        //NEW ADDITIONS
        int crazyGhostRound = highScoreTable.GetRoundNum();

        //if round number is a multiple of 5 - every 5th round turn a ghost crazy
        if (crazyGhostRound != -1 && crazyGhostRound % 5 == 0)
        {
            //randomly choose a ghost to turn crazy
            //call method to turn ghost crazy
            GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Ghost");
            GameObject randomGhost = allEnemies[0];
            Ghost crazyGhost = randomGhost.GetComponent<Ghost>();
            crazyGhost.setCrazyBehaviour();
            playerObject.setCrazyRound();
        }
        
        //if the player eats all the pellets (completes level) or dies mid round
        if (playerObject.PelletsEaten() == pellets.Length)
        {
            //Debug.Log("Level Completed!");
            
            //get the player's last recorded score from the current session
            int tempScore = highScoreTable.GetTempScore();

            int round = 1;

            //player's first round/game in current session so just save current score
            if(tempScore == -1)
            {
                highScoreTable.WriteTempHighScore(round, playerObject.GetScore());
                //reload the scene to begin a new level/round
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                return;
            }

            
            int totalScore = tempScore + playerObject.GetScore();

            round = highScoreTable.GetRoundNum();

            //write new score for session to temp score file
            highScoreTable.WriteTempHighScore(round, totalScore);
            
            
            //reload the scene to begin a new level/round
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    
    public void OnApplicationQuit()
    {
        int tempScore = highScoreTable.GetTempScore();

        //player's first round/game so just save current score
        if (!highScoreTable.findPlayer("player"))
        {
            //write to score file
            highScoreTable.WriteHighScore("player", playerObject.GetScore());
        }
        else
        {
            //player has an entry and their latests score is greater than their recorded one
            if(highScoreTable.findPlayer("player") && tempScore > highScoreTable.GetHighestScore())
            {
                //write to score file
                highScoreTable.WriteHighScore("player", tempScore);
            }
        }
    }
    

    void UpdateMainMenu()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            StartHighScores();
        }
    }

    void UpdateHighScores()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartMainMenu();
        }
    }

    void UpdateMainGame()
    {
        //Display highest score (score to beat)
        GameObject childThree = gameUI.transform.GetChild(2).gameObject;
        Text highestScoreText = childThree.GetComponent<Text>();
        string highestScoreStr = "Highest Score: ";
        highestScoreText.text = highestScoreStr + highScoreTable.GetHighestScore().ToString();

        //Display player's current score AND round/level number
        int tempScore = highScoreTable.GetTempScore();

        GameObject childFour = gameUI.transform.GetChild(3).gameObject;
        Text scoreText = childFour.GetComponent<Text>();
        string scoreStr = "Score: ";

        GameObject childSix = gameUI.transform.GetChild(5).gameObject;
        Text roundText = childSix.GetComponent<Text>();
        string roundStr = "Round: ";
        int round = 1;

        if (tempScore == -1)
        {
            scoreText.text = scoreStr + playerObject.GetScore().ToString();
        }
        else
        {
            int sessionScore = tempScore + playerObject.GetScore();
            scoreText.text = scoreStr + sessionScore.ToString();

            //temp file exists so round is greater than one so get the round number and set round
            round = highScoreTable.GetRoundNum();
            //Debug.Log(round);
        }
        roundText.text = roundStr + round.ToString();

        //Display lives remaing
        GameObject childFive = gameUI.transform.GetChild(4).gameObject;
        Text livesText = childFive.GetComponent<Text>();
        string livesStr = "Lives: ";
        livesText.text = livesStr + playerObject.GetLives().ToString();
    }

    void StartMainMenu()
    {
        gameMode                        = GameMode.MainMenu;
        mainMenuUI.gameObject.SetActive(true);
        highScoreUI.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(false);
    }


    void StartHighScores()
    {
        gameMode                = GameMode.HighScores;
        mainMenuUI.gameObject.SetActive(false);
        highScoreUI.gameObject.SetActive(true);
        gameUI.gameObject.SetActive(false);
    }

    void StartGame()
    {
        gameMode                = GameMode.InGame;
        mainMenuUI.gameObject.SetActive(false);
        highScoreUI.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(true);
    }
}
