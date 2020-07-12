using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHitBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider){
        
        

        if(collider.gameObject.tag == "Cat" || collider.gameObject.tag == "Dog" || collider.gameObject.tag == "Mouse"){
            gameObject.transform.parent.gameObject.GetComponent<DoorController>().isColliding = true;
            Debug.Log("door");
        }
    }

    void OnTriggerExit2D(Collider2D collider){
         if(collider.gameObject.tag == "Cat"|| collider.gameObject.tag == "Dog" || collider.gameObject.tag == "Mouse"){
            gameObject.transform.parent.gameObject.GetComponent<DoorController>().isColliding = false;
            Debug.Log("leave door");
        } }
}
