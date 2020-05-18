using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.Rendering.Universal;

public class BobDoor : MonoBehaviour
{
    private AudioController audioController;
    Vector2 target_position;
    Vector2 start_position;

    public GameObject lightFromDoor;
    float intensivity;

    // Start is called before the first frame update
    void Start()
    {
        intensivity = 0f;
        audioController = GameObject.Find("AudioManager").gameObject.GetComponent<AudioController>();
        start_position = transform.GetChild(0).position;
        target_position = start_position;

        lightFromDoor.GetComponent<Light2D>().intensity = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).position = Vector2.MoveTowards(transform.GetChild(0).position, target_position, 1.1f * Time.deltaTime);
        lightFromDoor.GetComponent<Light2D>().intensity =
           Mathf.Lerp(lightFromDoor.GetComponent<Light2D>().intensity, intensivity, 2f * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Marshall"))
        {

            
            StartCoroutine(open(1f));
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Marshall"))
        {
         
           StartCoroutine(audioController.Play("SlideDoor"));
           target_position = start_position;

           intensivity = 0f;
        }
    }
    IEnumerator open(float offset) {
        yield return new WaitForSeconds(offset);

        StartCoroutine(audioController.Play("SlideDoor"));
        target_position = new Vector2(start_position.x + 0.4f, start_position.y);

        intensivity = 2f;
    }
}
