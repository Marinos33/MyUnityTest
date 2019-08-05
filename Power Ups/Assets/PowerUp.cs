using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=CLSiRf_OrBk&list=WL&index=10&t=2s
public class PowerUp : MonoBehaviour
{
    public float multiplier = 1.4f;
    public GameObject pickupEffect;
    public float duration = 4f;

    // fonction pour savoir quand un objet rentre en contact avec cette objet
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // si le tag de l'objet avec lequel il ya collision est "player" alors...
        {
            StartCoroutine (PickUp(other)); //startcoroutine pour un ienumerator
        }
    }

    private IEnumerator PickUp(Collider2D player)
    {
        //effet de ramassage (VFX)
        Instantiate(pickupEffect, transform.position, transform.rotation);


        //application du powerup sur le player
        player.transform.localScale *= multiplier;
        UsellesStats playerStats = player.GetComponent<UsellesStats>();
        playerStats.stat *= multiplier;

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        //bonus a durée limité
        yield return new WaitForSeconds(duration); // le IEenumerator est necessaire pour cette commande qui met en pause le script

        //annulé le bonus
        playerStats.stat /= multiplier;

        //detruit le power up
        Destroy(gameObject);
    }
}
