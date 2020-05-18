using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogeTriggerHospital2 : MonoBehaviour
{

    public bool start = false;
    [SerializeField]
    private GameObject marshall;
    private MarshallController marshallController;

    public GameObject enemy;
    [SerializeField]
    private GameObject camera;
    private CameraScript cameraController;

    public bool isPressedShift = false;

    [SerializeField]
    private Vector3 cameraPosition;
    [SerializeField]
    DialogeController dController;

    public Dialoge dialoge;


    // Start is called before the first frame update
    void Start()
    {
        marshall = GameObject.FindGameObjectWithTag("Marshall").gameObject;
        camera = GameObject.FindGameObjectWithTag("MainCamera").gameObject;

        marshallController = marshall.GetComponent<MarshallController>();
        cameraController = camera.GetComponent<CameraScript>();

        dController = FindObjectOfType<DialogeController>();
        TriggerDialoge();

        dialoge.sentences[0] = "Press " + Global.sitDown.ToString() + " to bend down";
    }

    // Update is called once per frame
    void Update()
    {
 
        if (Input.GetKeyDown(Global.sitDown) && Time.timeScale != 0 && dController.pointer == 2) {
            
            StartCoroutine(Close(0.1f));
            StartCoroutine(comeBack());
        }

        if (marshall.transform.position.x >= 6.3f) {

            StartCoroutine(Display(0.05f));
        }
        if (marshall.transform.position.x >= 7.5f && dController.pointer == 6)

        {
            StartCoroutine(Close(0.05f));
        }

        dialoge.sentences[0] = "Press " + Global.sitDown.ToString() + " to bend down";
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



    IEnumerator attention() {
        cameraPosition = camera.transform.position;
        cameraController.isRestricted = true;
        Time.timeScale = 0.1f;
       
        while (Vector2.Distance(camera.transform.position, enemy.transform.position) >= 0.2f) {

            camera.transform.position = Vector3.MoveTowards(camera.transform.position,
                new Vector3(enemy.transform.position.x, enemy.transform.position.y, cameraController.camera_Offset), 1f * Time.timeScale);
            yield return null;
        }
    }

    IEnumerator comeBack()
    {
        Time.timeScale = 1f;
 
        while (Vector3.Distance(camera.transform.position, cameraPosition) >= 0.1f)
        {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position,
                cameraPosition, 0.15f * Time.timeScale);
            yield return null;
        }

        cameraController.isRestricted = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Marshall"))
        {
            if (start == false) {
                start = true;
                TriggerDialoge();
                StartCoroutine(Display(0.1f));
                StartCoroutine(attention());
            }
           
        }
    }

}
