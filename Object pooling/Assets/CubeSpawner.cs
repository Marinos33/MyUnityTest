using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    ObjectPooler objectPooler; // on recupere le script
    private void Start()
    {
        objectPooler = ObjectPooler.Instance; //on recupere un proprieté du script en particulier
    }

    private void FixedUpdate()
    {
        objectPooler.SpawnFromPool("Cube", transform.position, Quaternion.identity); // la boucle de spawn
    }
}
