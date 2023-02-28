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


        if (Random.Range(0.0f , 100.0f) < 25.0f){
            ChangeDirection();
            Debug.Log("ChangeDirection");
        }


        transform.Translate(0, 0, Speed * Time.deltaTime);

    }


    private void ChangeDirection(){

        

        Vector3 grouping = Vector3.zero;
        Vector3 following = Vector3.zero;
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
            grouping = (grouping / GroupMembers.Count) * GroupingBehaviour ;
            Vector3 direction = (grouping  + avoiding + (fishManager.Destination-transform.position) ) - transform.position;
            if (grouping != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), RotationSpeed * Time.deltaTime);
            }

        }

    }


    /* #if(UNITY_EDITOR)
         [ContextMenu("Initialize fish type")]
         void IntitializeFish(){
             fishManager = GetComponentInParent<FishManager>();
             fishManager.FishSpeed = Speed;  
         }

     #endif*/

}
