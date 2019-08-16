using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=wkKsl1Mfp5M&list=WL&index=4&t=0s
public class Weapon : MonoBehaviour
{
    public Transform Firepoint;
    public GameObject bulletPrefab;

    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, Firepoint.position, Firepoint.rotation);
    }
}
