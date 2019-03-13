using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMoveRoutine : MonoBehaviour {
    public float SwitchTime = 2;
    public float WaitingTime = 0.5f;
    public float Speed = 2;
    public bool StartUp = true;

    protected float currentDir = 1;
    private float currentWait = 0;
    protected float currentIdle = 0;
    protected Rigidbody2D rb;


    void Start()
    {
        currentDir = StartUp ? 1 : -1;
        currentWait = SwitchTime;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (currentIdle > 0)
        {
            currentIdle -= Time.deltaTime;
        }
        else if (currentWait > 0)
        {
            currentWait -= Time.deltaTime;
            if (currentWait <= 0)
            {
                if (currentDir == 1 || currentDir == -1)
                {
                    currentIdle = WaitingTime;
                    currentDir = -currentDir;
                    currentWait = SwitchTime;
                }
            }
        }
       

        DoMove();
    }
    void DoMove()
    {
        float nDir = currentIdle > 0 ? 0 : currentDir;
        if (rb != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, nDir * Speed);
        }
        else
        {
            this.transform.Translate(new Vector3(0, nDir * Speed * Time.deltaTime, 0));
        }
    }
}
