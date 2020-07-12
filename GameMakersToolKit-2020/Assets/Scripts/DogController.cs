using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DogController : MonoBehaviour
{
    public float groundCheckDistance;
    public float wallCheckDistance;
    public float walkSpeed;
    public float visionVert;
    public float visionHor;

    private bool facingRight;
    private bool chasingLeft;
    private bool chasingRight;
    
    private GameObject cat;
    private Rigidbody2D rb2d;
    private BoxCollider2D collider;
    private SpriteRenderer spriteRenderer;
    private RaycastHit2D[] result;
    private Vector2 down;
    private Vector2 side;
    private Vector2 colliderOffset;
    private Vector2 colliderOffsetVert;

    public Sprite idle;
    public Sprite walking1;
    public Sprite walking2;

    private GameObject redEx;

    private System.Random rand;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D> ();
        collider = GetComponent<BoxCollider2D> ();
        spriteRenderer = GetComponent<SpriteRenderer>();
        result = new RaycastHit2D[2];
        cat = GameObject.FindWithTag("Cat");
        down = new Vector2(0, groundCheckDistance);
        side = new Vector2(wallCheckDistance, 0);
        colliderOffset = new Vector2(collider.size.x/2, 0);
        colliderOffsetVert = new Vector2(0, collider.size.y/3);
        facingRight = true;
        chasingLeft = false;
        chasingRight = false;
        InvokeRepeating("change_walking_sprite", 0, 0.6f);


        rand = new System.Random();
        redEx = this.gameObject.transform.GetChild(0).gameObject;
    }

    void Update() {
        /* change direction of sprite */
        if (!facingRight) {
            spriteRenderer.flipX = true;
        } else {
            spriteRenderer.flipX = false;
        }


        if((chasingLeft || chasingRight) && !GetComponent<AudioSource>().isPlaying){
            GetComponent<AudioSource>().Play();
        }

        if(chasingLeft || chasingRight) {
            redEx.SetActive(true);
        } else {
            redEx.SetActive(false);
        }
    }

    /* Flips between different walking sprites*/
    void change_walking_sprite() {
        if (spriteRenderer.sprite == walking1) {
            spriteRenderer.sprite = walking2;
        } else {
            spriteRenderer.sprite = walking1;
        }
    }

    void FixedUpdate()
    {
        Debug.DrawLine(rb2d.position, cat.transform.position, Color.red, 0);

        Debug.DrawLine(rb2d.position + new Vector2(-visionHor, visionVert), rb2d.position + new Vector2(visionHor, visionVert), Color.blue, 0);
        Debug.DrawLine(rb2d.position + new Vector2(-visionHor, -visionVert), rb2d.position + new Vector2(visionHor, -visionVert), Color.blue, 0);

        int hits = Physics2D.LinecastNonAlloc(rb2d.position, cat.transform.position, result, LayerMask.GetMask("Ground"));
        if (hits == 0 && Mathf.Abs(cat.transform.position.x - this.transform.position.x) < visionHor && 
            Mathf.Abs(cat.transform.position.y - this.transform.position.y) < visionVert) {
            facingRight = cat.transform.position.x > this.transform.position.x;
            if (facingRight) {
                if (chasingLeft) {
                    chasingLeft = false;
                    cat.GetComponent<CatController>().dogRight = false;

        
                }
                cat.GetComponent<CatController>().dogLeft = true;
                chasingRight = true;
            } else {
                if (chasingRight) {
                    chasingRight = false;
                    cat.GetComponent<CatController>().dogLeft = false;

        
                }
                cat.GetComponent<CatController>().dogRight = true;
                chasingLeft = true;
            }
            
            walk();
        } else {
            if (chasingRight || chasingLeft) {
                chasingRight = false;
                chasingLeft = false;
                cat.GetComponent<CatController>().dogLeft = false;
                cat.GetComponent<CatController>().dogRight = false;
                GetComponent<AudioSource>().Stop();
            }
            patrol();
        }
    }

    void patrol() {
        if (wallOnLeft() || !groundOnLeft()) {
            facingRight = true;
        }
        if (wallOnRight() || !groundOnRight()) {
            facingRight = false;
        }
        walk();
    }

    void walk() {
        if (facingRight) {
            rb2d.velocity = new Vector2(walkSpeed, rb2d.velocity.y);
        } else {
            rb2d.velocity = new Vector2(-walkSpeed, rb2d.velocity.y);
        }
    }

    bool wallOnLeft() {
        Debug.DrawLine(rb2d.position - colliderOffsetVert, rb2d.position - side - colliderOffsetVert, Color.red, 0);
        int hits = Physics2D.LinecastNonAlloc(rb2d.position, rb2d.position - side, result, LayerMask.GetMask("Ground"));
        return hits >= 1;
    }

    bool wallOnRight() {
        Debug.DrawLine(rb2d.position - colliderOffsetVert, rb2d.position + side - colliderOffsetVert, Color.red, 0);
        int hits = Physics2D.LinecastNonAlloc(rb2d.position - colliderOffsetVert, rb2d.position + side - colliderOffsetVert, result, LayerMask.GetMask("Ground"));
        return hits >= 1;
    }

    bool groundOnLeft() {
        Debug.DrawLine(rb2d.position - colliderOffset, rb2d.position - down - colliderOffset, Color.red, 0);
        int hits = Physics2D.LinecastNonAlloc(rb2d.position - colliderOffset, rb2d.position - down - colliderOffset, result, LayerMask.GetMask("Ground"));
        return hits >= 1;
    }

    bool groundOnRight() {
        Debug.DrawLine(rb2d.position + colliderOffset, rb2d.position - down + colliderOffset, Color.red, 0);
        int hits = Physics2D.LinecastNonAlloc(rb2d.position + colliderOffset, rb2d.position - down + colliderOffset, result, LayerMask.GetMask("Ground"));
        return hits >= 1;
    }
}
