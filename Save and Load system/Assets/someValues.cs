using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class someValues : MonoBehaviour
{
    public float[] scale = { 5, 5, 5 };
    public int test = 0;
    public string check = "it is not changed yet";

    private void Update()
    {
        transform.localScale = new Vector3(scale[0], scale[1], scale[2]);
    }

    public void save()
    {
        SaveSystem.Save(this);
    }

    public void load()
    {
        Data data = SaveSystem.Load();

        test = data.test;
        check = data.check;

        Vector3 Scales;

        Scales.x = data.scale[0];
        Scales.y = data.scale[1];
        Scales.z = data.scale[2];
        transform.position = Scales;

    }
}
