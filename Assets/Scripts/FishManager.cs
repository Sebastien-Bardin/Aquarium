

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{

    public List<GameObject> FishList = new List<GameObject>(); 

    [Header("Group fish settings")]
    public GameObject FishPrefab;
    public int FishNumber = 1;
    [Range(0.0f, 10.0f)]
    public float FishSpeed;
    [Range(0.0f, 10.0f)]
    public float RotationSpeed;

    //Variable to set if the fish move in group
    [Range(0.0f, 1.0f)]
    public float GroupingBehaviour;

    //Variable to set the distance between fishes in the group
    [Range(0.0f, 10.0f)]
    public float GroupDist;

    //Variable to set if the fish try to align with eachother
    [Range(0.0f, 10.0f)]
    public float AligningBehaviour;

    
    [Range(0.0f, 10.0f)]
    public float ObstacleDistanceDetection;

    [Range(0.0f, 100.0f)]
    public float ChangingDestinationChance = 1.0f;

    public float TimeBetweenDestChange = 2.0f;

    public Vector3 GroupSpawnRange = new Vector3(5, 5, 5);

    public Vector3 Destination = Vector3.zero;



    private bool DestChanged =false;


    
    void Start()
    {
        //spawnig fish at start and setting a default destination
        SpawnFish();
        Destination = transform.position;
    }

    void Update()
    {   
        //Checking every frame if spawning or removing fish is necessary
        SpawnFish();

        //Changing the group destination
        ChangeDestination();
    }




    //method to spawn fish 
    private void SpawnFish()
    {
        //checking if prefab is set
        if (FishPrefab)
        {
            //checking if we need more fish
            if (FishList.Count < FishNumber)
            {
                for (int i = FishList.Count; i < FishNumber; i++)
                {
                    //adding fish at a random position in the Group spawn range 
                    Vector3 randomPos = transform.position + new Vector3(Random.Range(-GroupSpawnRange.x, GroupSpawnRange.x), Random.Range(-GroupSpawnRange.y, GroupSpawnRange.y), Random.Range(-GroupSpawnRange.z, GroupSpawnRange.z));
                    GameObject fish = Instantiate(FishPrefab, randomPos, Quaternion.identity);
                    //Linking the fish to its group manager
                    fish.transform.parent = gameObject.transform;

                    //adding the fish to the list
                    FishList.Add(fish);

                }
            }
            //checking if we need less fish
            else if (FishList.Count > FishNumber)
            {
                for (int i = FishList.Count - 1; i >= FishNumber; i--)
                {
                    //remove the fish from the scene
                    Destroy(FishList[i]);
                    //remove the fish from the list
                    FishList.Remove(FishList[i]);
                }
            }
        }
    }
    
    //changing the fish prefab
    public void ChangeFishType(){
        //removing every old prefab fish
        foreach (var fish in FishList)
        {
            Destroy(fish);
        }
        FishList.Clear();
    }


    //Timer to prevent the distination from changing for a certain time(TimeBettweenDestChange)
    IEnumerator ChangeDestinationTimer(){

        yield return new WaitForSeconds(TimeBetweenDestChange);
        DestChanged =false;
        
    }

    //Changing the group fish destination
    private void ChangeDestination(){
        
        //percentage chance that the direction will change
        if (Random.Range(0,100)<ChangingDestinationChance && !DestChanged)
        {
            //finding random destination in the spawn range
            Destination = transform.position + new Vector3(Random.Range(-GroupSpawnRange.x, GroupSpawnRange.x), Random.Range(-GroupSpawnRange.y, GroupSpawnRange.y), Random.Range(-GroupSpawnRange.z, GroupSpawnRange.z));
            DestChanged = true;
            //launching timer
            StartCoroutine(ChangeDestinationTimer());
           
        }
    }


    
}
