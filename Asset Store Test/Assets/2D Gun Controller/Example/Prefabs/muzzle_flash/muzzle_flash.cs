using UnityEngine;
using System.Collections;

public class muzzle_flash : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        Destroy(gameObject, 0.01f);
    }
}
