

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
    [Range(0.0f, 1.0f)]
    public float GroupingBehaviour;
    [Range(0.0f, 10.0f)]
    public float GroupDist;
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


    // Start is called before the first frame update
    void Start()
    {
        SpawnFish();
        Destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnFish();

        ChangeDestination();
        Debug.Log(DestChanged);
    }





    private void SpawnFish()
    {
        Debug.Log(FishList.Count);
        if (FishPrefab)
        {
            if (FishList.Count < FishNumber)
            {
                for (int i = FishList.Count; i < FishNumber; i++)
                {
                    Vector3 randomPos = transform.position + new Vector3(Random.Range(-GroupSpawnRange.x, GroupSpawnRange.x), Random.Range(-GroupSpawnRange.y, GroupSpawnRange.y), Random.Range(-GroupSpawnRange.z, GroupSpawnRange.z));
                    GameObject fish = Instantiate(FishPrefab, randomPos, Quaternion.identity);
                    fish.transform.parent = gameObject.transform;
                    FishList.Add(fish);

                }
            }
            else if (FishList.Count > FishNumber)
            {
                for (int i = FishList.Count - 1; i >= FishNumber; i--)
                {

                    Destroy(FishList[i]);
                    FishList.Remove(FishList[i]);
                }
            }
        }
    }

    IEnumerator ChangeDestinationTimer(){

        yield return new WaitForSeconds(TimeBetweenDestChange);
        DestChanged =false;
        
    }

    private void ChangeDestination(){
        
        if (Random.Range(0,100)<ChangingDestinationChance && !DestChanged)
        {
            Destination = transform.position + new Vector3(Random.Range(-GroupSpawnRange.x, GroupSpawnRange.x), Random.Range(-GroupSpawnRange.y, GroupSpawnRange.y), Random.Range(-GroupSpawnRange.z, GroupSpawnRange.z));
            DestChanged = true;
            StartCoroutine(ChangeDestinationTimer());
           
        }
    }


    
}
