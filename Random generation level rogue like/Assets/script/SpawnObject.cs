using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{

    public GameObject[] objects;

    public void Start()
    {
        int rand = Random.Range(0, objects.Length);
        //permet d'instancier les mur des piece comme enfant de la piece complete(le gameobject) au lieu de just instancier les murs
        GameObject instance = (GameObject)Instantiate(objects[rand], new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        instance.transform.parent = transform;
    }

}
