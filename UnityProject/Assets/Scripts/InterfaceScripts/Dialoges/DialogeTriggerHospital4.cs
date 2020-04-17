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

        StartCoroutine(Display(2.5f));
        StartCoroutine(Close(7f));
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerDialoge()
    {
        Debug.Log("Trig");
        dController.StartDialoge(dialoge);
    }

    IEnumerator Display(float time)
    {
        dController.pointer++;
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
