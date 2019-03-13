using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public int Value = 1;
    public bool DestroyOnTouch = true;

	private void getCoin()
    {
        CoinsManager cm = GameObject.FindObjectOfType<CoinsManager>();
        if (cm != null)
        {
            cm.GetCoin(Value);
        }
        else
            Debug.LogWarning("COIN SCRIPT BUT NO COINS MANAGER");
        if (DestroyOnTouch)
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            getCoin();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            getCoin();
        }
    }
}
