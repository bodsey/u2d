using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitable : MonoBehaviour {

    public int LifePoints = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Hitbox") return;

        Destroy(collision.gameObject);
        TakeHit();
        
    }
    public void TakeHit()
    {
        LifePoints--;
        if (LifePoints <= 0)
            Destroy(gameObject);
    }
}
