using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Untagged")){
            player.GetComponent<Rigidbody2D>().position = this.transform.position;
            Destroy(this.gameObject);
        }
    }
}
