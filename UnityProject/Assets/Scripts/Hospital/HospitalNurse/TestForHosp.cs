using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForHosp : MonoBehaviour
{
    public Vector2 position;
    HospitalNurseController nurseController;
    // Start is called before the first frame update
    void Start()
    {
        nurseController = this.GetComponent<HospitalNurseController>();
        StartCoroutine(defineDefault(0.1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator defineDefault(float time) {
        yield return new WaitForSecondsRealtime(time);
        nurseController.default_position = position;
    }
}
