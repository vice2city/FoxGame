using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy_Frog : Enemy
{
    private Rigidbody2D frog;
    private Collider2D coll;
    public LayerMask ground;
    public Transform leftpoint, rightpoint;
    public float speed;
    [FormerlySerializedAs("jumpforce")] public float jumpForce;
    private float leftx,  rightx;
    private bool faceLeft = true;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        frog = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnim();
    }

    void Movement()
    {
        if (faceLeft)
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping",true);
                frog.velocity = new Vector2(-speed,jumpForce);
            }
            if (transform.position.x < leftx)
            {
                frog.velocity = new Vector2(speed, jumpForce); 
                transform.localScale = new Vector3(-1,1,1);
                faceLeft = false;
            }
        }
        else
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping",true);
                frog.velocity = new Vector2(speed,jumpForce);
            }
            if (transform.position.x > rightx)
            {
                frog.velocity = new Vector2(-speed, jumpForce); 
                transform.localScale = new Vector3(1,1,1);
                faceLeft = true;
            }
        }
    }

    void SwitchAnim()
    {
        if (anim.GetBool("jumping"))
        {
            if (frog.velocity.y < 0.1f)
            {
                anim.SetBool("jumping",false);
                anim.SetBool("falling",true);
            }
        }

        if (coll.IsTouchingLayers(ground) && anim.GetBool("falling"))
        {
            anim.SetBool("falling",false);
            frog.velocity = new Vector2(0,0);
        }
    }
}
