using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogeTriggerHospital1 : MonoBehaviour
{
    public bool isPressedW = false;
    public bool isPressedS = false;
    public bool isLookedAround = false;

    private int pointer = 1;
    public Vector2 tmpPosition;
    [SerializeField]
    DialogeController dController;

    public Dialoge dialoge;
    // Start is called before the first frame update
    void Start()
    {

        dController = FindObjectOfType<DialogeController>();

        TriggerDialoge();

        StartCoroutine(Display(1.2f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && dController.pointer == pointer)
        {
            pointer++;
            StartCoroutine(Close(0.2f));
            StartCoroutine(Display(0.7f));
        }

        if (Input.GetKeyDown(KeyCode.S) && dController.pointer == pointer)
        {
            pointer++;
            StartCoroutine(Close(0.2f));
            StartCoroutine(Display(0.7f));
        }


        if (Vector2.Distance(Input.mousePosition, tmpPosition) > 100f && dController.pointer == pointer) {
            pointer++;
            isLookedAround = true;
            StartCoroutine(Close(0.3f));
        }
    }

    public void TriggerDialoge()
    {
        Debug.Log("Trig");
        dController.StartDialoge(dialoge);
    }

    IEnumerator Display(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("TrigDisplay");
        dController.DisplayNextSentence();
    }

    IEnumerator Close(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("TrigClose");
        dController.CloseSentence();
    }
}
