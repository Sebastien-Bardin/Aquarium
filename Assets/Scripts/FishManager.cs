

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{

    public int FishNumber = 1;
    public GameObject FishPrefab;
    public Vector3 FishTankLimits = new Vector3(5, 5, 5);
    public List<GameObject> FishList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnFish();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnFish();
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
                    Vector3 randomPos = transform.position + new Vector3(Random.Range(-FishTankLimits.x, FishTankLimits.x), Random.Range(-FishTankLimits.y, FishTankLimits.y), Random.Range(-FishTankLimits.z, FishTankLimits.z));
                    FishList.Add(Instantiate(FishPrefab, randomPos, Quaternion.identity));

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
}
