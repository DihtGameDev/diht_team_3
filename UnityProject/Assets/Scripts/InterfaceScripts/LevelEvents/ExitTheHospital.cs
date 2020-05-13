using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.Rendering.Universal;

public class ExitTheHospital : MonoBehaviour
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

            StartCoroutine(audioController.Play("SlideDoor"));
            StartCoroutine(audioController.Play("BackGround", 2f));
            StartCoroutine(audioController.ChangeVolume("HospitalTrack", audioController.GetClipRelevantVolume("HospitalTrack") * 0.2f));
            target_position = new Vector2(start_position.x, start_position.y + 1f);

            intensivity = 1f;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Marshall"))
        {
            StartCoroutine(audioController.Stop("BackGround", 2f));
            StartCoroutine(audioController.Play("SlideDoor"));
            StartCoroutine(audioController.ChangeVolume("HospitalTrack", audioController.GetClipRelevantVolume("HospitalTrack") * 1f));
            target_position = start_position;

            intensivity = 0f;
        }
    }
}
