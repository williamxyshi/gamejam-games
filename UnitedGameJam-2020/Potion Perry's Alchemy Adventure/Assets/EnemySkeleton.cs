using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : MonoBehaviour
{
     public Animator animator;

    public float searchDelay;
    public Rigidbody2D projectile;
    public float totalAttackTime;
    public float boneSpeed;
    public int health;
    public float totalInvTime;
    private GameObject player;
    private Rigidbody2D rb2d;
    private RaycastHit2D[] result;
    private float attackTime;
    private bool searching;
    private bool attacking;
    private float invTime;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D> ();
        player = GameObject.FindWithTag("Player");
        result = new RaycastHit2D[4];
        searching = false;
        attacking = false;
        attackTime = 0;
        invTime = 0;
    }

    // Update is called once per frame
    void Update()
    {

        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        if (invTime > 0) {
            invTime -= Time.fixedDeltaTime;
        }
        if (!searching) {
            searching = true;
            StartCoroutine("CheckVision");
        }

        if (attacking && attackTime <= 0) {
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
            playerPos -= rb2d.position;
            playerPos.Normalize();
            Rigidbody2D clone;
            clone = Instantiate(projectile, this.transform.position, this.transform.rotation);
            clone.velocity = playerPos * boneSpeed;
            clone.angularVelocity = 270f;
            attackTime = totalAttackTime;
        } else if (attackTime > 0) {
            attackTime -= Time.fixedDeltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Punch") && invTime <= 0) {
            health -= 2;
            invTime = totalInvTime;
        }
        if (health <= 0) {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Fire") && invTime <= 0) {
            Destroy(this.gameObject);
            health -= 2;
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
