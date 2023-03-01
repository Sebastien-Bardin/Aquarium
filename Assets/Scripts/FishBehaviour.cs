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
        fishManager = GetComponentInParent<FishManager>();
        fishManager.FishSpeed = Speed;
        fishManager.RotationSpeed = RotationSpeed;
        fishManager.GroupingBehaviour = GroupingBehaviour;
        fishManager.GroupDist = GroupDist;
        fishManager.AligningBehaviour = AligningBehaviour;
        fishManager.ObstacleDistanceDetection = ObstacleDistanceDetection;
        GroupMembers = fishManager.FishList;

        AvoidingDirections();


    }

    // Update is called once per frame
    void Update()
    {
        Speed = fishManager.FishSpeed;
        RotationSpeed = fishManager.FishSpeed;
        GroupingBehaviour = fishManager.GroupingBehaviour;
        GroupDist = fishManager.GroupDist;
        AligningBehaviour = fishManager.AligningBehaviour;
        ObstacleDistanceDetection = fishManager.ObstacleDistanceDetection;

        CurrentAvoidingVector = AvoidObstacles();
        ChangeDirection();

        transform.Translate(0, 0, Speed * Time.deltaTime);

    }


    private void ChangeDirection()
    {



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
            alining = (alining / GroupMembers.Count) * AligningBehaviour;


            if (grouping != Vector3.zero)
            {
                Vector3 destination = fishManager.Destination - transform.position;
                Direction = (grouping.normalized + avoiding + CurrentAvoidingVector + destination + alining.normalized) - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), RotationSpeed * Time.deltaTime);
            }
            else
            {
                Direction = CurrentAvoidingVector;
                if (Random.Range(0.0f, 1000.0f) < fishManager.ChangingDestinationChance)
                {
                    Debug.Log("Changing direction");
                    Vector3 destination = transform.position + new Vector3(Random.Range(-fishManager.GroupSpawnRange.x, fishManager.GroupSpawnRange.x), Random.Range(-fishManager.GroupSpawnRange.y, fishManager.GroupSpawnRange.y), Random.Range(-fishManager.GroupSpawnRange.z, fishManager.GroupSpawnRange.z));
                    Direction += destination;
                }

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction),  RotationSpeed * Time.deltaTime);
            }

        }

    }


    private Vector3 AvoidObstacles()
    {
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
        if (Physics.Raycast(transform.position, transform.forward, out hit, ObstacleDistanceDetection, obstacleMask))
        {
            Debug.Log("raycast hit");

            for (int i = 0; i < DirectionToCheck.Length; i++)
            {

                var potentialDirection = transform.TransformDirection(DirectionToCheck[i].normalized);
                if (Physics.Raycast(transform.position, potentialDirection, out hit, ObstacleDistanceDetection, obstacleMask))
                {
                    Debug.Log("Pot HIT");
                    float obstacleDistance = Vector3.Distance(hit.point, transform.position);
                    if (obstacleDistance > maxDist)
                    {
                        maxDist = obstacleDistance;
                        //Debug.Log(potentialDirection);
                        newDirection = potentialDirection;
                        //newDirection = -transform.forward;
                    }
                }
                else
                {

                    newDirection = potentialDirection;
                    //newDirection = -transform.forward;
                    Debug.Log("Avoiding");
                    return newDirection.normalized;
                }
            }
        }
        return newDirection.normalized;
    }


    private void OnCollisionStay(Collision other)
    {
        /*if (other.gameObject.layer == 6)
        {
            Direction = fishManager.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), 3 * RotationSpeed * Time.deltaTime);
        }*/
    }

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
