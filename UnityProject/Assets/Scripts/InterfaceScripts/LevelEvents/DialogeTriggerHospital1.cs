using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DialogeTriggerHospital1 : MonoBehaviour
{

    public bool isPressedW = false;
    public bool isPressedS = false;
    public bool isLookedAround = false;


    public Vector2 tmpPosition;
    [SerializeField]
    DialogeController dController;

    public Dialoge dialoge;

    private AudioController audioController;
    // Start is called before the first frame update
    void Start()
    {
        audioController = GameObject.Find("AudioManager").gameObject.GetComponent<AudioController>();
        dController = FindObjectOfType<DialogeController>();
        TriggerDialoge();

        StartCoroutine(Display(2.0f));
     

        dialoge.sentences[0] = "Press " + Global.moveUp.ToString();
        dialoge.sentences[1] = "Press " + Global.moveDown.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(Global.moveUp) && Time.timeScale != 0 && dController.pointer == 2)
        {
            TriggerDialoge();
            StartCoroutine(Close(0.2f));

        }

        if (dController.pointer == 4)
        {
            StartCoroutine(Display(0.6f));
        }

        if (Input.GetKeyDown(Global.moveDown) && Time.timeScale != 0 && dController.pointer == 6)
        {
            TriggerDialoge();
            StartCoroutine(Close(0.2f));
        }

        if (dController.pointer == 8)
        {
            StartCoroutine(Display(0.6f));
        }


        if (Vector2.Distance(Input.mousePosition, tmpPosition) > 100f && Time.timeScale != 0 && dController.pointer == 10) {

            isLookedAround = true;
            StartCoroutine(Close(0.3f));
        }

        dialoge.sentences[0] = "Press " + Global.moveUp.ToString();
        dialoge.sentences[1] = "Press " + Global.moveDown.ToString();
    }

    public void TriggerDialoge()
    {
        dController.StartDialoge(dialoge);
    }

    IEnumerator Display(float time)
    {
        dController.pointer++;
        yield return new WaitForSeconds(time);

        dController.DisplayNextSentence();
    }

    IEnumerator Close(float time, bool skipAllowed = false)
    {
        dController.pointer++;
        StartCoroutine(dController.CloseSentence(time, skipAllowed));
        yield return null;
    }

}
