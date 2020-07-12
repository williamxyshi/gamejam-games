using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public float duration;

    void Update()
    {
        duration -= Time.fixedDeltaTime;
        if (duration <= 0) {
            Destroy(this.gameObject);
        }
    }
}
