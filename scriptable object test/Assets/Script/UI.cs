using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public ScriptObject ob;

    void Update()
    {
        gameObject.GetComponent<Text>().text = ob.hasChanged;
    }
}
