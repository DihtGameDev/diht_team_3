using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SwitcherLogic : MonoBehaviour
{
    public bool isAlloedToPress = false;

    [SerializeField]
    float start_intensity;
    [SerializeField]
    Color startColor;
    [SerializeField]
    Color triggerColor;
    [SerializeField]
    Light2D lighter;
    [SerializeField]
    LightController lightController;

    [SerializeField]
    SpriteRenderer renderer;

    GameObject marshall;
    MarshallController marshallController;

    [SerializeField]
    bool isOnClickArea = false;

    public bool isOnArea = false;

    [SerializeField]
    private AudioController audioController;

    // Start is called beforeihe first frame update
    void Start()
    {

        marshall = GameObject.FindGameObjectWithTag("Marshall").gameObject;
        marshallController = marshall.GetComponent<MarshallController>();

        lighter = transform.parent.Find("Light").GetComponent<Light2D>();
        lightController = transform.parent.Find("Light").GetComponent<LightController>();

        renderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        startColor = renderer.color;
        triggerColor = Color.white;
        start_intensity = lighter.intensity;

        audioController = GameObject.Find("AudioManager").gameObject.GetComponent<AudioController>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(Global.action) && isOnClickArea && isAlloedToPress
            && !marshallController.isRestricted && Time.timeScale != 0f) {
            lightController.isShine = !lightController.isShine;
            //StopAllCoroutines();
            StartCoroutine(audioController.Play("LightSwitch"));
            StartCoroutine(buttonChangeColor());
            lighter.intensity = lightController.isShine ? start_intensity : 0f;
            lighter.gameObject.GetComponent<TriggerLogic>().isTrigger = true;

            if (isOnArea) {
                if (lightController.isShine)
                {
                    marshallController.shinersCounter++;
                }
                else {
                    marshallController.shinersCounter--;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Marshall"))
        {
            renderer.color = triggerColor;
            if (lightController.isShine) {
                StartCoroutine(lightSignal());
            }
            isOnClickArea = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Marshall"))
        {
            renderer.color = startColor;
            isOnClickArea = false;
        }
    }

     public IEnumerator lightSignal() {
        float start_intensity = lighter.intensity;
        lighter.GetComponent<LightController>().isSignalizeToPlayer = true;
        for (int i = 0; i < 2; i++)
        {
            lighter.intensity = 0f;
            yield return new WaitForSeconds(0.05f);
            lighter.intensity = start_intensity;
            yield return new WaitForSeconds(0.05f);
        }
        lighter.GetComponent<LightController>().isSignalizeToPlayer = false;

    }

    IEnumerator buttonChangeColor()
    {
        renderer.color = startColor;
        yield return new WaitForSeconds(0.2f);
        renderer.color = triggerColor;

    }


}
