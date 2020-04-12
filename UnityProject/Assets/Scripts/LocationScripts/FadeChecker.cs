using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class FadeChecker : MonoBehaviour
{
    [SerializeField]    
    private float speedOfFading;

    private GameObject marshall;
    
    float cur_transparency;
    float targ_transparency;
    [SerializeField]
    const float transparency = 0.25f;

    [SerializeField]
    List<GameObject> children;

    // Start is called before the first frame update
    void Start()
    {
        speedOfFading = 2f;

        children = new List<GameObject>();
        marshall = GameObject.FindGameObjectWithTag("Marshall").gameObject;

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).GetComponent<SpriteRenderer>() != null ||
                transform.parent.GetChild(i).GetComponent<Tilemap>() != null)
            {               
                children.Add(transform.parent.GetChild(i).gameObject);
            }       
        }

        targ_transparency = 1f;
        cur_transparency = targ_transparency;
    }

    // Update is called once per frame
    void Update()
    {
        cur_transparency = Mathf.MoveTowards(cur_transparency, targ_transparency, speedOfFading * Time.deltaTime);
        foreach (var child in children)
        {
            if (child.GetComponent<SpriteRenderer>() != null)
            {
                child.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, cur_transparency);
            }
            else {
                child.GetComponent<Tilemap>().CompressBounds();
                child.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, cur_transparency);
            }
            
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Marshall")
        {
            targ_transparency = transparency;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Marshall")
        {
            targ_transparency = 1f;
        }
    }
}
