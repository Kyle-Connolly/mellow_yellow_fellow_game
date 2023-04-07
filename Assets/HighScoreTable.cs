using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;

public class HighScoreTable : MonoBehaviour
{
    [SerializeField]
    string highScoreFile = "scores.txt";//to access the score file
    string tempScoreFile = "tmpScore.txt"; //to temporarily store current session score and number of rounds

    struct HighScoreEntry
    {
        public int score;
        public string name;
    }

    List<HighScoreEntry> allScores = new List<HighScoreEntry>();

    [SerializeField]
    Font scoreFont;

    // Start is called before the first frame update
    void Start()
    {
        LoadHighScoreTable();
        SortHighScoreEntries();
        CreateHighScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadHighScoreTable()
    {
        using (TextReader file = File.OpenText(highScoreFile))
        {
            string text = null;
            while ((text = file.ReadLine()) != null)
            {
                Debug.Log(text);
                string[] splits = text.Split(' ');
                HighScoreEntry entry;
                entry.name = splits[0];
                entry.score = int.Parse(splits[1]);
                allScores.Add(entry);
            }
        }
    }

    void CreateHighScoreText()
    {
        for (int i = 0; i < allScores.Count; ++i)
        {
            GameObject o = new GameObject();
            o.transform.parent = transform;

            Text t = o.AddComponent<Text>();
            t.text = allScores[i].name + "\t\t" + allScores[i].score;
            t.font = scoreFont;
            t.fontSize = 50;

            o.transform.localPosition = new Vector3(0, -(i) * 6, 0);

            o.transform.localRotation = Quaternion.identity;
            o.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            o.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 100);
        }
    }

    public void SortHighScoreEntries()
    {
        allScores.Sort((x, y) => y.score.CompareTo(x.score));
    }

    //search through the score file to see if the player has played before, return true if so, else return false
    public bool findPlayer(string playerName)
    {
        //check if the player already has an entry in the high score file
        using (TextReader file = File.OpenText(highScoreFile))
        {
            string text = null;
            while ((text = file.ReadLine()) != null)
            {
                Debug.Log(text);
                string[] splits = text.Split(' ');
                string name = splits[0];
                if (name.Equals(playerName))
                {
                    return true;
                }
            }
        }
        return false;
    }

    
    //Write the player's new high score to the score file
    public void WriteHighScore(string playerName, int tmpScore)
    {
        if(!File.Exists(highScoreFile))
        {
            File.WriteAllText(highScoreFile, playerName + ' ' + tmpScore.ToString());

        }
        else 
        {
            if (findPlayer(playerName) == true)
            {
                int lastHighScore = 0;

                //create temporary file to copy over contents of original file
                string newHighSFilePath = "newHighSFile.txt";


                using (var reader = new StreamReader(highScoreFile))
                using (var writer = new StreamWriter(newHighSFilePath))
                {
                    string text = null;
                    
                    //loop until the end of the original file has not been reached
                    while ((text = reader.ReadLine()) != null)
                    {
                        Debug.Log(text);
                        string[] splits = text.Split(' ');
                        //create new high score entry struct
                        HighScoreEntry entry;
                        //set values in struct
                        entry.name = splits[0];
                        entry.score = int.Parse(splits[1]);
                        //if the entry does not match the player's name, write to the new file
                        if (entry.name != playerName)
                            writer.WriteLine(text);

                        if (entry.name.Equals(playerName))
                        {
                            lastHighScore = tmpScore + entry.score;
                        }
                            
                    }
                }
                //append name and score to file
                using (var writer = new StreamWriter(newHighSFilePath, append:true))
                {
                    writer.WriteLine(playerName + ' ' + lastHighScore.ToString());
                }
                //delete old file, rename new file to original
                File.Delete(highScoreFile);
                File.Move(newHighSFilePath, highScoreFile);
                File.Delete(tempScoreFile);
                return;
            }
            else
            {
                //append name and score to file
                using (var writer = new StreamWriter(highScoreFile, append:true))
                {
                    writer.WriteLine(playerName + ' ' + tmpScore.ToString());
                }
                File.Delete(tempScoreFile);
            }
        }
    }

    //write current session score to temporary file
    public void WriteTempHighScore(int roundNum, int tmpScore)
    {
        if (!File.Exists(tempScoreFile))
        {
            File.WriteAllText(tempScoreFile, roundNum.ToString() + ' ' + tmpScore.ToString());
        }
        else
        {
            using (var writer = new StreamWriter(tempScoreFile, false))
            {
                writer.WriteLine(roundNum.ToString() + ' ' + tmpScore.ToString());
            }
        }
    }

    //get last recorded score from current session
    public int GetTempScore()
    {
        if (File.Exists(tempScoreFile))
        {
            using (TextReader file = File.OpenText(tempScoreFile))
            {
                string text = null;
                while ((text = file.ReadLine()) != null)
                {
                    Debug.Log(text);
                    string[] splits = text.Split(' ');
                    int score = int.Parse(splits[1]);
                    return score;
                }
            }
        }
        return -1; 
    }

    //get the highest score from the score file
    public int GetHighestScore()
    {
        //populate and sort high score table if not already done
        LoadHighScoreTable();
        SortHighScoreEntries();

        //check the file exists
        if (File.Exists(highScoreFile))
        {
            //highest is stored at the start of the list so get index 0
            HighScoreEntry highest = allScores[0];
            return highest.score;
        }
        //file doesn't exist
        return -1;
    }

    //get the round number stored in the temporary file
    public int GetRoundNum()
    {
        if (File.Exists(tempScoreFile))
        {
            using (TextReader file = File.OpenText(tempScoreFile))
            {
                string text = null;
                while ((text = file.ReadLine()) != null)
                {
                    Debug.Log(text);
                    string[] splits = text.Split(' ');
                    int round = int.Parse(splits[0]);
                    //temp score file stores the amount of rounds completed. Want to know current round so +1
                    return round+1;
                }
            }
        }
        return -1;
    }
}
