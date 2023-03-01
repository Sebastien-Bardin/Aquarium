using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCam : MonoBehaviour
{
    [SerializeField] private float MouseSensitivity = 1.0f;
    [SerializeField] private Transform Target;
    [SerializeField] private float TargetDist = 30.0f;
    private Vector2 Rotation;
    private float RotationY;
    private float RotationX;

    // Moving the camera around the target
    void Update()
    {
        //Zooming with scroll
        TargetDist -= Input.mouseScrollDelta.y; 

        //if the left click is pressed then get X and Y mous axis and rotate the camera auraound
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = new Vector2(Input.GetAxis("Mouse X") * MouseSensitivity, Input.GetAxis("Mouse Y") * MouseSensitivity);
            Rotation.y = Rotation.y+mousePos.x;
            Rotation.x = Rotation.x-mousePos.y;

            transform.localEulerAngles = Rotation;
            
        }
      
        //Fixing the target
        transform.position = Target.position - transform.forward * TargetDist;

        
    }
}
