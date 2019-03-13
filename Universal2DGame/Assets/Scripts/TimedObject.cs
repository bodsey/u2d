using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedObject : MonoBehaviour {

    public float LifeTime = 1f;

	void Update () {
        LifeTime -= Time.deltaTime;
        if (LifeTime <= 0)
            Destroy(gameObject);
	}
}
