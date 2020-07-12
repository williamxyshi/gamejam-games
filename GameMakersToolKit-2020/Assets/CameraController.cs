using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed;
    public float highBound;
    public float lowBound;
    public float rightBound;
    public float leftBound;

    void FixedUpdate()
    {
        float moveX = 0;
        float moveY = 0;

        if (Input.GetButton("Up") && this.transform.position.y < highBound) {
            moveY += cameraSpeed;
        }
        if (Input.GetButton("Down") && this.transform.position.y > lowBound) {
            moveY += -cameraSpeed;
        }
        if (Input.GetButton("Right") && this.transform.position.x < rightBound) {
            moveX += cameraSpeed;
        }
        if (Input.GetButton("Left") && this.transform.position.x > leftBound) {
            moveX += -cameraSpeed;
        }
        
        this.transform.position += new Vector3(moveX, moveY, 0);
    }
}
