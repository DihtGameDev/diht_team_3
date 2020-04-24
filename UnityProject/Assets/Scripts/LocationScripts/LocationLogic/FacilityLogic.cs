using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FacilityLogic : MonoBehaviour
{
    public Light2D facilityLight;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndStartFacility(1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator WaitAndStartFacility(float time)
    {
        facilityLight.intensity = 0.0f;
        for (int i = 0; i < 30; i++)
        {
            facilityLight.intensity += 0.7f / 30f;
            yield return new WaitForSecondsRealtime(time / 30f);
        }
      
    }

}
