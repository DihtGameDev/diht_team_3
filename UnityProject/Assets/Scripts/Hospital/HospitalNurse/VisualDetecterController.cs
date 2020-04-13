﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDetecterController : MonoBehaviour
{
    [HideInInspector]
    public bool isMashallVisible;

    [SerializeField]
    private int numerOfObstacles;

    [SerializeField]
    private int numberOfShelters;

    [SerializeField]
    private bool isMarshalOnSafeZoneAndHiden; // в некоторой безопасной зоне и за укрытием (в/но стоит)

    [SerializeField]
    private float distanceToMarshall;

    [HideInInspector]
    public float distanceOfViewing;
    public float calmDistanceOfViewing;
    public float angryDistanceOfViewing;

    [SerializeField]
    GameObject marshall;
    [SerializeField]
    GameObject itsOwner;
    [SerializeField]
    MarshallController marshallController;
    [SerializeField]
    HospitalNurseController ownerController;

    [SerializeField]
    private Dictionary<string, GameObject> shelters;

    private string name;
    [SerializeField]
    EdgeCollider2D view;

    [HideInInspector]
    public List<Vector2> newVerticies = new List<Vector2>();
    // Start is called before the first frame update
    void Start()
    {
        name = this.transform.parent.name;

        isMashallVisible = false;
        isMarshalOnSafeZoneAndHiden = false;
        numerOfObstacles = 0;
        numberOfShelters = 0;
        distanceToMarshall = 0f;

        calmDistanceOfViewing = 7f;
        angryDistanceOfViewing = 10f;
        distanceOfViewing = calmDistanceOfViewing;

        view = GetComponent<EdgeCollider2D>();
        marshall = GameObject.FindGameObjectWithTag("Marshall").gameObject;
        marshallController = marshall.GetComponent<MarshallController>();
        itsOwner = transform.parent.gameObject;
        ownerController = itsOwner.GetComponent<HospitalNurseController>();

        shelters = new Dictionary<string, GameObject>();
        newVerticies.Add(Vector2.zero);
        newVerticies.Add(marshall.transform.position - transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        newVerticies[0] = new Vector2(0f, ownerController.height);
        newVerticies[1] = new Vector2(marshall.transform.position.x - transform.position.x,
            marshall.transform.position.y - transform.position.y + marshallController.marshal_height);
        view.points = newVerticies.ToArray();

        distanceToMarshall = Vector2.Distance(view.points[0], view.points[1]);

        isMarshalOnSafeZoneAndHiden = false;
        foreach (var shelter in shelters) {
            if (shelter.Value.GetComponent<ShelterLogic>().information[name].isHided) {
                isMarshalOnSafeZoneAndHiden = true;
                break;
            }
        }

        //if()

        numberOfShelters = shelters.Count;

        if (distanceToMarshall > distanceOfViewing)
        {
            isMashallVisible = false;
        }
        else {
            if (numerOfObstacles > 0)
            {
                isMashallVisible = false;
            }
            else {
                if (isMarshalOnSafeZoneAndHiden && marshallController.isSitting)
                {
                    isMashallVisible = false;
                }
                else {
                    isMashallVisible = true;
                }
            }
        }     
    }

    void OnTriggerEnter2D(Collider2D other) {       
        if (other.CompareTag("Obstacle"))
        {
            numerOfObstacles++;
        }


        if (other.CompareTag("Shelter"))
        {
            shelters.Add(other.name, other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other) {

        if (other.CompareTag("Obstacle"))
        {
            numerOfObstacles--;
        }

        if (other.CompareTag("Shelter"))
        {
            shelters.Remove(other.name);
        }
    }
}