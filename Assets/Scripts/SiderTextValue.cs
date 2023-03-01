using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SiderTextValue : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Slider slider;
    void Start()
    {
        slider = GetComponentInParent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TMP_Text>().maxVisibleCharacters = 3;
        GetComponent<TMP_Text>().text = slider.value.ToString();
    }
}
