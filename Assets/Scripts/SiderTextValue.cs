using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SiderTextValue : MonoBehaviour
{

    [SerializeField] private Slider slider;
    void Start()
    {   //getting the slider associated to the text
        slider = GetComponentInParent<Slider>();
    }

    
    void Update()
    {
        //restricing text characters to 3
        GetComponent<TMP_Text>().maxVisibleCharacters = 3;

        //updating the text with the slider value
        GetComponent<TMP_Text>().text = slider.value.ToString();
    }
}
