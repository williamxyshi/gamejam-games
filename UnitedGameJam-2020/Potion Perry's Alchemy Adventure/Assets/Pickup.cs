using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
   private Inventory inventory;

   private void Start()
   {
       
       inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
   }

   private void OnTriggerEnter2D(Collider2D other){
          Debug.Log("b");
       if(other.CompareTag("Player")){
           inventory.BoneShards++;
           Destroy(gameObject);
        
       }
   }
}
