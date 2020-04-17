using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DoorScript : MonoBehaviour
{
    private bool come = false;
    public GameObject lightFromDoor;
    public Light2D globalLight;
    private float start_intensity_global;

    float intensivity;
    Vector2 target_position;
    GameObject sprite;

    Vector2 start_position;// Start is called before the first frame update
    void Start()
    {
        start_intensity_global = globalLight.intensity;
        globalLight.intensity = 0f;

        lightFromDoor.GetComponent<Light2D>().intensity = 0f;
        intensivity = 0f;
        sprite = transform.GetChild(0).gameObject;
        start_position = transform.GetChild(0).position;
        target_position = start_position;
    }

    // Update is called once per frame
    void Update()
    {
        lightFromDoor.GetComponent<Light2D>().intensity =
            Mathf.Lerp(lightFromDoor.GetComponent<Light2D>().intensity, intensivity, 2f * Time.deltaTime);
        transform.GetChild(0).position = Vector2.MoveTowards(transform.GetChild(0).position, target_position, 0.5f * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Marshall") || other.CompareTag("Doctor"))
        {           
            target_position = new Vector2(start_position.x - 0.3f, start_position.y);
           
            intensivity = 1.65f;
        
        }
        if (other.CompareTag("Doctor")) {
            StartCoroutine(lightFade(start_intensity_global));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Marshall") || other.CompareTag("Doctor"))
        {
            target_position = start_position;            
             intensivity = 0f;          
        }
    }

    IEnumerator lightFade(float target) {
        while (Mathf.Abs(globalLight.intensity - target) > 0.03f) {
            globalLight.intensity = Mathf.Lerp(globalLight.intensity, target, 0.6f * Time.deltaTime);
            yield return null;
        }
    }
}
