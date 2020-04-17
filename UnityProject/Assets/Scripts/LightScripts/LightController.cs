using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class LightController : MonoBehaviour
{
    CircleCollider2D visibleArea;

    GameObject marshall;
    MarshallController marshallController;
    SwitcherLogic switcher;

    private float start_intencity;
    public bool isShine = true;
    public bool isSignalizeToPlayer = false;

    public float force_of_fading = 0.25f;
    public float frequency_of_fading = 1.5f;


    void Start()
    {
        start_intencity = GetComponent<Light2D>().intensity;
        marshall = GameObject.FindGameObjectWithTag("Marshall").gameObject;
        marshallController = marshall.GetComponent<MarshallController>();

        switcher = transform.parent.Find("Switcher").GetComponent<SwitcherLogic>();
        visibleArea = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isShine && !isSignalizeToPlayer)
        {
            this.GetComponent<Light2D>().intensity =
                start_intencity * (1f - force_of_fading) + Mathf.PingPong(Time.time * frequency_of_fading, force_of_fading);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Marshall"))
        {
            if (switcher != null) {
                switcher.isOnArea = true;
            }
            
            if (isShine) {
                marshallController.shinersCounter++;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Marshall"))
        {
            if (switcher != null)
            {
                switcher.isOnArea = false;
            }
            if (isShine)
            {
                marshallController.shinersCounter--;
            }
        }
    }

}
