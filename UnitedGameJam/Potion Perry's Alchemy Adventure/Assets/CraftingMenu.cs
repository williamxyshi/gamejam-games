using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMenu : MonoBehaviour
{   

    public Sprite soulFragmentSprite;
    public Sprite boneShardSprite;
    public Sprite brownCapSprite;
public Sprite emptySprite;

    private GameObject craftingLeft;
    private GameObject craftingRight;

    public bool leftSet;
    public bool rightSet;

    public string rightItem;
    public string leftItem;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        this.craftingLeft = GameObject.FindWithTag("LeftCraft");
        this.craftingRight = GameObject.FindWithTag("RightCraft");

        this.player = GameObject.FindWithTag("Player");

        this.leftSet = false;
        this.rightSet = false;
        
        this.leftItem = "";
        this.rightItem = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void removeLeft(){
        this.craftingLeft.GetComponent<Image>().sprite = this.emptySprite;
        this.leftSet = false;

        if(this.leftItem == "soul"){

            this.player.GetComponent<PlayerController>().inventory.SoulFragments++;
        } else if(this.leftItem == "bone"){
             this.player.GetComponent<PlayerController>().inventory.BoneShards++;

        }else if(this.leftItem == "brown"){
             this.player.GetComponent<PlayerController>().inventory.BrownCaps++;

        }
        this.leftItem = "";
    }
    public void removeRight(){
        this.craftingRight.GetComponent<Image>().sprite = this.emptySprite;
        this.rightSet = false;

        
        if(this.rightItem == "soul"){

            this.player.GetComponent<PlayerController>().inventory.SoulFragments++;
        } else if(this.rightItem == "bone"){
             this.player.GetComponent<PlayerController>().inventory.BoneShards++;

        }else if(this.rightItem == "brown"){
             this.player.GetComponent<PlayerController>().inventory.BrownCaps++;

        }
        this.rightItem = "";
    }

    public void craftLeftHand(){
         Debug.Log("bruh");
        if(this.leftItem != "" && this.rightItem != "" && (this.leftItem != this.rightItem)){
            string potion = "";
            if( (this.leftItem == "soul" && this.rightItem == "bone" ) || (this.leftItem == "bone" && this.rightItem == "soul" ) ){
                potion = "teleport";
            } else if( (this.leftItem == "brown" && this.rightItem == "bone" ) || (this.leftItem == "bone" && this.rightItem == "brown" ) ){
                potion = "fire";
            } else if( (this.leftItem == "brown" && this.rightItem == "soul" ) || (this.leftItem == "soul" && this.rightItem == "brown" )){
                potion = "heal";
            }



            this.leftItem = "";
            this.rightItem = "";
            this.rightSet = false; this.leftSet = false;

             Debug.Log(potion);

              this.craftingLeft.GetComponent<Image>().sprite = this.emptySprite;
               this.craftingRight.GetComponent<Image>().sprite = this.emptySprite;
                this.player.GetComponent<PlayerController>().potionLeft = potion;

           




            
        }




    }

    public void craftRightHand(){
        if(this.leftItem != "" && this.rightItem != "" && (this.leftItem != this.rightItem)){
            string potion = "";
            if( (this.leftItem == "soul" && this.rightItem == "bone" ) || (this.leftItem == "bone" && this.rightItem == "soul" ) ){
                potion = "teleport";
            } else if( (this.leftItem == "brown" && this.rightItem == "bone" ) || (this.leftItem == "bone" && this.rightItem == "brown" ) ){
                potion = "fire";
            } else if( (this.leftItem == "brown" && this.rightItem == "soul" ) || (this.leftItem == "soul" && this.rightItem == "brown" )){
                potion = "heal";
            }

              this.craftingLeft.GetComponent<Image>().sprite = this.emptySprite;
               this.craftingRight.GetComponent<Image>().sprite = this.emptySprite;
            this.leftItem = "";
            this.rightItem = "";
            this.rightSet = false; this.leftSet = false;

            Debug.Log(potion);

            this.player.GetComponent<PlayerController>().potionRight = potion;




            
        }

    }

    
    public void setCraft(string recipe){
        Debug.Log(recipe);

        if(recipe == "soul" && this.player.GetComponent<PlayerController>().inventory.SoulFragments >= 1){

            if(!leftSet){
                this.craftingLeft.GetComponent<Image>().sprite = this.soulFragmentSprite;
                this.leftSet = true;

                this.player.GetComponent<PlayerController>().inventory.SoulFragments--;

                this.leftItem = "soul";
            } else if(!rightSet){
                this.craftingRight.GetComponent<Image>().sprite = this.soulFragmentSprite;
                this.rightSet = true;

                this.player.GetComponent<PlayerController>().inventory.SoulFragments--;

                this.rightItem = "soul";
            } 
                
        } else  if(recipe == "bone" && this.player.GetComponent<PlayerController>().inventory.BoneShards >= 1){

            if(!leftSet){
                this.craftingLeft.GetComponent<Image>().sprite = this.boneShardSprite;
                this.leftSet = true;

                this.player.GetComponent<PlayerController>().inventory.BoneShards--;


                this.leftItem = "bone";
            } else if(!rightSet){
                this.craftingRight.GetComponent<Image>().sprite = this.boneShardSprite;
                this.rightSet = true;

                 this.player.GetComponent<PlayerController>().inventory.BoneShards--;

                this.rightItem = "bone";
            } 
                
        }  else  if(recipe == "brown"&& this.player.GetComponent<PlayerController>().inventory.BrownCaps >= 1){

            if(!leftSet){
                this.craftingLeft.GetComponent<Image>().sprite = this.brownCapSprite;
                this.leftSet = true;

                 this.player.GetComponent<PlayerController>().inventory.BrownCaps--;

                this.leftItem = "brown";
            } else if(!rightSet){
                this.craftingRight.GetComponent<Image>().sprite = this.brownCapSprite;
                this.rightSet = true;

                this.player.GetComponent<PlayerController>().inventory.BrownCaps--;

                this.rightItem = "brown";
            } 
                
        } 


    }
}
