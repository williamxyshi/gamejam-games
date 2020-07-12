using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    public GameObject Door;
    public Sprite doorOpen;
    public Sprite doorClosed;


    public AudioSource open;
    public AudioSource close;

    public bool isColliding = false;

    bool isClosed = true;

    public void toggleDoor(){
        Debug.Log(
            "bruh"
        );
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        /*
        *   flips open or closes the door
        */
        if(Input.GetMouseButtonDown(0)){
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            BoxCollider2D coll = Door.GetComponent<BoxCollider2D>();
            if(coll.OverlapPoint(wp) ){
                if(!this.isClosed && !this.isColliding){
                    this.isClosed = !this.isClosed;
                } else if(this.isClosed){
                    this.isClosed = !this.isClosed;
                }

                

                if(this.isClosed){
                    this.close.Play();
                } else {
                    this.open.Play();
                }
            }

        }


        if(this.isClosed){
            gameObject.GetComponent<SpriteRenderer>().sprite = this.doorClosed;
             gameObject.layer = LayerMask.NameToLayer("Ground");

            

        } else {
            gameObject.GetComponent<SpriteRenderer>().sprite = this.doorOpen;
             gameObject.layer = LayerMask.NameToLayer("Default");
        
        }






        
    }
}
