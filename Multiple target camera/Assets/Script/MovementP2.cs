using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementP2 : MonoBehaviour
{
    private void FixedUpdate()
    {
        //associe les controls au input ayant les noms suivant 
        float moveX = Input.GetAxisRaw("Horizontal2");
        float moveY = Input.GetAxisRaw("Vertical2");

        move(moveX, moveY);
    }
    void move(float moveX, float moveY)
    {
        if (Input.GetButton("Horizontal2"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * 2, GetComponent<Rigidbody2D>().velocity.y);
        }
        if (Input.GetButton("Vertical2"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, moveY * 2);
        }
    }
}
