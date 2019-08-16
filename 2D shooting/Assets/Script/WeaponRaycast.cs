using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=wkKsl1Mfp5M&list=WL&index=4&t=0s a 13:31
// script alternatif au weapon utilisant le raycast(laser) au lieu du instantiate de bullet
public class WeaponRaycast : MonoBehaviour
{
    public Transform Firepoint;
    public LineRenderer line; // composant linerenderer de l'objet line
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        RaycastHit2D hitinfo =  Physics2D.Raycast(Firepoint.position, Firepoint.right); // initialisation du raycast, (tout objet possede un raycast)

        if (hitinfo)
        {
            Debug.Log(hitinfo.transform.name);

            Die enemy = hitinfo.transform.GetComponent<Die>(); // instancie le script enemy dans un objet
            if(enemy != null)
            {
                enemy.die(2);//appel la fonction dans enemy
            }

            /* Instantiate(impactEffect, hitinfo.point, quaternion.identity); pour ajouter un effet lors de le disparition de la bullet 
         attention pas oublier de mettre le public impactEffect ici ET de mettre sur l'effet (qui est une prefab) un script qui la detruit a la fin de l'animation avec
         onanimationfinished()
         */

            line.SetPosition(0, Firepoint.position); // set le debut de la ligne grace au 0
            line.SetPosition(1, hitinfo.point); //set la fin de ligne grace au 1
            
        }
        else
        {
            line.SetPosition(0, Firepoint.position);
            line.SetPosition(1, Firepoint.position + Firepoint.right * 100); //si ne touhce rien continue jusque la distance indiqué
        }

        line.enabled = true; // active le render

        // attend une frame 
        yield return new WaitForSeconds(0.02f); 

        line.enabled = false; //desactive le rendu
    }

}
