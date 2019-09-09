using UnityEngine;
using System.Collections;

public class bullet_trail : MonoBehaviour {

    public int bullet_speed = 230;

	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.right * Time.deltaTime * bullet_speed);
        Destroy(gameObject, 1);
	}
}
