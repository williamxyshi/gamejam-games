using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaserController : MonoBehaviour
{
    public bool laserOn;
    public bool overGrid;

    void FixedUpdate()
    {
         GameObject[] grids = GameObject.FindGameObjectsWithTag("Grid");
         this.overGrid = false;
        foreach(GameObject grid in grids){
            Bounds bounds = grid.GetComponent<BoxCollider2D>().bounds;
       
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            bool inX = Math.Abs(bounds.center[0] - mousePos[0]) <= 0.5;
            bool inY = Math.Abs(bounds.center[1] - mousePos[1]) <= 0.5;

            if(inX && inY) this.overGrid = true;
     
        }



  

        if(!this.overGrid){
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButton(0)) {
                transform.position = new Vector3(mousePos.x, mousePos.y, 0);
                laserOn = true;
            } else {
                laserOn = false;
                transform.position = new Vector3(-100, -100, 0);
            }
        } else {
            laserOn = false;
            transform.position = new Vector3(-100, -100, 0);
        }
  
    }
}
