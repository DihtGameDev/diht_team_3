using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterLogic : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<string, EnenmyInfo> information;

    [HideInInspector]
    public float safetyBias; // > 0.0f
    [HideInInspector]
    public float safetyConst; // < 1.0f

    public bool isObserved;

    private float upDateDistPeriod;
    private float upDateHidenessPeriod;

    [SerializeField]
    float distanceToMarshall;

    GameObject marshall;

    Collider2D collider;

    Rigidbody2D rb;

    [HideInInspector]
    public class EnenmyInfo {
        public GameObject go;
        public float dist;
        public bool isHided;

        public EnenmyInfo(GameObject ob, float d, bool isH) {
            go = ob;
            dist = d;
            isHided = isH;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        //Default Values
        marshall = GameObject.FindGameObjectWithTag("Marshall").gameObject;
        distanceToMarshall = Vector2.Distance(transform.position, marshall.transform.position);
        information = new Dictionary<string, EnenmyInfo>();
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        isObserved = false;

        //Changable Values
        upDateDistPeriod = 0.15f;
        upDateHidenessPeriod = 0.15f;


        safetyBias = 0.25f;
        safetyConst = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.zero;
        if (information.Count != 0)
        {
            isObserved = true;
        }
        else
        {
            isObserved = false;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Detecter"))
        {
            information.Add(other.transform.parent.name, new EnenmyInfo(other.transform.parent.gameObject,
                Vector2.Distance(transform.position, other.transform.parent.position), false));

            StartCoroutine(updateDistance(other.transform.parent.name, upDateDistPeriod));
            StartCoroutine(updateHideness(other.transform.parent.name, upDateHidenessPeriod));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Detecter"))
        {
            information.Remove(other.transform.parent.name);
        }
    }

    IEnumerator updateDistance(string enemy, float period)
    {
        while (true)
        {
            if (!information.ContainsKey(enemy))
            {
                break;
            }

            information[enemy].dist = Vector2.Distance(transform.position, information[enemy].go.transform.position);
            yield return new WaitForSecondsRealtime(period);
        }
    }


    IEnumerator updateHideness(string enemy, float period)
    {
        while (true)
        {
            if (!information.ContainsKey(enemy))
            {
                break;
            }

            distanceToMarshall = Vector2.Distance(transform.position, marshall.transform.position);
            if (distanceToMarshall < safetyConst * information[enemy].dist + safetyBias)
            {
                information[enemy].isHided = true;
            }
            else
            {
                information[enemy].isHided = false;
            }

            yield return new WaitForSecondsRealtime(period);
        }
    }
}
