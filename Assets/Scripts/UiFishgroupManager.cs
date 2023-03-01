using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiFishgroupManager : MonoBehaviour
{

    public GameObject content;
    public GameObject FishGroupCanvasPrefab;
    public GameObject FishManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Button add function 
    public void AddGroup(){

        //instantiating a new fish group canvas
       GameObject newCanvas =  Instantiate(FishGroupCanvasPrefab, content.transform.position, Quaternion.identity);
       newCanvas.transform.SetParent(content.transform, false);

        //instantiating a new fish manager
       GameObject NewFishGroup = Instantiate(FishManager, Vector3.zero, Quaternion.identity);

        //Linking the freshly created canvas to its fish manager 
       newCanvas.gameObject.GetComponent<UiFishParameters>().FM = NewFishGroup.GetComponent<FishManager>();
    }

    //Button remove function 
    public void RemoveGroup(){
        //checking if any box are checked
        foreach (Toggle toggle in content.GetComponentsInChildren<Toggle>())
        {   
            if (toggle.isOn)
            {
                //Removing fish group canvas and its fish manager
                Destroy(toggle.GetComponentInParent<UiFishParameters>().FM.transform.gameObject);
                Destroy(toggle.GetComponentInParent<UiFishParameters>().gameObject);
            }
        }

    }
}
