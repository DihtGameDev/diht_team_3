using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogeTriggerHospital4 : MonoBehaviour
{

    [SerializeField]
    DialogeController dController;

    public Dialoge dialoge;
    // Start is called before the first frame update
    void Start()
    {
        dController = FindObjectOfType<DialogeController>();

        TriggerDialoge();

        StartCoroutine(Display(3.5f));
       // dController.isTypingScaled = false;
        StartCoroutine(Close(8f));
    }


    // Update is called once per frame
    void Update()
    {
        
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
