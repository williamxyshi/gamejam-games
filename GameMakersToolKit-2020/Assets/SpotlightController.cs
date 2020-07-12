using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightController : MonoBehaviour
{
    public float moveSpeed;
    public GameObject point1;
    public GameObject point2;

    private bool goToPoint1;

    void Start()
    {
        goToPoint1 = true;
    }

    void FixedUpdate()
    {
        if (goToPoint1) {
            transform.position = Vector2.MoveTowards(transform.position, point1.transform.position, moveSpeed);
        } else {
            transform.position = Vector2.MoveTowards(transform.position, point2.transform.position, moveSpeed);
        }

        if (transform.position == point1.transform.position) {
            goToPoint1 = false;
        } else if (transform.position == point2.transform.position) {
            goToPoint1 = true;
        }
    }
}
