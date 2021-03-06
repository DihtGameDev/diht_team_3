﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLogic : MonoBehaviour
{
    public bool isProblemLight;
    public static float wallHigh = 1f;
    public bool isTrigger = false;

    public float distanceOfTriggering;


    public List<GameObject> checkersStates = new List<GameObject>();

    public GameObject checker;
    // Start is called before the first frame update
    void Start()
    {
        distanceOfTriggering = 8f;

        StartCoroutine(findEnemies(2f));

    }

    // Update is called once per frame
    void Update()
    {
        if (isTrigger) {
            isTrigger = false;
            foreach (var checker in checkersStates)
            {
                if (checker.GetComponent<Checker>().targetIsObserving) {
                    checker.GetComponent<Checker>().target.
                        GetComponent<HospitalNurseController>().lightTrigger.isTriggerOnTheLight = true;
                    checker.GetComponent<Checker>().target.
                        GetComponent<HospitalNurseController>().lightTrigger.light = transform.gameObject;
                }
            }
        }
    }

    IEnumerator findEnemies(float offset) {
        yield return new WaitForSecondsRealtime(offset);

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            GameObject newObj = Instantiate(checker,
            new Vector2(transform.position.x, transform.position.y - wallHigh), Quaternion.identity);
            newObj.AddComponent<Checker>();
            newObj.GetComponent<Checker>().target = enemy;
            newObj.GetComponent<Checker>().parent = this.gameObject;
            checkersStates.Add(newObj);

        }
    }

}
