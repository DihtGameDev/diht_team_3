using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogeTriggerHospital3 : MonoBehaviour
{
 

    public bool start = false;
    [SerializeField]
    private GameObject marshall;
    [SerializeField]
    private MarshallController marshallController;

    public GameObject enemy;
    public GameObject light;
    [SerializeField]
    private GameObject camera;
    [SerializeField]
    private CameraScript cameraController;


    [SerializeField]
    private Vector3 cameraPosition;
    [SerializeField]
    DialogeController dController;

    public Dialoge dialoge;

    public SwitcherLogic switcher;
    // Start is called before the first frame update
    void Start()
    {
        marshall = GameObject.FindGameObjectWithTag("Marshall").gameObject;
        camera = GameObject.FindGameObjectWithTag("MainCamera").gameObject;

        marshallController = marshall.GetComponent<MarshallController>();
        cameraController = camera.GetComponent<CameraScript>();

        dController = FindObjectOfType<DialogeController>();
        TriggerDialoge();

        dialoge.sentences[0] = "Press " + Global.action.ToString() + " to switch off the light";
    }


    
    // Update is called once per frame
    void Update()
    {
        if (dController.pointer == 2) {
            marshallController.isRestricted = false;
        }

        dialoge.sentences[0] = "Press " + Global.action.ToString() + " to switch off the light";

    }

    public void TriggerDialoge()
    {

        dController.StartDialoge(dialoge);
    }

    IEnumerator Display(float time)
    {
        dController.pointer++;
        yield return new WaitForSecondsRealtime(time);

        dController.DisplayNextSentence();
    }

    IEnumerator Close(float time, bool skipAllowed = false)
    {
        dController.pointer++;
        StartCoroutine(dController.CloseSentence(time, skipAllowed));
        yield return null;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Marshall"))
        {

            if (start == false)
            {
                start = true;

                StartCoroutine(switcher.lightSignal());
                TriggerDialoge();
                StartCoroutine(Display(0f));
                marshallController.isRestricted = true;
                StartCoroutine(attention(light, 1.2f, 0.3f));
            }        
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Marshall"))
        {
            if (Input.GetKeyDown(Global.action) && dController.pointer == 2 && switcher.isAlloedToPress)
            {            
                StartCoroutine(Display(0.0f));
                StartCoroutine(attention(enemy, 2f, 0.3f));
            }
        }
    }

    IEnumerator attention(GameObject obj, float wait_on_obj, float speed)
    {
        cameraPosition = camera.transform.position;
        cameraController.isRestricted = true;
        Time.timeScale = 0.2f;
        
        while (Vector2.Distance(camera.transform.position, obj.transform.position) >= 0.4f)
        {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position,
                new Vector3(obj.transform.position.x, obj.transform.position.y, cameraController.camera_Offset), speed * Time.timeScale);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(wait_on_obj);
        StartCoroutine(comeBack(speed/2f));
    }

    IEnumerator comeBack(float speed)
    {

        while (Vector3.Distance(camera.transform.position, cameraPosition) >= 0.2f)
        {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position,
                cameraPosition, speed * Time.timeScale);
            yield return null;
        }
        if (dController.pointer == 4)
        {
            StartCoroutine(Close(0.2f));
        }

        cameraController.isRestricted = false;

        if (dController.pointer == 2) {
            switcher.isAlloedToPress = true;
        }

        Time.timeScale = 1f;
    }

    
}
