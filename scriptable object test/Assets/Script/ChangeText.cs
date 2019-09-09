using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeText : MonoBehaviour
{
    public ScriptObject ob;

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            ob.hasChanged = "changed";
        }
    }
}
