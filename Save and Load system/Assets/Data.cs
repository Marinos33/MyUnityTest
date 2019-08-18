using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=XOjd_qU2Ido&list=WL&index=2&t=0s
[System.Serializable]
public class Data 
{
    public float[] scale;
    public int test = 0;
    public string check = "it is not changed yet";

    public Data(someValues values)
    {
        scale = new float[3];
        scale[0] = values.transform.position.x;
        scale[1] = values.transform.position.y;
        scale[2] = values.transform.position.z;

        test = values.test;
        check = values.check;
    }
}
