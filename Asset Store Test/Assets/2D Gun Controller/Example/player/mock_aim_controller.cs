using UnityEngine;
using System.Collections;

/*=============================================================================
*   This is a "mock" aim controller script used for example purposes.
*   The only purpose of this script is to rotate the gun in the Example scene.
*
*   Because this is only a "mock" script, it is not intended for use in any
*   production code. But you are welcome to use it as a base or what have you.
=============================================================================*/

public class mock_aim_controller : MonoBehaviour {

    public Transform muzzle;
    public Transform arm;
    public float off_set = 0;

    // Update is called once per frame
    void Update () {
        float rotZ;

        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(muzzle.position);
        rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        arm.rotation = Quaternion.Euler(0f, 0f, rotZ + off_set);
    }
}
