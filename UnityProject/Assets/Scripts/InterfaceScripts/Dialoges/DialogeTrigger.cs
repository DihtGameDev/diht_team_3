using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogeTrigger : MonoBehaviour
{
    
    [SerializeField]
    DialogeController dController;

    public Dialoge dialoge;
    // Start is called before the first frame update
    void Start()
    {
        dController = FindObjectOfType<DialogeController>();

        TriggerDialoge();

        StartCoroutine(Display(1.8f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && dController.pointer == 1) {

            StartCoroutine(Close(0.1f));
            StartCoroutine(Display(0.6f));
        }
        if (Input.GetKeyDown(KeyCode.A) && dController.pointer == 2) {
            StartCoroutine(Close(0.1f));
        }
    }
    
    public void TriggerDialoge()
    {
        Debug.Log("Trig");
        dController.StartDialoge(dialoge);
    }

    IEnumerator Display(float time) {
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
