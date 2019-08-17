using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=whzomFgjT50&list=WL&index=11&t=0s
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    public Rigidbody2D rb;
    public Animator anim;
    Vector2 movement;

    //input
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //set les valeur des variable de l'animator en fonction du mouvement
        anim.SetFloat("Hoirzontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("speed", movement.sqrMagnitude); //movement.sqrmagnitude et plus opti que movement.magnitude
    }

    //move
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime); // move le rigidbody avec son objet, position actuel + movement * la vitesse * un timer fixe qui s'adapte au fps

    }
}
