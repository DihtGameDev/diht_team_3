using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class ClothLogic : MonoBehaviour
{
    // Start is called before the first frame update

    public Sprite mainTex;
    public Sprite selectTex;

    public bool allowedToTake = false;

    private AudioController audioController;
    void Start()
    {
        audioController = GameObject.Find("AudioManager").gameObject.GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (allowedToTake && Input.GetKeyDown(Global.action)) {
            StartCoroutine(audioController.Play("SelectButtonGameMenu"));
            Destroy(this.gameObject, 0.2f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Marshall"))
        {
            allowedToTake = true;
            transform.GetChild(0).transform.GetComponent<SpriteRenderer>().sprite = selectTex;

        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Marshall"))
        {
            allowedToTake = false;
            transform.GetChild(0).transform.GetComponent<SpriteRenderer>().sprite = mainTex;
        }
    }
}
