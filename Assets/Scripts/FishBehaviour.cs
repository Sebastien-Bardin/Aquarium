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

        CurrentAvoidingVector= AvoidObstacles();
        ChangeDirection();

        transform.Translate(0, 0, Speed * Time.deltaTime);

    }


    private void ChangeDirection()
    {



        Vector3 grouping = Vector3.zero;
        Vector3 avoiding = Vector3.zero;


        if (GroupMembers.Count > 0)
        {
            for (int i = 0; i < GroupMembers.Count; i++)
            {
                if (GroupMembers[i] != this.gameObject)
                {
                    //Grouping sum of fish pos
                    grouping += GroupMembers[i].transform.position;

                    //Avoiding members
                    if (Vector3.Distance(GroupMembers[i].transform.position, this.transform.position) < GroupDist)
                    {
                        avoiding += (this.transform.position - GroupMembers[i].transform.position);
                    }

                }

            }
            //grouping point
            grouping = (grouping / GroupMembers.Count) * GroupingBehaviour;
            Vector3 destination = fishManager.Destination - transform.position;
           
            if (grouping != Vector3.zero)
            {
                Direction = (grouping.normalized + avoiding + CurrentAvoidingVector + destination.normalized) - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), RotationSpeed * Time.deltaTime);
            }
            else
            {
                Direction =  CurrentAvoidingVector*2;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), RotationSpeed * Time.deltaTime);
            }

        }

    }


    private Vector3 AvoidObstacles()
    {
        if (CurrentAvoidingVector != Vector3.zero)
        {
            RaycastHit check;
            if (!Physics.Raycast(transform.position,transform.forward, out check, ObstacleDistanceDetection, obstacleMask))
            {
                return CurrentAvoidingVector;
            }
        }
        float maxDist = 0 ;
        Vector3 newDirection = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, ObstacleDistanceDetection, obstacleMask))
        {
            
            for (int i = 0; i < DirectionToCheck.Length; i++)
            {
                var potentialDirection = transform.TransformDirection(DirectionToCheck[i].normalized);
                if (Physics.Raycast(transform.position, potentialDirection, out hit, ObstacleDistanceDetection, obstacleMask))
                {
                    float obstacleDistance = Vector3.Distance(hit.point, transform.position);
                    if (obstacleDistance > maxDist)
                    {
                        maxDist = obstacleDistance;
                        newDirection = potentialDirection;
                    }else
                    {
                        newDirection = potentialDirection;
                        return newDirection.normalized;
                    }
                }
            }
        }

        
        return newDirection.normalized;


    }


     private void OnCollisionStay(Collision other) {
        if (other.gameObject.layer == 6){
            Direction =  fishManager.transform.position -transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), 3*RotationSpeed * Time.deltaTime);
        }
     }

}
