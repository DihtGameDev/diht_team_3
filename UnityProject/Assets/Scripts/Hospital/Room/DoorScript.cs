using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DoorScript : MonoBehaviour
{

    public GameObject lightFromDoor;
    float intensivity;
    Vector2 target_position;
    GameObject sprite;

    Vector2 start_position;// Start is called before the first frame update
    void Start()
    {
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
        if (other.CompareTag("Marshall"))
        {           
            target_position = new Vector2(start_position.x - 0.3f, start_position.y);
            intensivity = 1.65f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Marshall"))
        {
            target_position = start_position;
            intensivity = 0f;
        }
    }
}
