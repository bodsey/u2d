using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            DefeatRules dr = GameObject.FindObjectOfType<DefeatRules>();
            if (dr != null)
                dr.SetCheckpointPos(this.transform.position);
        }
    }
}
