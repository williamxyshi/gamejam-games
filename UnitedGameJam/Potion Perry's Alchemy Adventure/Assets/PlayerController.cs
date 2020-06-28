using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{


    public Animator animator;

    private int MAX_ITEMS = 9;

    public float speed;
    public float totalInvTime;
    public float punchDistance;

    private float invTime;

    public GameObject leftHand;
    public GameObject rightHand;

    public string potionLeft;
    public string potionRight;

    public HealthBar healthBar;

    public Sprite zero;
    public Sprite one;
    public Sprite two;
    public Sprite three;
    public Sprite four;
    public Sprite five;
    public Sprite six;
    public Sprite seven;
    public Sprite eight;
    public Sprite nine;

    public Sprite teleportationPotion;
    public Sprite firePotion;
    public Sprite healthPotion;
    public Sprite nothing;

    public int maxHealth = 100;

    public int health;

    public Rigidbody2D projectile;
    public float throwSpeed;

    public Rigidbody2D tele;
    public float teleSpeed;

    public GameObject punch;

    private Rigidbody2D rb2d;

    public Inventory inventory;

    private GameObject soulCounter;
    private GameObject boneCounter;
    private GameObject brownCounter;

    private List<Sprite> numbers = new List<Sprite>();

    private GameObject scroll;

    private bool overBrewButton;

    

    // Start is called before the first frame update
    void Start()
    {
        this.health = this.maxHealth;
        healthBar.SetMaxHealth(this.maxHealth);



        rb2d = GetComponent<Rigidbody2D> ();
        invTime = 0;

        this.inventory = new Inventory();

        Debug.Log(inventory.BoneShards);

        numbers.Add(zero);
        numbers.Add(one);
        numbers.Add(two);
        numbers.Add(three);
        numbers.Add(four);
        numbers.Add(five);
        numbers.Add(six);
        numbers.Add(seven);
        numbers.Add(eight);
        numbers.Add(nine);

        this.soulCounter = GameObject.FindWithTag("SoulCounter");
        this.boneCounter = GameObject.FindWithTag("BoneCounter");
        this.brownCounter = GameObject.FindWithTag("BrownCounter");

        this.rightHand = GameObject.FindWithTag("RightHand");
        this.leftHand = GameObject.FindWithTag("LeftHand");

        this.boneCounter.GetComponent<Image>().sprite = numbers[inventory.BoneShards];

        this.soulCounter.GetComponent<Image>().sprite = numbers[inventory.SoulFragments];

        this.brownCounter.GetComponent<Image>().sprite = numbers[inventory.BrownCaps];
  
        scroll = GameObject.FindWithTag("Scroll");
        
        overBrewButton = false;
    }

    void FixedUpdate()
    {
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));

        Vector2 movement = new Vector2 (0, 0);
        if (Input.GetButton("Up")) {
            movement.y += 1;
        }
        if (Input.GetButton("Down")) {
            movement.y -= 1;
        }
        if (Input.GetButton("Right")) {
            movement.x += 1;
        }
        if (Input.GetButton("Left")) {
            movement.x -= 1;
        }

        rb2d.MovePosition(rb2d.position + movement * speed * Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update(){
        if (invTime > 0) {
            invTime -= Time.fixedDeltaTime;
        }
        // float moveVertical = Input.GetAxis("Vertical");
        // float moveHorizontal = Input.GetAxis("Horizontal");

        // Vector3 newPosition = new Vector3(moveHorizontal, 0.0f, moveVertical);
        // transform.LookAt(newPosition + transform.position);
        // transform.Translate(newPosition * speed * Time.deltaTime, Space.World);
        
        this.boneCounter.GetComponent<Image>().sprite = numbers[inventory.BoneShards];

        this.soulCounter.GetComponent<Image>().sprite = numbers[inventory.SoulFragments];

        this.brownCounter.GetComponent<Image>().sprite = numbers[inventory.BrownCaps];

        if(this.potionLeft == "teleport"){
            this.leftHand.GetComponent<Image>().sprite = teleportationPotion;
        } else if(this.potionLeft == "heal"){
            this.leftHand.GetComponent<Image>().sprite = healthPotion;
        } else if(this.potionLeft == "fire"){
            this.leftHand.GetComponent<Image>().sprite = firePotion;
        } else if(this.potionLeft == ""){
            this.leftHand.GetComponent<Image>().sprite = nothing;
        }

        if(this.potionRight == "teleport"){
            this.rightHand.GetComponent<Image>().sprite = teleportationPotion;
        } else if(this.potionRight == "heal"){
            this.rightHand.GetComponent<Image>().sprite = healthPotion;
        } else if(this.potionRight == "fire"){
            this.rightHand.GetComponent<Image>().sprite = firePotion;
        } else if(this.potionRight == ""){
            this.rightHand.GetComponent<Image>().sprite = nothing;
        }

        if (!scroll.activeSelf && !overBrewButton) {
            if (Input.GetMouseButtonDown(0)) {
                if(this.potionLeft == "heal") {
                    health += 30;
                    if (health > 100) {
                        health = 100;
                    }
                } else if (this.potionLeft == "fire") {
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mousePos = new Vector2 (location.x, location.y);
                    mousePos -= rb2d.position;
                    mousePos.Normalize();
                    Rigidbody2D clone;
                    clone = Instantiate(projectile, this.transform.position, this.transform.rotation);
                    clone.velocity = mousePos * throwSpeed;
                    clone.angularVelocity = 270f;
                } else if (this.potionLeft == "teleport") {
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mousePos = new Vector2 (location.x, location.y);
                    mousePos -= rb2d.position;
                    mousePos.Normalize();
                    Rigidbody2D clone;
                    clone = Instantiate(tele, this.transform.position, this.transform.rotation);
                    clone.velocity = mousePos * teleSpeed;
                } else {
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mousePos = new Vector2 (location.x, location.y);
                    mousePos -= rb2d.position;
                    mousePos.Normalize();
                    Vector3 punchDirection = new Vector3 (mousePos.x, mousePos.y, 0) * punchDistance;
                    GameObject clone;
                    clone = Instantiate(punch, this.transform.position + punchDirection, this.transform.rotation);
                    clone.SetActive(true);
                }
                this.potionLeft = "";
            }
            if (Input.GetMouseButtonDown(1)) {
                if(this.potionRight == "heal") {
                    health += 30;
                    if (health > 100) {
                        health = 100;
                    }
                } else if (this.potionRight == "fire") {
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mousePos = new Vector2 (location.x, location.y);
                    mousePos -= rb2d.position;
                    mousePos.Normalize();
                    Rigidbody2D clone;
                    clone = Instantiate(projectile, this.transform.position, this.transform.rotation);
                    clone.velocity = mousePos * throwSpeed;
                    clone.angularVelocity = 270f;
                } else if (this.potionRight == "teleport") {
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mousePos = new Vector2 (location.x, location.y);
                    mousePos -= rb2d.position;
                    mousePos.Normalize();
                    Rigidbody2D clone;
                    clone = Instantiate(tele, this.transform.position, this.transform.rotation);
                    clone.velocity = mousePos * teleSpeed;
                } else {
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mousePos = new Vector2 (location.x, location.y);
                    mousePos -= rb2d.position;
                    mousePos.Normalize();
                    Vector3 punchDirection = new Vector3 (mousePos.x, mousePos.y, 0) * punchDistance;
                    GameObject clone;
                    clone = Instantiate(punch, this.transform.position + punchDirection, this.transform.rotation);
                    clone.SetActive(true);
                }
                this.potionRight = "";
            }
        }

        if(this.health < 0){
            SceneManager.LoadScene(2);
        }

        healthBar.SetHealth(this.health);
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("BoneShard")){
            if(inventory.BoneShards < MAX_ITEMS){
            inventory.BoneShards++;
            }

            Destroy(other.gameObject);
            Debug.Log(inventory.BoneShards);

            this.boneCounter.GetComponent<Image>().sprite = numbers[inventory.BoneShards];
        }
        else if(other.CompareTag("SoulFragment")){
            if(inventory.SoulFragments < MAX_ITEMS){
            inventory.SoulFragments++;
            }

            Destroy(other.gameObject);
            Debug.Log(inventory.SoulFragments);

            this.soulCounter.GetComponent<Image>().sprite = numbers[inventory.SoulFragments];
        }
        else if(other.CompareTag("BrownCap")){
            if(inventory.BrownCaps < MAX_ITEMS){
            inventory.BrownCaps++;
            }

            Destroy(other.gameObject);
            Debug.Log(inventory.BrownCaps);

            this.brownCounter.GetComponent<Image>().sprite = numbers[inventory.BrownCaps];
        } else if ((other.CompareTag("Enemy") | other.CompareTag("Fire")) && invTime <= 0) {
            health -= 20;
            invTime = totalInvTime;
        }
        else if(other.CompareTag("WIN")){
            SceneManager.LoadScene(3);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if((other.CompareTag("Enemy") | other.CompareTag("Fire")) && invTime <= 0) {
            health -= 20;
            invTime = totalInvTime;
        }
    }
    
    public void overBrew() {
        overBrewButton = true;
    }

    public void leftBrew() {
        overBrewButton = false;
    }
}
