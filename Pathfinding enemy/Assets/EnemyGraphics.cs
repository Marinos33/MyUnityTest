using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; //pas oublier pour manipuler le pathfinding
//https://www.youtube.com/watch?v=jvtFUfJ6CP8&list=WL&index=1
public class EnemyGraphics : MonoBehaviour
{
    public AIPath aiPath; // refenrence au gameobject contenant l'ia
    bool isFacingRight = false;
    // Update is called once per frame
    void Update()
    {
        // verifie si la velocity en x tend vers le positif ou negatif et flip le sprite lorsque nécessaire
        if (aiPath.desiredVelocity.x >= 0.01f && isFacingRight) 
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            //transform.localRotation = Quaternion.Euler(0f, -180f, 0f);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            //transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
}
