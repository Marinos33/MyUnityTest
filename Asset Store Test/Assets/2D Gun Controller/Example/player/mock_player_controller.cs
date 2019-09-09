using UnityEngine;
using System.Collections;

/*=============================================================================
*   This is a "mock" player controller script used for example purposes.
*   The only purpose of this script is to set animator boolean parameters based
*   on certain keys being pressed, so that a you, the user, can see how you
*   can adjust accuracy depending on the stance or motion that player is in.
*
*   Because this is only a "mock" script, it is not intended for use in any
*   production code as is. 
*   But you are welcome to use it as a base or what have you.
=============================================================================*/

public class mock_player_controller : MonoBehaviour {

    public Animator m_Anim;             // Reference to the player's animator component.

    public void move(bool crouching, bool jumping, bool sprinting, bool walking)
    {
        m_Anim.SetBool("Grounded", !jumping);
        m_Anim.SetBool("Crouching", crouching);
        m_Anim.SetBool("Sprinting", sprinting);
        m_Anim.SetBool("Walking", walking);

        if (!jumping && !sprinting && !walking)
        {
            m_Anim.SetBool("Standing", true);
        }
        else
        {
            m_Anim.SetBool("Standing", false);
        }
    }

    // Update is called once per frame
    void Update () {
        //Checking if the player is crouching
        bool crouching = Input.GetKey(KeyCode.LeftControl);

        //Checking if the player is sprinting
        bool sprinting = Input.GetKey(KeyCode.LeftShift);

        bool walking = Input.GetKey(KeyCode.A) ? true : Input.GetKey(KeyCode.D);

        //Checking if the player is jumping
        bool jumping = Input.GetKey(KeyCode.Space);

        //Sending results
        move(crouching, jumping, sprinting, walking);
    }
}
