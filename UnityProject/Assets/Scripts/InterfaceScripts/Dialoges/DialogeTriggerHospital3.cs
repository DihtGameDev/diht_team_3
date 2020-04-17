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

        TriggerDialoge();
    }


    
    // Update is called once per frame
    void Update()
    {
        if (dController.pointer == 2) {
            marshallController.isRestricted = false;
        }
      
    }

    public void TriggerDialoge()
    {

        dController.StartDialoge(dialoge);
    }

    IEnumerator Display(float time)
    {
        dController.pointer++;
        yield return new WaitForSecondsRealtime(time);
        Debug.Log("TrigDisplay");
        dController.DisplayNextSentence();
    }

    IEnumerator Close(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Debug.Log("TrigClose");
        dController.CloseSentence();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Marshall"))
        {
            if (start == false)
            {
                start = true;
                StartCoroutine(Display(0f));
                marshallController.isRestricted = true;
                StartCoroutine(attention(light, 1.2f, 0.08f));
            }        
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Marshall"))
        {
            if (Input.GetKeyDown(KeyCode.Space) && dController.pointer == 2 && switcher.isAlloedToPress)
            {
                StartCoroutine(Display(0.0f));
                StartCoroutine(attention(enemy, 2f, 0.08f));
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
                new Vector3(obj.transform.position.x, obj.transform.position.y, cameraController.camera_Offset), speed);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(wait_on_obj);
        StartCoroutine(comeBack(speed/2f));
    }

    IEnumerator comeBack(float speed)
    {

        while (Vector3.Distance(camera.transform.position, cameraPosition) >= 0.1f)
        {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position,
                cameraPosition, speed);
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
