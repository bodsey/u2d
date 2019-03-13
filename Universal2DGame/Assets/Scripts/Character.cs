using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    [Header("NAVIGATION")]

    [Header("Move parameters")]
    public bool AutomaticHorizontalMove = false;
    public float Speed = 2;
    [Range(0,1)]  public float AirControl = 1;
    public bool CanFly = false;

    [Header("Jump parameters")]
    public bool CanJump = true;
    public float JumpForce = 5;
    public bool CanDoubleJump = true;
    public bool CanWallJump = true;
    public bool CanInfiniteJump = false;
    public string JumpButton = "Jump";

    [Header("CONFRONTATION")]

    [Header("Mario style killing feet ability")]
    [Tooltip("Mario style")] public bool HasKillingFeet = true;
    public float FeetKilllTolerance = .3f;
    public float FeetKillBumpForce = 5;

    [Header("Melee Attack")]
    public bool CanMeleeAttack = true;
    [Range(0, float.PositiveInfinity)] public float MeleeBuildUpTime = 0.35f;
    [Range(0, float.PositiveInfinity)] public float MeleeCooldDownTime = 0.5f;
    [Range(0.1f, 10f)] public float MeleeHitboxSize = 1f;
    public float MeleeAttackRange = 0.2f;
    public GameObject MeleeHitboxModel;
    public string MeleeAttackButton = "Fire1";

    [Header("Shoot")]
    public bool CanShoot = true;
    [Range(0, float.PositiveInfinity)] public float ShootBuildUpTime = 0.35f;
    [Range(0, float.PositiveInfinity)] public float ShootCooldDownTime = 0.5f;
    public bool ButtonCanStayPressedToShoot = true;
    [Range(0.1f, 10f)] public float BulletSize = 1f;
    public float BulletLifeTime = 0.2f;
    public float BulletSpeed = 3;
    public GameObject BulletModel;
    public string ShootButton = "Fire1";
    public bool AlwaysShootRight = false;

    [Header("REFLEXION")]
    [Header("Grabbing")]
    public bool CanGrab = true;
    public bool GrabOnTouch = true;
    public string GrabbableTag = "Key";
    public string GrabButton = "Fire2";


    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isOnWall = false;
    private int jumpCount = 0;
    private float height;
    private bool facingRight = true;
    private float meleeBuildUp = 0;
    private float meleeCooldown = 0;
    private float shootBuildUp = 0;
    private float shootCooldown = 0;
    private GameObject grabbed = null;
    private GameObject grabbable = null;
    private Vector3 grabOffset;
    private Vector3 initScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        height = transform.localScale.y;
        initScale = transform.localScale;
    }
    public void Update()
    {
        //MOVE
        float hAxis = Input.GetAxis("Horizontal");
        if(Mathf.Abs(hAxis) > .1f)
            facingRight = hAxis >= 0;
        if (AutomaticHorizontalMove)
            hAxis = 1;
        Vector2 oldVel = rb.velocity;

        rb.velocity = new Vector2(hAxis * Speed, rb.velocity.y);
        if(!isGrounded && AirControl < 1)
        {
            rb.velocity = rb.velocity * (AirControl) + oldVel * (1 - AirControl);
        }

        //FLY
        if(CanFly)
        {
            float vAxis = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, vAxis * Speed);
            rb.gravityScale = 0;
        }

        //JUMP
        if(Input.GetButtonDown("Jump"))
        {
            bool isAbleToJump = false;
            if (CanInfiniteJump) isAbleToJump = true;
            if (CanJump && isGrounded) isAbleToJump = true;
            if (CanDoubleJump && !isGrounded && jumpCount == 1) isAbleToJump = true;
            if (CanWallJump && isOnWall) isAbleToJump = true;

            if (isAbleToJump)
            {
                doJump(JumpForce);
            }            
        }

        //FIGHT
        if (HasKillingFeet)
            checkFeetKill();
        if (CanMeleeAttack)
            manageMeleeAttack();
        if (CanShoot)
            manageShoot();

        //REFLEXION
        if (CanGrab)
        {
            if (grabbed != null && grabbed.GetComponent<Rigidbody2D>() != null)
            {
                grabbed.transform.position = this.transform.position + grabOffset;
            }
            if (Input.GetButtonDown(GrabButton))
            {
                if (grabbed == null)
                {
                    if (grabbable != null && !GrabOnTouch)
                    {
                        grab(grabbable);
                        grabbable = null;
                    }
                }
                else
                {
                    ungrab();
                }
            }
        }

        //FLIP SPRITES
        if (facingRight)
            transform.localScale = new Vector3(initScale.x, initScale.y, initScale.z);
        else
            transform.localScale = new Vector3(-initScale.x, initScale.y, initScale.z);
    }
    
    private void manageShoot()
    {
        if(shootBuildUp > 0)
        {
            shootBuildUp -= Time.deltaTime;
            if(shootBuildUp <= 0)
            {
                doShoot();
            }
            return;
        }
        if(shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
            return;
        }
        if (Input.GetButtonDown(ShootButton) || (Input.GetButton(ShootButton) && ButtonCanStayPressedToShoot))
        {
            if (ShootBuildUpTime == 0)
                doShoot();
            else
                shootBuildUp = ShootBuildUpTime;

        }
    }
    private void doShoot()
    {
        if (BulletModel == null)
        {
            Debug.LogWarning("SHOOTING BUT NO HITBOX");
            return;
        }
        GameObject hb = (GameObject)Instantiate(BulletModel);
        hb.transform.localScale = new Vector3(BulletSize, BulletSize, hb.transform.localScale.z);
        hb.transform.position = this.transform.position;// + (facingRight ? new Vector3(0, 0, 0) : new Vector3(-MeleeAttackRange, 0, 0));
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), hb.GetComponent<Collider2D>());
        shootCooldown = ShootCooldDownTime;
        if(BulletSpeed == 0)
            hb.transform.parent = this.transform;
        bool toRight = facingRight;
        if (AlwaysShootRight) toRight = true;
        hb.GetComponent<Rigidbody2D>().velocity = toRight ? new Vector2(BulletSpeed, 0) : new Vector2(-BulletSpeed, 0);
        hb.GetComponent<TimedObject>().LifeTime = BulletLifeTime;
    }
    private void manageMeleeAttack()
    {
        if(meleeBuildUp > 0)
        {
            meleeBuildUp -= Time.deltaTime;
            if (meleeBuildUp <= 0)
                doMeleeAttack();
            return;
        }
        if(meleeCooldown > 0)
        {
            meleeCooldown -= Time.deltaTime;
            return;
        }
        if(Input.GetButtonDown(MeleeAttackButton))
        {
                if (MeleeBuildUpTime == 0)
                    doMeleeAttack();
                else
                    meleeBuildUp = MeleeBuildUpTime;
            
        }
    }
    private void doMeleeAttack()
    {
        if (MeleeHitboxModel == null)
        {
            Debug.LogWarning("ATTACKING BUT NO HITBOX");
            return;
        }
        GameObject hb = (GameObject)Instantiate(MeleeHitboxModel);
        hb.transform.localScale = new Vector3(MeleeHitboxSize, MeleeHitboxSize, hb.transform.localScale.z);
        hb.transform.position = this.transform.position + (facingRight ? new Vector3(MeleeAttackRange, 0, 0) : new Vector3(-MeleeAttackRange, 0, 0));
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), hb.GetComponent<Collider2D>());
        meleeCooldown = MeleeCooldDownTime;
        hb.transform.parent = this.transform;
    }
    private void doJump(float force)
    {
       
        rb.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
        if(!(CanWallJump && isOnWall))
            jumpCount++;
        isGrounded = false;
        isOnWall = false;
    }
    private void checkFeetKill()
    {
        Collider2D[] overlap = Physics2D.OverlapCircleAll(rb.position + new Vector2(0, -height / 2), FeetKilllTolerance);
        foreach (Collider2D c in overlap)
        {
            if (c.gameObject != this.gameObject && c.GetComponent<Hitable>() != null)
            {
                //Destroy(c.gameObject);
                c.GetComponent<Hitable>().TakeHit();
                jumpCount = 0;
                doJump(FeetKillBumpForce);
                break;
            }
        }
    }
    
    private void ungrab()
    {
        grabbed.transform.parent = null;
        //if (grabbed.GetComponent<Rigidbody2D>() != null)
        //grabbed.GetComponent<Rigidbody2D>().isKinematic = false;
        //Physics2D.IgnoreCollision(grabbed.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), false);
        grabbed.AddComponent<TimedIgnoreCollision>().goToIgnore = this.gameObject;
        
        grabbed = null;
    }
    private void grab(GameObject go)
    {
        if (grabbed != null)
            ungrab();
        if (go.GetComponent<Rigidbody2D>() != null)
        {
            //go.GetComponent<Rigidbody2D>().isKinematic = true;
            go.transform.position = new Vector3(go.transform.position.x, this.transform.position.y, go.transform.position.z);
            grabOffset = go.transform.position - this.transform.position;
            
        }
        Physics2D.IgnoreCollision(go.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        go.transform.parent = this.transform;
        grabbed = go;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //CHECK GRAB
        if(CanGrab && collision.gameObject.tag == GrabbableTag)
        {
            if (GrabOnTouch)
                grab(collision.gameObject);
            else
                grabbable = collision.gameObject;
        }

        //CHECK LANDING
        Collider2D[] overlap = Physics2D.OverlapCircleAll(rb.position + new Vector2(0, -height / 2), .2f);
        foreach (Collider2D c in overlap)
        {
            if (c.gameObject != this.gameObject)
            {               
                isGrounded = true;
                jumpCount = 0;
                break;
            }
        }

        //CHECK WALL
        overlap = Physics2D.OverlapBoxAll(rb.position, new Vector2(Mathf.Abs(transform.localScale.x) + .1f, 0.1f), 0f);
        foreach (Collider2D c in overlap)
        {
            if (c.gameObject != this.gameObject)
            {
                isOnWall = true;
                break;
            }
        }

        
    }
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
        isOnWall = false;
    }

}
