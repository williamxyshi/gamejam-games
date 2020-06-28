using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroom : MonoBehaviour
{

     public Animator animator;

    public float searchDelay;
    public float speed;
    public int moveDirection;
    public int health;
    public float totalInvTime;
    private GameObject player;
    private Rigidbody2D rb2d;
    private RaycastHit2D[] result;
    private bool searching;
    private bool attacking;
    private bool randomMove;
    private float randomTimer;
    private float invTime;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D> ();
        player = GameObject.FindWithTag("Player");
        result = new RaycastHit2D[4];
        searching = false;
        attacking = false;
        randomMove = false;
        randomTimer = 0;
        invTime = 0;
    }

    void Update() {

        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        if (invTime > 0) {
            invTime -= Time.fixedDeltaTime;
        }
        if (!searching) {
            searching = true;
            StartCoroutine("CheckVision");
        }
        if (randomMove && randomTimer <= 0) {
            moveDirection = (int)(Random.value*4);
            randomTimer = Random.value + 1f;
        }
        if (randomTimer > 0) {
            randomTimer -= Time.fixedDeltaTime;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 movement = new Vector2(0, 0);
        if (attacking) {
            randomMove = true;
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
            movement = playerPos - rb2d.position;
            movement.Normalize();
        } else {
            switch (moveDirection) {
                case 0:
                    movement.y = 1f;
                    break;
                case 1:
                    movement.x = 1f;
                    break;
                case 2:
                    movement.y = -1f;
                    break;
                case 3:
                    movement.x = -1f;
                    break;
            }
        }
        rb2d.MovePosition(rb2d.position + movement * speed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("GoUp")) {
            moveDirection = 0;
            randomMove = false;
        } else if (col.gameObject.CompareTag("GoRight")) {
            moveDirection = 1;
            randomMove = false;
        } else if (col.gameObject.CompareTag("GoDown")) {
            moveDirection = 2;
            randomMove = false;
        } else if (col.gameObject.CompareTag("GoLeft")) {
            moveDirection = 3;
            randomMove = false;
        } else if (col.gameObject.CompareTag("Punch") && invTime <= 0) {
            health -= 2;
            invTime = totalInvTime;
            if (health <= 0) {
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Fire") && invTime <= 0) {
            health -= 4;
            invTime = totalInvTime;
        }
        if (health <= 0) {
            Destroy(this.gameObject);
        }
    }

    IEnumerator CheckVision() {
        yield return new WaitForSeconds(searchDelay);
        Debug.DrawLine(rb2d.position, player.transform.position, Color.red, searchDelay);
        int hits = Physics2D.LinecastNonAlloc(rb2d.position, player.transform.position, result);
        attacking = hits <= 3;
        searching = false;
    }
}
