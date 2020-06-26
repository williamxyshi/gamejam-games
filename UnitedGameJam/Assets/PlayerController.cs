using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D> ();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 movement = new Vector2 (0, 0);
        if (Input.GetButton("Up")) {
            movement.y += 1;
        }
        if (Input.GetButton("Down")) {
            movement.y -= 1;
        }
        if (Input.GetButton("Right")) {
            movement.x += 1;
        }
        if (Input.GetButton("Left")) {
            movement.x -= 1;
        }

        rb2d.MovePosition(rb2d.position + movement * speed * Time.fixedDeltaTime);
    }
}
