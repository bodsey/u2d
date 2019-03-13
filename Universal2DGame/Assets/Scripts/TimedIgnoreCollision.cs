using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedIgnoreCollision : MonoBehaviour {

    public float LifeTime = .85f;
    public GameObject goToIgnore;

    private void Update()
    {
        LifeTime -= Time.deltaTime;
        if(LifeTime <= 0)
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), goToIgnore.GetComponent<Collider2D>(), false);
            Destroy(this);
        }
    }
}
