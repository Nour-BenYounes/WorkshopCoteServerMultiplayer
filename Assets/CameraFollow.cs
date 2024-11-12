using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Vector3 decalage;
    public float smoothSpeed = 0.125f;


    public void decalagestart()
    {
        decalage = transform.position - target.transform.position;
        
    }
    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.transform.position;
            Vector3 desiredCameraPosition = targetPosition + decalage;


            //transform.position = desiredCameraPosition;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredCameraPosition, smoothSpeed);
            transform.position = smoothedPosition;

        }
    }
}
