using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FacilityTriggerLogic : MonoBehaviour
{
    public bool isActive;
    public bool isNeedToStart = false;
    public float start_intensity = 0.7f;

    public GameObject facility;
    public GameObject facility_next;
    public FacilityTriggerLogic trigger_next;

    public GameObject facility_previous;
    public FacilityTriggerLogic trigger_previous;
    public Light2D facilityLight;

    public bool isEnter;
    public bool isExit;

    void Awake()
    {
        if (this.gameObject.name == "EnterFacilityTrigger")
        {
            isEnter = true;
            
        }
        else if (this.gameObject.name == "ExitFacilityTrigger") {
            isExit = true;
        }
        StartCoroutine(start(0.3f));

    }

    // Update is called once per frame
    void Update()
    {
        if (isNeedToStart) {
            isNeedToStart = false;
            StartCoroutine(WaitAndStartFacility(2f));
        }

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Marshall")) {
            if (facility.transform.FindChild("Enemies").gameObject.activeSelf) { 
            StartCoroutine(WaitAndFadeFacility(2f));
            
            }
        }
    }

    IEnumerator WaitAndFadeFacility(float speed) {

        if (isEnter && facility_previous != null)
        {
            facility_previous.transform.FindChild("Enemies").gameObject.SetActive(true);
            trigger_previous.isNeedToStart = true;
        }
        else if (isExit && facility_next != null)
        {
            facility_next.transform.FindChild("Enemies").gameObject.SetActive(true);
            trigger_next.isNeedToStart = true;
        }
        

        while (facilityLight.intensity > 0.105f) {
            facilityLight.intensity = Mathf.Lerp(facilityLight.intensity, 0.1f, speed * Time.deltaTime);
            
            yield return null;
        }
        StartCoroutine(turnOfEnemies(15f));

    }

    public IEnumerator WaitAndStartFacility(float speed) {

        facilityLight.intensity = 0.1f;
        while (facilityLight.intensity < 0.7f - 0.01f)
        {
            facilityLight.intensity = Mathf.Lerp(facilityLight.intensity, 0.7f, speed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("Shine");
    }

    IEnumerator start(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        if (!isActive) {
            facility.transform.FindChild("Enemies").gameObject.SetActive(false);
        }
        

        Debug.Log("Shine");
    }

    IEnumerator turnOfEnemies(float time)// иначе остаются корутины
    {
        yield return new WaitForSecondsRealtime(time);
        facility.transform.FindChild("Enemies").gameObject.SetActive(false);


        Debug.Log("Shine");
    }



}

