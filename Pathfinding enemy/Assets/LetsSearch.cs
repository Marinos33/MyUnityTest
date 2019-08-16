using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class LetsSearch : MonoBehaviour
{
    public AIPath aiPath; 
    //lance la recherche du joueur par la cible lorsque que le joueur rentre dans son rayon
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            aiPath.canSearch = true;
        }
    }
}
