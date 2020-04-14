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


    public bool isShine = true;

    void Start()
    {
        marshall = GameObject.FindGameObjectWithTag("Marshall").gameObject;
        marshallController = marshall.GetComponent<MarshallController>();

        switcher = transform.parent.Find("Switcher").GetComponent<SwitcherLogic>();
        visibleArea = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
