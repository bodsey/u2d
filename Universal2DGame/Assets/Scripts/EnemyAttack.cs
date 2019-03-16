using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Tooltip("Punchzone or bullet")]
    public GameObject HitBox;
    public float HitBoxSize;
    public float AttackSpeed;
    public Vector2 AttackDirection;
    [Tooltip("in seconds")] public float TimeBetweenAttacks = 1f;
    [Tooltip("in seconds")] public float PreparationTime = 0.2f;
    public bool DoesPreparationStun = true;

    private float timer;
    private int currentState; //0 = wait 1 = prep
    private Rigidbody2D thisRb;
    private Color _color;
    private void Start()
    {
        timer = TimeBetweenAttacks;
        thisRb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(timer >= 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                switch(currentState)
                {
                    case 0:
                        {
                            currentState = 1;
                            timer = PreparationTime;
                            ShowPrep();
                            break;
                        }
                    case 1:
                        {
                            currentState = 0;
                            timer = TimeBetweenAttacks;
                            Shoot();
                            break;
                        }
                }
            }
        }
    }
    private void ShowPrep()
    {
        if(DoesPreparationStun)
        {
            thisRb.velocity = Vector2.zero;
        }
        _color = this.GetComponentInChildren<SpriteRenderer>().color;
        this.GetComponentInChildren<SpriteRenderer>().color = Color.red;
    }
    private void Shoot()
    {
        GameObject nBullet = (GameObject)Instantiate(HitBox, this.transform.position, Quaternion.identity);
        nBullet.transform.localScale = Vector3.one * HitBoxSize;
        Rigidbody2D rb = nBullet.GetComponent<Rigidbody2D>();
        rb.velocity = AttackDirection * AttackSpeed;
        this.GetComponentInChildren<SpriteRenderer>().color = _color;
        
    }
}
