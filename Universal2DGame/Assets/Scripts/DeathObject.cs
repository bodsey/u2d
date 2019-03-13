using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathObject : MonoBehaviour {

    private void die()
    {
        DefeatRules dr = GameObject.FindObjectOfType<DefeatRules>();
        if (dr != null)
        {
            dr.Death();
        }
        else
        {
            Debug.LogWarning("DEATH ZONE WITHOUT DEFEAT RULES !!!!");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            die();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            die();
        }
    }

}
