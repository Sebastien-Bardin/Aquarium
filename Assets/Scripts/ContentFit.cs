using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentFit : MonoBehaviour
{
    private float ContentSize;


    //Resizing the scroll view content size based on the actual content 
    void Update()
    {
        ContentSize = 0;
        foreach (RectTransform child in transform.GetComponentsInChildren<RectTransform>())
        {
            if(child.gameObject.tag == "FishGroupCanvas"){

                ContentSize += child.rect.height;

            }
        }
        
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().rect.width, ContentSize);
    }
}
