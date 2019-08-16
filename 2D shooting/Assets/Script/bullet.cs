using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=wkKsl1Mfp5M&list=WL&index=4&t=0s
public class bullet : MonoBehaviour
{

    public float speed = 20f;
    public Rigidbody2D rb;
    public int damage = 2;
    void Start()
    {
        rb.velocity = transform.right * speed; //bouge la bullet vers sa droite
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        
        Die ennemy = hitInfo.GetComponent<Die>();
        Debug.Log(hitInfo.name + ennemy.health); // affiche la chose toucher dans la console
        if (ennemy != null)
        {
            ennemy.die(damage);
        }

        /* Instantiate(impactEffect, transform.position, transform.rotation); pour ajouter un effet lors de le disparition de la bullet 
         attention pas oublier de mettre le public impactEffect ici ET de mettre sur l'effet (qui est une prefab) un script qui la detruit a la fin de l'animation avec
         onanimationfinished()
         */

        Destroy(gameObject);
    }


}
