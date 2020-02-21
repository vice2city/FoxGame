using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected AudioSource deathAudio;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        deathAudio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }
    
    public void JumpOn()
    {
        anim.SetTrigger("death");
        deathAudio.Play();
    }

    public void death()
    {
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }
}
