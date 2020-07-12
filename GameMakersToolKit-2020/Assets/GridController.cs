using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridController : MonoBehaviour
{

    private GameObject laser;

    void Start(){
        this.laser = GameObject.FindWithTag("Laser");
    }
    void Update()
    {
        // Bounds bounds = gameObject.GetComponent<BoxCollider2D>().bounds;
       
        // Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         
        // bool inX = Math.Abs(bounds.center[0] - mousePos[0]) <= 0.5;
        // bool inY = Math.Abs(bounds.center[1] - mousePos[1]) <= 0.5;
        // Debug.Log(inX);
        // Debug.Log(inY);
         
        // if(inX && inY){
        //     laser.GetComponent<LaserController>().overGrid = true;
        // }
    

                
    
    
    }

    void OnMouseDown(){
        laser.GetComponent<LaserController>().overGrid = true;
    }

     

}

    
