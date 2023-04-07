using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fellow : MonoBehaviour
{
    Vector3 ogPosition; //To store the original position of the Fellow (Player)

    [SerializeField]
    float speed = 0.05f;

    [SerializeField]
    int lives = 5; //To store the number of lives the player has, serialise field to experiment in Unity Editor

    [SerializeField]
    float immunityDuration = 2.5f; //Stores the total amount of time the Fellow is immune for after respawning from being killed
    float immunityTime = 0.0f; //Stores the current immunity time left
    [SerializeField] 
    Material respawnMaterial; //Stores the material the Fellow has when immune
    [SerializeField] 
    Material normalMaterial; //Stores the normal material of the Fellow

    [SerializeField]
    int score = 0;
    int pelletsEaten = 0;
    [SerializeField]
    int pointsPerPellet = 100;

    [SerializeField]
    float powerupDuration = 10.0f ; // How long should powerups last ?
    float powerupTime = 0.0f ; // How long is left on the current powerup ?

    float powerupRespawnDuration = 20.0f; //Stores the total amount of time the powerup is unavailable after use for
    float powerupRespawnTime = 0.0f; //Stores the current respawn time of the powerup left

    bool powerupUsed = false; //To store when the player has consumed a powerup

    [SerializeField] 
    Material usedMaterial;
    [SerializeField] 
    Material powerupMaterial;

    bool crazyRound = false;

    public int PelletsEaten()
    {
        return pelletsEaten;
    }

    public bool PowerupActive()
    {
        return powerupTime > 0.0f;
    }

    public bool RespawnImmunityActive()
    {
        return immunityTime > 0.0f;
    }

    public int GetScore()
    {
        return this.score;
    }

    public int GetLives()
    {
        return this.lives;
    }

    public void setCrazyRound()
    {
        this.crazyRound = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        ogPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        powerupTime = Mathf.Max(0.0f, powerupTime - Time.deltaTime);
        powerupRespawnTime = Mathf.Max(0.0f, powerupRespawnTime - Time.deltaTime); //decrement powereup respawn timer
        immunityTime = Mathf.Max(0.0f, immunityTime - Time.deltaTime); //decrement Fellow respawn timer

        //check if power up time is 0, if so player can consume powerup again
        if (powerupRespawnTime == 0 && powerupUsed)
        {
            GameObject[] powerups;
            powerups = GameObject.FindGameObjectsWithTag("Powerup");
            foreach (GameObject p in powerups)
            {
                p.GetComponent<Renderer>().material = powerupMaterial;
            }
            powerupUsed = false;
        }

        if (immunityTime == 0)
        {
            this.GetComponent<Renderer>().material = normalMaterial;
        }
    }
    
    void FixedUpdate()
    {
        Rigidbody b = GetComponent<Rigidbody>();
        Vector3 velocity = b.velocity;

        if (Input.GetKey(KeyCode.A))
        {
            velocity.x = -speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity.x = speed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            velocity.z = speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity.z = -speed;
        }
        b.velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pellet"))
        {
            pelletsEaten++;
            score += pointsPerPellet;
            Debug.Log("Score is " + score);
        }
        //if the player has not picked up a powerup
        if (other.gameObject.CompareTag("Powerup") && !powerupUsed) //NEW ADDITION - !powerupUsed
        {
            powerupTime = powerupDuration;
            powerupRespawnTime = powerupRespawnDuration; //set timer on powerup respawn time (20 secs)
            //NEW ADDTIONS
            GameObject[] powerups;
            powerups = GameObject.FindGameObjectsWithTag("Powerup");
            powerupUsed = true;
            foreach (GameObject p in powerups) //set all powerups to reflect state
            {
                p.GetComponent<Renderer>().material = usedMaterial;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //if the Fellow collided with a ghost and is not immune from respawn
        if (collision.gameObject.CompareTag("Ghost") && !RespawnImmunityActive())
        {
            //if the Fellow is not powered up OR the Fellow encounters a crazy ghost (crazy ghost ignores powerup)
            if(!PowerupActive() || (PowerupActive() && crazyRound))
            {
                //Debug.Log("You've lost a life!");
                lives--; //decrement lives total
                gameObject.transform.position = ogPosition; //set Fellow position to original spawn position
                immunityTime =  immunityDuration; //make player immune to normal ghosts on respawn 
                this.GetComponent<Renderer>().material = respawnMaterial; //change Fellow material to reflect immune state
            }

            if (lives == 0) //Player has lost all their lives and so are dead
            {
                //Debug.Log("You died!");
                gameObject.SetActive(false); //remove Fellow from scene as dead
            }
        }
    }
}
