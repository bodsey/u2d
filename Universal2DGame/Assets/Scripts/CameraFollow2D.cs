using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour {

    public float FollowSpeed = 5;
    public GameObject Target;

    private void Start()
    {
        if(Target == null)
        {
            Target = GameObject.FindGameObjectWithTag("Player");
        }
    }
    private void Update()
    {
        Vector3 targetPos = new Vector3(Target.transform.position.x, Target.transform.position.y, this.transform.position.z);

        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, FollowSpeed * Time.deltaTime);
    }
}
