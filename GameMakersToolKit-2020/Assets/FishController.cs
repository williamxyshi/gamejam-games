using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FishController : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    public bool eaten = false;

    public Sprite still;
    public Sprite struggle;
    public Sprite bones;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("change_sprite", 0, 1.0f);
    }

    void change_sprite() {
        if (eaten) return;

        if (spriteRenderer.sprite == still) {
            spriteRenderer.sprite = struggle;
        } else {
            spriteRenderer.sprite = still;
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {

        if (collider.gameObject.tag == "Cat") {
            eaten = true;
            spriteRenderer.sprite = bones;

            StartCoroutine("LoadLevel");






        }

    }

    IEnumerator LoadLevel(){

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);





    }

}
