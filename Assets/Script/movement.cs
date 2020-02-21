using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class movement : MonoBehaviour
{
    private Rigidbody2D fox;
    private Animator anim;
    public AudioSource jumpAudio,hurtAudio,cherryAudio;

    public Collider2D coll, disColl;
    public Transform cellingCheck;
    public LayerMask ground;
    public float speed;
    [FormerlySerializedAs("jumpforce")] public float jumpForce;
    public int cherry;
    [FormerlySerializedAs("ishurt")] public bool isHurt;

    [FormerlySerializedAs("cherrynum")] public Text cherryNum;
    
    // Start is called before the first frame update
    void Start()
    {
        fox = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isHurt)
        {
            Movement();
        }
        
        SwitchAnim();
        Jump();
    }

    private void Update()
    { 
        Crouch();
    }

    void Movement()
    {
        float inputmovement = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");

        // 角色移动
        if (inputmovement != 0)
        {
            fox.velocity = new Vector2(inputmovement * speed * Time.fixedDeltaTime ,fox.velocity.y);
            anim.SetFloat("running",Mathf.Abs(facedirection));
        }

        // 角色转向
        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection,1,1);
        }

    }

    void SwitchAnim()
    {
        if (fox.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", true);
        }
        if (anim.GetBool("jumping"))
        {
            if (fox.velocity.y < 0)
            {
                anim.SetBool("jumping",false);
                anim.SetBool("falling",true);
            }
        }else if (isHurt)
        {
            if (Mathf.Abs(fox.velocity.x) < 0.1f)
            {
                isHurt = false;
                anim.SetBool("hurt",false);
            }
        }else if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling",false);
        }
    }

    // 角色跳跃
    void Jump()
    {
     
        if (Input.GetButton("Jump") && coll.IsTouchingLayers(ground))
        {
            fox.velocity = new Vector2(fox.velocity.x,jumpForce * Time.fixedDeltaTime);
            anim.SetBool("jumping",true);
            jumpAudio.Play();
        }
    }
    
    //角色爬行
    void Crouch()
    {
        float inputcrouch = Input.GetAxisRaw("Vertical");
        if (!Physics2D.OverlapCircle(cellingCheck.position,0.2f,ground))
        {
            if (inputcrouch < 0)
            {
                anim.SetBool("crouching", true);
                disColl.enabled = false;
            }
            else
            {
                anim.SetBool("crouching", false);
                disColl.enabled = true;
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //收集物品
        if (collision.tag == "Collections")
        {
            Destroy(collision.gameObject);
            cherryAudio.Play();
            cherry += 1;
            cherryNum.text = cherry.ToString();
        }
        //死亡判定
       if (collision.tag == "DeadLine")
        {
            GetComponent<AudioSource>().enabled = false;
            Invoke("restart",2f);
        }
        
    }

    //消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemies")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling"))
            {
                enemy.JumpOn();
                fox.velocity = new Vector2(fox.velocity.x,jumpForce * Time.deltaTime);
                anim.SetBool("jumping",true);
            }else if (transform.position.x < collision.gameObject.transform.position.x) 
            {
                fox.velocity = new Vector2(-10, fox.velocity.y);
                isHurt = true;
                hurtAudio.Play();
                anim.SetBool("hurt",true);
            }else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                fox.velocity = new Vector2(10, fox.velocity.y);
                isHurt = true;
                anim.SetBool("hurt",true);
            }
        }
            
    }

    //死亡判定
    void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}