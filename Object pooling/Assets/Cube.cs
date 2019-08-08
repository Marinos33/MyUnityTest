using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* dans le cas de ce projet unity l'utilisation d'une interface n'est pas obligatoire et peut etre remplacer par OnEnabled car cette fonction s'active a l'activation (setactive(true)) de l'objet 
ce qui est le cas dans le fonctionnement du script principal (ObjectPooler) qui appel cette objet */

public class Cube : MonoBehaviour, IPooledObject
{

    public float upForce = 1f;
    public float sideForce = .1f;

    // les forces appliquer sur les objets au spawn
    public void onObjectSpawn() // peut etre remplacer par OnEnabled
    {
        float xForce = Random.Range(-sideForce, sideForce);
        float yForce = Random.Range(upForce / 2f, upForce);
        //float zForce = Random.Range(-sideForce, sideForce);

        Vector2 force = new Vector3(xForce, yForce);

        GetComponent<Rigidbody2D>().velocity = force;
    }

}
