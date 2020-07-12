using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatController : MonoBehaviour
{
    public GameObject LostScreen;

    public float groundCheckDistance;
    public float maxPounceDistance;
    public float maxPouncePrepareTime;
    public float minPounceHeight;
    public float maxPounceHeight;
    public float minSidePounceHeight;
    public float sidePounceVelX;
    public float sidePounceVelY;
    public float downPounceVelX;
    public float downPounceVelY;

    public bool dogRight;
    public bool dogLeft;
    public bool mouseRight;
    public bool mouseLeft;

    private bool facingRight;
    private float pouncePrepareTime;
    private bool preparingPounceUp;
    private bool preparingPounceSide;
    private bool preparingPounceDown;

    private GameObject laser;
    private Rigidbody2D rb2d;
    private BoxCollider2D collider;
    private SpriteRenderer spriteRenderer;
    private RaycastHit2D[] result;
    private Vector2 down;
    private Vector2 colliderOffset;

    public Sprite idle;
    public Sprite walking1;
    public Sprite walking2;
    public Sprite readyToJump;
    public Sprite jumping;
    public Sprite falling;

    private GameObject redEx;
    private GameObject yellowEx;

    public enum State
    {
        idle,
        walking,
        preparingPounceUp,
        preparingPounceSide,
        preparingPounceDown,
        jumping,
        fleeing,
        hiding,
        chasing
    }

    public State state;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D> ();
        collider = GetComponent<BoxCollider2D> ();
        spriteRenderer = GetComponent<SpriteRenderer> ();
        result = new RaycastHit2D[2];
        laser = GameObject.FindWithTag("Laser");
        down = new Vector2(0, groundCheckDistance);
        colliderOffset = new Vector2(collider.size.x/2, 0);
        facingRight = true;
        dogRight = false;
        dogLeft = false;
        mouseRight = false;
        mouseLeft = false;
        resetPounce();
        InvokeRepeating("change_walking_sprite", 0, 0.3f);
        redEx = this.gameObject.transform.GetChild(0).gameObject;
        yellowEx = this.gameObject.transform.GetChild(1).gameObject;
    }

    void Update() {

        /* Update sprite for different cat actions */
        switch (state) {
            case State.idle:
                spriteRenderer.sprite = idle;
                break;

            case State.preparingPounceUp:
                spriteRenderer.sprite = readyToJump;
                break;

            case State.preparingPounceDown:
                spriteRenderer.sprite = readyToJump;
                break;

            case State.preparingPounceSide:
                spriteRenderer.sprite = readyToJump;
                break;

            case State.jumping:
                if (rb2d.velocity.y > 0) {
                    spriteRenderer.sprite = jumping;

                } else {
                    spriteRenderer.sprite = falling;
                }
                break;
        }

        if (state == State.fleeing || state == State.hiding) {
            redEx.SetActive(true);
            yellowEx.SetActive(false);
        } else if (state == State.chasing) {
            redEx.SetActive(false);
            yellowEx.SetActive(true);
        } else {
            redEx.SetActive(false);
            yellowEx.SetActive(false);
        }


        /* change direction of sprite */
        if (facingRight) {
            spriteRenderer.flipX = true;
        } else {
            spriteRenderer.flipX = false;
        }
    }

    /* Flips between different walking sprites*/
    void change_walking_sprite() {
        if (state == State.walking || state == State.fleeing || state == State.chasing) {
            
            if (spriteRenderer.sprite == walking1) {
                spriteRenderer.sprite = walking2;
            } else {
                spriteRenderer.sprite = walking1;
            }
        }
    }

    void FixedUpdate()
    {
        Debug.DrawLine(rb2d.position, laser.transform.position, Color.red, 0);
        int hits = Physics2D.LinecastNonAlloc(rb2d.position, laser.transform.position, result, LayerMask.GetMask("Ground"));
        if (CheckGrounded() && (dogLeft || dogRight)) {
            state = State.fleeing;
            flee();
        } else if (CheckGrounded() && (mouseLeft || mouseRight)) {
            state = State.chasing;
            chaseMouse();
        } else if (laser.GetComponent<LaserController>().laserOn && hits == 0 && CheckGrounded() && 
            laser.transform.position.y - this.transform.position.y < maxPounceHeight &&
            !(Mathf.Abs(laser.transform.position.x - this.transform.position.x) < 0.5 && laser.transform.position.y - this.transform.position.y <= minPounceHeight)) {
            facingRight = laser.transform.position.x > this.transform.position.x;
            chaseLaser();
        } else {
            if (CheckGrounded()) {
                state = State.idle;
            } else {
                state = State.jumping;
            }
        }
    }

    void resetPounce() {
        state = State.idle;
        pouncePrepareTime = maxPouncePrepareTime;
    }
    
    void flee() {
        if (dogLeft && !dogRight) {
            rb2d.velocity = new Vector2(5, rb2d.velocity.y);
            facingRight = true;
        } else if (!dogLeft && dogRight) {
            rb2d.velocity = new Vector2(-5, rb2d.velocity.y);
            facingRight = false;
        } else {
            state = State.hiding;
        }
    }

    void chaseMouse() {
        if (mouseRight) {
            rb2d.velocity = new Vector2(5, rb2d.velocity.y);
            facingRight = true;
        } else {
            rb2d.velocity = new Vector2(-5, rb2d.velocity.y);
            facingRight = false;
        } 
    }

    void chaseLaser() {
        if (Mathf.Abs(laser.transform.position.x - this.transform.position.x) <= maxPounceDistance) {
            pouncePrepareTime -= Time.fixedDeltaTime;
            if (laser.transform.position.y - this.transform.position.y > minPounceHeight) {
                state = State.preparingPounceUp;
                if (pouncePrepareTime <= 0) {
                    pounceUp();
                }
            } else if (((!groundOnLeft() && !facingRight) || (!groundOnRight() && facingRight)) && 
                       laser.transform.position.y - this.transform.position.y >= minSidePounceHeight) {
                state = State.preparingPounceSide;
                if (pouncePrepareTime <= 0) {
                    pounceSide();
                }
            } else if ((!groundOnLeft() && !facingRight) || (!groundOnRight() && facingRight)){
                state = State.preparingPounceDown;
                if (pouncePrepareTime <= 0) {
                    pounceDown();
                }
            } else {
                resetPounce();
                walkTowardsLaser();
            }
        } else {
            resetPounce();
            walkTowardsLaser();
        }
    }

    void walkTowardsLaser() {
        if (facingRight && groundOnRight()) {
            rb2d.velocity = new Vector2(5, rb2d.velocity.y);
            state = State.walking;
        } else if (!facingRight && groundOnLeft()) {
            rb2d.velocity = new Vector2(-5, rb2d.velocity.y);
            state = State.walking;
        } else {
            state = State.idle;
        }
        
    }

    void pounceUp() {
        float velocityY = Mathf.Sqrt(2 * -Physics2D.gravity.y * rb2d.gravityScale * (laser.transform.position.y - this.transform.position.y));
        float velocityX = (laser.transform.position.x - this.transform.position.x) / (velocityY / (-Physics2D.gravity.y * rb2d.gravityScale));
        rb2d.velocity = new Vector2(velocityX,  velocityY);
        resetPounce();
    }

    void pounceSide() {
        if (facingRight) {
            rb2d.velocity = new Vector2(sidePounceVelX,  sidePounceVelY);
        } else {
            rb2d.velocity = new Vector2(-sidePounceVelX,  sidePounceVelY);
        }
        resetPounce();
    }

    void pounceDown() {
        if (facingRight) {
            rb2d.velocity = new Vector2(downPounceVelX,  downPounceVelY);
        } else {
            rb2d.velocity = new Vector2(-downPounceVelX,  downPounceVelY);
        }
        resetPounce();
    }

    bool CheckGrounded()
    {
        return groundOnRight() || groundOnLeft();
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

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Dog") || other.CompareTag("Spotlight")){
            Debug.Log("LOSE");
            switch_to_loss_scene();
        } 
    }

    void switch_to_loss_scene() {
        this.LostScreen.SetActive(true);
    }
}
