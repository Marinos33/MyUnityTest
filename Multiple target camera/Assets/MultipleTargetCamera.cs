using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=aLpixrPvlB8&list=WL&index=8&t=0s
[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    public List<Transform> targets; //les cibles a suivre
    public Vector3 offset; // les limite de la camera voulu
    public float smoothTime = 0.5f; // temps de suivi de la camera
    private Vector3 velocity; // utliser pour le smoothdamp mais rien a faire avec
    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;
    private Camera cam; // objet camera

    private void Start()
    {
        cam = GetComponent<Camera>(); // donne l'objet camera a cam
    }
    private void LateUpdate()
    {
        //si pas de cibles/joueurs, ne fais rien
        if (targets.Count == 0)
        {
            return;
        }

        MOve();
        Zoom();
    }

    private void MOve()
    {
        Vector3 centerPoint = GetCenterPoint(); // la position du milieu de l'ecran entre les 2 joueurs
        Vector3 newposition = centerPoint + offset; // calcule de la position de la camera aprés mouvements des joueurs
        transform.position = Vector3.SmoothDamp(transform.position, newposition, ref velocity, smoothTime); // application de la nouvelle position + some smoothness
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter); // ajuste le zoom
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    //met le zoom en fonction de la distance entre les joueurs les plus eloignés
    float GetGreatestDistance()
    {
        Bounds bounds = new Bounds(targets[0].position, Vector3.zero); //limite de la zone de deplacement des joueurs/camera 
        //boucle qui passe et set les limite pour tous les joueur
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position); // set les limites
        }

        return Mathf.Max(bounds.size.x, bounds.size.y); // renvoie la la largeur
    }
    private Vector3 GetCenterPoint()
    {
        //si 1 seul cible, suit la cible
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        Bounds bounds = new Bounds(targets[0].position, Vector3.zero); //limite de la zone de deplacement des joueurs/camera 
        //boucle qui passe et set les limite pour tous les joueur
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position); // set les limites
        }

        return bounds.center; //renvoie le centre
    }
}
