using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // pour l'ia de l'agent 
using UnityStandardAssets.Characters.ThirdPerson;
//https://www.youtube.com/watch?v=CHV1ymlw-P8&list=WL&index=3&t=0s
public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public Camera cam2; // pour funny things

    public NavMeshAgent agent;

    public ThirdPersonCharacter character; // ref au script exemple 5


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

        agent.updateRotation = false; // désactive la rotation du character
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // clic gauche
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition); // converti la position de la souris en coordonée lu par la camera

           
          

            // met dans le raycast le position de la souris et la varibale raycast
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // move player
                agent.SetDestination(hit.point); // set la position de l'agent sur l'endroit ou on a cliqué avec la souris
            } 
        }

        //pour funny things
        if (GetComponent<Rigidbody>().velocity.x == 0f)
        {
            cam.enabled = true;
            cam2.enabled = false;
        }
        else
        {
            //pour funny things
            cam2.enabled = true;
            cam.enabled = false;
        }

        //pour l'exemple 5
        //si la distance entre le point cliquer par la souris et le personnage est plus grand que la distance entre le personnge et son point d'arret alors il bouge
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity, false, false); 
        }
        else
        {
            character.Move(Vector3.zero,false,false);
           
        }
    }
}
