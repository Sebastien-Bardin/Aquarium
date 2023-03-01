using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiFishParameters : MonoBehaviour
{
    //UI Elements 
    public FishManager FM;
    public TMP_Dropdown FishType;
    public TMP_InputField FishNum;
    public Slider FishSpeed;
    public Slider RotationSpeed;
    public Slider GroupingBehaviour;
    public Slider GroupDist;
    public Slider AligningBehaviour;
    public Slider ObstacleDist;
    public Slider DestinationFreq;

    //list of prefab we can instantiate
    public List<GameObject> FishPrefabs = new List<GameObject>();
    private List<string> FishPrefabNames = new List<string>();

    

    
    void Start()
    {
        //loading fish prefab in the dropdown
        foreach (var fish in FishPrefabs)
        {
            FishPrefabNames.Add(fish.name);
        }
        FishType.ClearOptions();
        FishType.AddOptions(FishPrefabNames);

        //Setting the default fish values
        FishSpeed.value = FishPrefabs[0].gameObject.GetComponent<FishBehaviour>().Speed;
        RotationSpeed.value = FishPrefabs[0].gameObject.GetComponent<FishBehaviour>().RotationSpeed ;
        GroupingBehaviour.value =FishPrefabs[0].gameObject.GetComponent<FishBehaviour>().GroupingBehaviour ;
        GroupDist.value = FishPrefabs[0].gameObject.GetComponent<FishBehaviour>().GroupDist ;
        AligningBehaviour.value = FishPrefabs[0].gameObject.GetComponent<FishBehaviour>().AligningBehaviour;
        ObstacleDist.value = FishPrefabs[0].gameObject.GetComponent<FishBehaviour>().ObstacleDistanceDetection;
        
        
    }

    void Update()
    {
        //Updating the FishManager values with the values entered in the UI
        if (FishNum.text.Length != 0)
        {
            FM.FishNumber = int.Parse((string)FishNum.text);
        }
        FM.FishSpeed = FishSpeed.value;
        FM.RotationSpeed = RotationSpeed.value;
        FM.GroupingBehaviour = GroupingBehaviour.value;
        FM.GroupDist = GroupDist.value;
        FM.AligningBehaviour = AligningBehaviour.value;
        FM.ObstacleDistanceDetection = ObstacleDist.value;
        FM.ChangingDestinationChance = DestinationFreq.value;

        
        
    }

    public void SelectType(){

        //Changing the fish prefab and setting default values
       foreach (var fish in FishPrefabs )
       {
         if (fish.name == FishPrefabNames[FishType.value])
         {
            FM.FishPrefab = fish;
            FM.ChangeFishType();
            FishSpeed.value = fish.GetComponent<FishBehaviour>().Speed;
            RotationSpeed.value = fish.gameObject.GetComponent<FishBehaviour>().RotationSpeed ;
            GroupingBehaviour.value =fish.gameObject.GetComponent<FishBehaviour>().GroupingBehaviour ;
            GroupDist.value = fish.gameObject.GetComponent<FishBehaviour>().GroupDist ;
            AligningBehaviour.value = fish.gameObject.GetComponent<FishBehaviour>().AligningBehaviour;
            ObstacleDist.value = fish.gameObject.GetComponent<FishBehaviour>().ObstacleDistanceDetection;
            
            return;
         }
        
       }
    }
   
}
