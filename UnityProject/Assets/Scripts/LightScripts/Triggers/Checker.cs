using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{

    public bool targetIsObserving = false;

    [SerializeField]
    private int numerOfObstacles;

    private float distanceOfTriggering;

    public GameObject target;
    public GameObject parent;

    EdgeCollider2D view;

    [HideInInspector]
    public List<Vector2> newVerticies = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        distanceOfTriggering = parent.GetComponent<TriggerLogic>().distanceOfTriggering;
        numerOfObstacles = 0;
        view = GetComponent<EdgeCollider2D>();

        newVerticies.Add(Vector2.zero);
        newVerticies.Add(target.transform.position - transform.position);
        StartCoroutine(updateTriggerDetect(0.2f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(newVerticies[0], newVerticies[1]) < distanceOfTriggering
            && numerOfObstacles == 0)
        {
            targetIsObserving = true;
        }
        else {
            targetIsObserving = false;
        }
        if (parent == null || target == null) {
            Destroy(gameObject);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            numerOfObstacles++;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Obstacle"))
        {
            numerOfObstacles--;
        }
    }

    IEnumerator updateTriggerDetect(float period)
    {
        while (true) {
            newVerticies[0] = Vector2.zero;
            newVerticies[1] = target.transform.position - transform.position;
            view.points = newVerticies.ToArray();
            yield return new WaitForSeconds(period);
        }

    }
    
}
