using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    public float Speed = 5.0f;
    public float ObstacleDistanceDetection = 1.0f;
    public float RotationSpeed = 5.0f;
    public float GroupingBehaviour = 5.0f;
    public float GroupDist = 1.0f;
    public float AligningBehaviour = 1.0f;


    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private Vector3[] DirectionToCheck;



    private Vector3 Direction = Vector3.zero;
    private Vector3 CurrentAvoidingVector = Vector3.zero;
    private List<GameObject> GroupMembers = new List<GameObject>();




    private FishManager fishManager;
    // Start is called before the first frame update



    void Start()
    {
        //initializing prefab values when implemented in a FishManager
        fishManager = GetComponentInParent<FishManager>();
        fishManager.FishSpeed = Speed;
        fishManager.RotationSpeed = RotationSpeed;
        fishManager.GroupingBehaviour = GroupingBehaviour;
        fishManager.GroupDist = GroupDist;
        fishManager.AligningBehaviour = AligningBehaviour;
        fishManager.ObstacleDistanceDetection = ObstacleDistanceDetection;
        GroupMembers = fishManager.FishList;

        //Adding vectors to look at in order to avoid obstacles
        AvoidingDirections();
    }

    
    void Update()
    {
        //getting the fishmanager values for the group
        Speed = fishManager.FishSpeed;
        RotationSpeed = fishManager.FishSpeed;
        GroupingBehaviour = fishManager.GroupingBehaviour;
        GroupDist = fishManager.GroupDist;
        AligningBehaviour = fishManager.AligningBehaviour;
        ObstacleDistanceDetection = fishManager.ObstacleDistanceDetection;

        //look if there are obstacles
        CurrentAvoidingVector = AvoidObstacles();

        //Change the fish direction (forward)
        ChangeDirection();

        //move the fish at a cosntant speed
        transform.Translate(0, 0, Speed * Time.deltaTime);

    }


    private void ChangeDirection()
    {


        //initializing vectors
        Vector3 grouping = Vector3.zero;
        Vector3 avoiding = Vector3.zero;
        Vector3 alining = Vector3.zero;

        
        if (GroupMembers.Count > 0)
        {
            for (int i = 0; i < GroupMembers.Count; i++)
            {
                if (GroupMembers[i] != this.gameObject)
                {
                    //Grouping sum of fish pos
                    grouping += GroupMembers[i].transform.position;

                    //Aliging fish forwards
                    alining += GroupMembers[i].transform.forward;

                    //Avoiding members
                    if (Vector3.Distance(GroupMembers[i].transform.position, this.transform.position) < GroupDist)
                    {
                        avoiding += (this.transform.position - GroupMembers[i].transform.position);
                    }

                }

            }
            //grouping point
            grouping = (grouping / GroupMembers.Count) * GroupingBehaviour;

            //Direction fish will try to align with
            alining = (alining / GroupMembers.Count) * AligningBehaviour;

            //If fish are grouped
            if (grouping != Vector3.zero)
            {
                Vector3 destination = fishManager.Destination - transform.position;
                // adding all vectors to make a direction
                Direction = (grouping.normalized + avoiding + CurrentAvoidingVector + destination + alining.normalized) - transform.position;
                //rotating the fish in order to make it face is destination
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), RotationSpeed * Time.deltaTime);
            }
            else // if fish is on its own
            {
                Direction = CurrentAvoidingVector;
                //this range is preventing the direction to change every frame
                if (Random.Range(0.0f, 1000.0f) < fishManager.ChangingDestinationChance)
                {
                    //new random destination for signle fish
                    Vector3 destination = transform.position + new Vector3(Random.Range(-fishManager.GroupSpawnRange.x, fishManager.GroupSpawnRange.x), Random.Range(-fishManager.GroupSpawnRange.y, fishManager.GroupSpawnRange.y), Random.Range(-fishManager.GroupSpawnRange.z, fishManager.GroupSpawnRange.z));
                    Direction += destination;
                }

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction),  RotationSpeed * Time.deltaTime);
            }

        }

    }

    //Method to avoid gameobjects on the Obstacle Layer
    private Vector3 AvoidObstacles()
    {
        //checking if the previous avoid vetor is still good 
        if (CurrentAvoidingVector != Vector3.zero)
        {
            RaycastHit check;
            if (!Physics.Raycast(transform.position, transform.forward, out check, ObstacleDistanceDetection, obstacleMask))
            {
                return CurrentAvoidingVector;
            }
        }
        float maxDist = 0;
        Vector3 newDirection = Vector3.zero;
        RaycastHit hit;

        //Checking if there is an obstacle 
        if (Physics.Raycast(transform.position, transform.forward, out hit, ObstacleDistanceDetection, obstacleMask))
        {
            //Check every avoid angles 
            for (int i = 0; i < DirectionToCheck.Length; i++)
            {
                //Checking if the potential avoiding direction hit something
                var potentialDirection = transform.TransformDirection(DirectionToCheck[i].normalized);
                if (Physics.Raycast(transform.position, potentialDirection, out hit, ObstacleDistanceDetection, obstacleMask))
                {   
                    //Potential avoiding vector hit something so we will store the one further away from the obsctacle
                    float obstacleDistance = Vector3.Distance(hit.point, transform.position);
                    if (obstacleDistance > maxDist)
                    {
                        maxDist = obstacleDistance;
                        newDirection = potentialDirection;
                       
                    }
                }
                else //return the avoiding vector
                {

                    newDirection = potentialDirection;
                    return newDirection.normalized;
                }
            }
        }
        return newDirection.normalized;
    }


    //Adding vectors to check when avoiding 
    private void AvoidingDirections()
    {
        DirectionToCheck = new Vector3[5];
        DirectionToCheck[0] = Vector3.right;
        DirectionToCheck[1] = Vector3.left;
        DirectionToCheck[2] = Vector3.up;
        DirectionToCheck[3] = Vector3.down;
        DirectionToCheck[4] = Vector3.back;
    }

}
