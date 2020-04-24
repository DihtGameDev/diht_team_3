using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointerController : MonoBehaviour
{

    public float resoluionX;
    public float resoluionY;
    RectTransform transformThis;
    GameObject link;
    GameObject camera;
    GameObject marshall;

    [Range(1f, 3f)]
    public float alignBeat;
    [Range(0.3f, 1f)]
    public float comeArea;

    Vector2 direction;

    public float newX;
    public float newY;

    private Vector3 target;
   
    public GameObject[] targets;
    private Queue<GameObject> targets_queue = new Queue<GameObject>();
    // Start is called before the first frame update
    private Vector2 pos;
    void Awake()
    {
        alignBeat = 1.7f;
        comeArea = 0.5f;

        transformThis = this.GetComponent<RectTransform>();
        link = GameObject.Find("Link").gameObject;
        camera = GameObject.FindGameObjectWithTag("MainCamera").gameObject;
        marshall = GameObject.FindGameObjectWithTag("Marshall").gameObject;

       

        foreach (var tar in targets) {
            targets_queue.Enqueue(tar);
        }
        targets = null;

        

        target = link.transform.position;
        if (targets_queue.Count> 0) { 
        
            target = targets_queue.Dequeue().transform.position;
        }
        StartCoroutine(release(1f, 1f));


    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(marshall.transform.position, target) < comeArea)
        {
            StartCoroutine(release(1f, 0f));
            target = targets_queue.Dequeue().transform.position;
        }
        pos = -Camera.main.WorldToScreenPoint(camera.transform.position) + Camera.main.WorldToScreenPoint(target);

        resoluionX = Screen.width / transform.parent.GetComponent<RectTransform>().localScale.x;
        resoluionY = Screen.height / transform.parent.GetComponent<RectTransform>().localScale.y;


        newX = Mathf.Clamp(pos.x/ alignBeat, -resoluionX / 2f + 45f, resoluionX / 2f - 45f);
        newY = Mathf.Clamp(pos.y/ alignBeat, -resoluionY / 2f + 45f, resoluionY / 2f - 45f);

        direction = target - marshall.transform.position;
        transformThis.localPosition = new Vector2(newX, newY);
        transformThis.rotation = Quaternion.Euler(0f, 0f, - 90f + Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x));
    }

    public IEnumerator release(float time = 0f, float offset = 0f) {
        float start_transparency = this.GetComponent<Image>().color.a;
        float transparency = 0f;

        this.GetComponent<Image>().color = new Color(this.GetComponent<Image>().color.r,
            this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, transparency);
        yield return new WaitForSeconds(offset);
        while (this.GetComponent<Image>().color.a < start_transparency) { 
            transparency += start_transparency / 30f;
            this.GetComponent<Image>().color = new Color(this.GetComponent<Image>().color.r,
                    this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, transparency);  
             yield return new WaitForSeconds(time / 30f);
        }
    }
}
