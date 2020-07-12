using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    public float walkSpeed;
    public float visionVert;
    public float visionHor;

    private bool facingRight;
    private bool fleeingLeft;
    private bool fleeingRight;

    private GameObject cat;
    private Rigidbody2D rb2d;
    private BoxCollider2D collider;
    private SpriteRenderer spriteRenderer;
    private RaycastHit2D[] result;

    public Sprite idle;
    public Sprite moving;
    public Sprite hole;

    private GameObject yellowEx;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D> ();
        collider = GetComponent<BoxCollider2D> ();
        result = new RaycastHit2D[2];
        cat = GameObject.FindWithTag("Cat");

        facingRight = true;
        fleeingLeft = false;
        fleeingRight = false;

        spriteRenderer = GetComponent<SpriteRenderer>();

        yellowEx = this.gameObject.transform.GetChild(0).gameObject;
    }

    void Update() {
        if (rb2d.velocity.x == 0) {
            spriteRenderer.sprite = idle;
        } else {
            spriteRenderer.sprite = moving;
        }


        if (!facingRight) {
            spriteRenderer.flipX = true;
        } else {
            spriteRenderer.flipX = false;
        }

        if (fleeingLeft || fleeingRight) {
            yellowEx.SetActive(true);
        } else {
            yellowEx.SetActive(false);
        }

        if((fleeingLeft || fleeingRight) && !GetComponent<AudioSource>().isPlaying){
            GetComponent<AudioSource>().Play();
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
            facingRight = cat.transform.position.x < this.transform.position.x;
            if (facingRight) {
                if (fleeingLeft) {
                    fleeingLeft = false;
                    cat.GetComponent<CatController>().mouseLeft = false;
                }
                cat.GetComponent<CatController>().mouseRight = true;
                fleeingRight = true;
            } else {
                if (fleeingRight) {
                    fleeingRight = false;
                    cat.GetComponent<CatController>().mouseRight = false;
                }
                cat.GetComponent<CatController>().mouseLeft = true;
                fleeingLeft = true;
            }
            
            flee();
        } else {
            if (fleeingRight || fleeingLeft) {
                fleeingRight = false;
                fleeingLeft = false;
                cat.GetComponent<CatController>().mouseLeft = false;
                cat.GetComponent<CatController>().mouseRight = false;

        
             GetComponent<AudioSource>().Stop();
     
            }
        }
    }

    void flee() {
        if (facingRight) {
            rb2d.velocity = new Vector2(walkSpeed, rb2d.velocity.y);
        } else {
            rb2d.velocity = new Vector2(-walkSpeed, rb2d.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Cat")){
            cat.GetComponent<CatController>().mouseLeft = false;
            cat.GetComponent<CatController>().mouseRight = false;
            Destroy(this.gameObject);
        }
    }
}
