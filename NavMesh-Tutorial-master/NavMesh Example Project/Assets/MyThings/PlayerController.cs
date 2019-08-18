using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // pour l'ia de l'agent 
//https://www.youtube.com/watch?v=CHV1ymlw-P8&list=WL&index=3&t=0s
public class PlayerController : MonoBehaviour
{
    public Camera cam;

    public NavMeshAgent agent;


    //pour la scene exemple3
    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // clic gauche
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition); // converti lea position de la souris en coordonée lu par la camera
                                                                 // initilaisation d'une variable raycast

            // met dans le raycast le position de la souris et la varibale raycast
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // move player
                agent.SetDestination(hit.point); // set la position de l'agant sur l'endroit ou on a cliqué avec la souris

            }

        }
    }
}
