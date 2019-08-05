
using UnityEngine;

public class smoothcamerafollow : MonoBehaviour
{
    public Transform target; //la position de la ciblea suivre

    public float smoothSpeed = 10f; // la vitesse de la camera quand elle suit, minimum 10 (car multiplié par delta time qui une valeur du style: 1,x)

    public Vector3 offset; //la vision de la caméra

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset; //the position of the camera 
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity ,smoothSpeed * Time.deltaTime); //la vitesse a laquelle la camera suit mais de maniere smooth
        transform.position = smoothedPosition; // position de la camera
        
        //transform.LookAt(target); fait des chose etranges, rend le suvi plus smooth mais fait bouger la rotation de la camera
    }
}
