using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FacilityTriggerLogic : MonoBehaviour
{
    private MarshallController marshallController;

    public GameObject facility;
    public GameObject facility_next;

    public Light2D facilityLight;

    private void Awake()
    {
        marshallController = GameObject.FindGameObjectWithTag("Marshall").gameObject.GetComponent<MarshallController>();
    }
    void Update()
    {
        facilityLight = facility.GetComponent<FacilityLogic>().facilityLight;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Marshall")) {
          
            StartCoroutine(WaitAndFadeFacility(3f));
        }
    }

    IEnumerator WaitAndFadeFacility(float time) {


        facility.transform.Find("Enemies").gameObject.SetActive(false);
        marshallController.number_of_rushers = 0; 

        facility_next.SetActive(true);
        float start_intencity = facilityLight.intensity;
        for (int i = 0; i < 30; i++)
        {
            facilityLight.intensity -= start_intencity / 30f;
            yield return new WaitForSecondsRealtime(time / 30f);
        }
        Destroy(facility.gameObject);


    }

}

