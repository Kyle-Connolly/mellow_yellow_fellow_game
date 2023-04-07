using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField]
    Fellow player;

    UnityEngine.AI.NavMeshAgent agent;
    
    private Vector3 ogPosition; //To store the original position of the Ghost

    [SerializeField]
    Material scaredMaterial;
    [SerializeField] 
    Material normalMaterial; //Store normal material
    [SerializeField] 
    Material eatenMaterial; //Store "eaten" material
    [SerializeField] 
    Material crazyMaterial; //Store material that shows Ghost is "crazy"

    [SerializeField]
    public Transform landingZone; //To store the position of the ghost house

    bool eaten = false; //To store if the ghost has been eaten by the Fellow or not

    bool hiding = false;

    bool crazy = false; //To store if the Ghost's crazy behaviour is activated
    bool destLocked = false; //To store if the Crazy Ghost has locked onto the player


    //setter method to set Ghost's crazy behaviour material to reflect state
    public void setCrazyBehaviour()
    {
        this.crazy = true;
        GetComponent<Renderer>().material = crazyMaterial;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = PickRandomPosition();
        normalMaterial = GetComponent<Renderer>().material;
        //set the original spawn position
        this.ogPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
    }

    Vector3 PickRandomPosition()
    {
        Vector3 destination = transform.position;
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle * 8.0f;
        destination.x += randomDirection.x;
        destination.z += randomDirection.y;

        UnityEngine.AI.NavMeshHit navHit;
        UnityEngine.AI.NavMesh.SamplePosition(destination, out navHit, 8.0f, UnityEngine.AI.NavMesh.AllAreas);

        return navHit.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.PowerupActive() && !crazy) //NEW ADDIION - !crazy - if Ghost is crazy ignore poweredup Fellow
        {
            Debug.Log("Hiding from Player!");
            if (!hiding || agent.remainingDistance < 0.5f)
            {
                hiding = true;
                agent.destination = PickHidingPlace();
                GetComponent<Renderer>().material = scaredMaterial;
            }

            //NEW ADDITION - if an eaten Ghost arrives at the Ghost House, restore it by setting bool and change material to reflect
            if (this.eaten == true && agent.transform.position == landingZone.transform.position)
            {
                this.eaten = false;
                GetComponent<Renderer>().material = scaredMaterial;
            }
            
        }
        else
        {
            //Debug.Log("Chasing Player!");
            if (hiding) 
            {
                GetComponent<Renderer>().material = normalMaterial;
                hiding = false;
            }

            if (CanSeePlayer())
            {
                //Debug.Log("I can see you!");
                agent.destination = player.transform.position;

                //NEW ADDITION - if the Ghost is "crazy" and has seen the player, lock its NavMesh destination to the player always
                if (crazy)
                {
                    destLocked = true;
                }
            }

            //NEW - always go for the player if it has seen the player and destination is locked
            if (destLocked)
            {
                agent.destination = player.transform.position;
            }

            //NEW ADDIION - !destLocked, only want to move randomly around level if normal Ghost
            if (agent.remainingDistance < 0.5f && !destLocked) 
            {
                agent.destination = PickRandomPosition();
                hiding = false;
                GetComponent<Renderer>().material = normalMaterial;
            }

        }
    }

    bool CanSeePlayer()
    {
        Vector3 rayPos = transform.position;
        Vector3 rayDir = (player.transform.position - rayPos).normalized;

        RaycastHit info;
        if (Physics.Raycast(rayPos, rayDir, out info))
        {
            if (info.transform.CompareTag("Fellow")) 
            {
                return true; // the ghost can see the player
            }
        }
        return false;
    }

    Vector3 PickHidingPlace()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        UnityEngine.AI.NavMeshHit navHit;
        UnityEngine.AI.NavMesh.SamplePosition(transform.position - (directionToPlayer * 8.0f) , out navHit , 8.0f , UnityEngine.AI.NavMesh.AllAreas);

        return navHit.position;
    }

    //To check if the Ghost has collided with the player
    void OnCollisionEnter(Collision collision)
    {
        //if the tag associated with the collider is Fellow, and powerup is active, and the Ghost is a normal ghost
        if (collision.gameObject.CompareTag("Fellow") && player.PowerupActive() && !crazy)
        {
            //Debug.Log("Ghost Eaten!");
            //Fellow is powered up so make the Ghost "eaten"
            this.eaten = true;
            //set material to reflect state
            GetComponent<Renderer>().material = eatenMaterial;
            //set the destination of the eaten Ghost to the Ghost House to be restored
            agent.SetDestination(landingZone.transform.position);
            
        }

        //If the Ghost collides with a non-poweredup Fellow and the Fellow is not immune due to recently respawning
        if (collision.gameObject.CompareTag("Fellow") && !player.PowerupActive() && !player.RespawnImmunityActive())
        {
            //Ghost has killed player so reset this instance of the Ghost
            agent.transform.position = ogPosition;
        }
    }
}
