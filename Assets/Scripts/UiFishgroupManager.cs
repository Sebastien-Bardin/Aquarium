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


    public void AddGroup(){
       GameObject newCanvas =  Instantiate(FishGroupCanvasPrefab, content.transform.position, Quaternion.identity);
       newCanvas.transform.SetParent(content.transform, false);

       GameObject NewFishGroup = Instantiate(FishManager, Vector3.zero, Quaternion.identity);

       newCanvas.gameObject.GetComponent<UiFishParameters>().FM = NewFishGroup.GetComponent<FishManager>();
    }

    public void RemoveGroup(){

        foreach (Toggle toggle in content.GetComponentsInChildren<Toggle>())
        {
            if (toggle.isOn)
            {
                Destroy(toggle.GetComponentInParent<UiFishParameters>().FM.transform.gameObject);
                Destroy(toggle.GetComponentInParent<UiFishParameters>().gameObject);
            }
        }

    }
}
