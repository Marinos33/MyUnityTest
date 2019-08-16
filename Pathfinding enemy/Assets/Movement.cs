using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private void FixedUpdate()
    {
        //associe les controls au input ayant les noms suivant 
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        move(moveX, moveY);
    }
    void move(float moveX, float moveY)
    {
        if (Input.GetButton("Horizontal"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * 2, GetComponent<Rigidbody2D>().velocity.y);
        }
        if (Input.GetButton("Vertical"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, moveY * 2);
        }
    }
}
