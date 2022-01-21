using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    private Vector3 cameraOffset;
    [Range(0.01f, 1.0f)]
    public bool lookAtPlayer = false;

    public bool rotateAroundPlayer = true;
    public float rotationSpeed = 5.0f;

    private bool cameraRay;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - playerTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (rotateAroundPlayer)
        {
            Quaternion cameraTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up);
            cameraOffset = cameraTurnAngle * cameraOffset;
        }

        Vector3 newPosition = playerTransform.position + cameraOffset;
        transform.position = newPosition;
        // With smoothFactor
        //transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);

        if (lookAtPlayer || rotateAroundPlayer)
        {
            transform.LookAt(playerTransform);
        }


        // TO DO - Walls management
        /*
        LayerMask layers = 1 << gameObject.layer;
        layers = ~layers;
        cameraRay = Physics.Raycast(transform.position, playerTransform.position);
        Debug.DrawLine(transform.position, playerTransform.position, Color.red,1, false);
        Ray ray = Camera.main.ScreenPointToRay(playerTransform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log("Rayed on " + hit.transform.name);
        }
        else
        {
            //Debug.Log("Nothing hit");
        }
        */
    }
}
