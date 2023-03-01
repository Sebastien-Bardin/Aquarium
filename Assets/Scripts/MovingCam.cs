using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        if (Input.GetMouseButton(0) && !IsOverUi(Input.mousePosition))
        {
            Vector2 mousePos = new Vector2(Input.GetAxis("Mouse X") * MouseSensitivity, Input.GetAxis("Mouse Y") * MouseSensitivity);
            Rotation.y = Rotation.y+mousePos.x;
            Rotation.x = Rotation.x-mousePos.y;

            transform.localEulerAngles = Rotation;
            
        }
      
        //Fixing the target
        transform.position = Target.position - transform.forward * TargetDist;

        
    }

     private bool IsOverUi(Vector3 pos)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        PointerEventData eventPosition = new PointerEventData(EventSystem.current);
        eventPosition.position = new Vector2(pos.x,pos.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventPosition, results);
        Debug.Log(results.Count);

        return results.Count > 0;
    }
}
