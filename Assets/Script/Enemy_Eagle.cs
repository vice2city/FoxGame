using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{
    private Rigidbody2D eagle;
    private Collider2D coll;
    public Transform toppoint, bottompoint;
    public float speed;
    private float topy, bottomy;
    private bool facetop = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        eagle = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        topy = toppoint.position.y;
        bottomy = bottompoint.position.y;
        Destroy(toppoint.gameObject);
        Destroy(bottompoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (facetop)
        {
            eagle.velocity = new Vector2(eagle.velocity.x, speed);
            if (transform.position.y > topy)
            {
                eagle.velocity = new Vector2(eagle.velocity.x, -speed);
                facetop = false;
            }
        }
        else
        {
            eagle.velocity = new Vector2(eagle.velocity.x, -speed);
            if (transform.position.y < bottomy)
            {
                eagle.velocity = new Vector2(eagle.velocity.x, speed);
                facetop = true;
            }
        }
    }
}
