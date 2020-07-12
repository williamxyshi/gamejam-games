using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePotionController : MonoBehaviour
{
    public GameObject fire;

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Enemy") | other.CompareTag("Untagged")){
            GameObject clone;
            clone = Instantiate(fire, this.transform.position, new Quaternion(0,0,0,0));
            clone.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
