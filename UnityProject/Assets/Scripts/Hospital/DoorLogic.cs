using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{

    private AudioController audioController;
    Vector2 target_position;
    Vector2 start_position;

    // Start is called before the first frame update
    void Start()
    {
        audioController = GameObject.Find("AudioManager").gameObject.GetComponent<AudioController>();
        start_position = transform.GetChild(0).position;
        target_position = start_position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).position = Vector2.MoveTowards(transform.GetChild(0).position, target_position, 1.1f * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Marshall") )
        {

            StartCoroutine(audioController.Play("SlideDoor"));
            target_position = new Vector2(start_position.x, start_position.y + 1f);


        }
       
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Marshall"))
        {
           
            StartCoroutine(audioController.Play("SlideDoor"));
            target_position = start_position;
        }
    }
}
