using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class LightController : MonoBehaviour
{

    float intesivity;

    List<GameObject> children;
    // Start is called before the first frame update
    void Start()
    {
        //intesivity = 1.2f;

        children = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
           // children[i].GetComponent<Light2D>().intensity = intesivity;
        }





    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
