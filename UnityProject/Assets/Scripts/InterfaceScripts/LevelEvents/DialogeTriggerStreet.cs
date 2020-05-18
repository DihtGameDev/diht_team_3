using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Audio;

public class DialogeTriggerStreet : MonoBehaviour
{
  
    private GameObject marshall;

    public GameObject MHead;
    public GameObject healhBar;
    public GameObject pointer;

    public PlayableDirector fading;

    [SerializeField]
    DialogeController dController;

    [SerializeField]
    private AudioController audioController;


    public Dialoge dialoge;
    // Start is called before the first frame update

    private void Awake()
    {
        // !!!Dangerous, if doesn't exist then the code below doesn't realising!!!
        marshall = FindObjectOfType<MarshallController>().gameObject;
    }
    void Start()
    {

        dController = FindObjectOfType<DialogeController>();
        //dController.flexible_speed_of_typing = true;

        audioController = GameObject.Find("AudioManager").gameObject.GetComponent<AudioController>();
        MHead.SetActive(false);
        healhBar.SetActive(false);
        pointer.SetActive(false);


        TriggerDialoge();

        StartCoroutine(audioController.ChangeVolume("BackGround", audioController.GetClipRelevantVolume("BackGround") * 0.75f));
        StartCoroutine(audioController.Play("NeonLamp", 0f));
        StartCoroutine(audioController.Play("BackGround", 2f));
        

        StartCoroutine(Display(2f));
    }

    // Update is called once per frame
    void Update()
    {

        if (dController.pointer == 2)
        {
            StartCoroutine(Close(2.6f));
        }
        if (dController.pointer == 4)
        {
            StartCoroutine(Display(0.4f));
            StartCoroutine(glitch(0.6f));
        }
        if (dController.pointer == 6)
        {
            StartCoroutine(Close(2.5f));
        }

        if (dController.pointer == 8)
        {

            StartCoroutine(Display(0.4f));
        }

        if (dController.pointer == 10)
        {
            StartCoroutine(Close(3.5f));
        }

        if (dController.pointer == 12)
        {

            StartCoroutine(Display(0.4f));
        }

        if (dController.pointer == 14)
        {
            StartCoroutine(Close(3.3f));
        }
        if (dController.pointer == 16)
        {
            StartCoroutine(Display(0.4f));
 
        }
        if (dController.pointer == 18)
        {
            StartCoroutine(Close(3.4f));
        }

        if (dController.pointer == 20)
        {
            
            StartCoroutine(Display(0.4f));
            StartCoroutine(fade(0.6f));
        }
        if (dController.pointer == 22)
        {
            StartCoroutine(Close(2.8f, false));
        }
        if (dController.pointer == 24)
        {
            StartCoroutine(Display(0.4f));
        }
        if (dController.pointer == 26)
        {
            StartCoroutine(Close(2.8f));
        }
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

    IEnumerator Close(float time, bool skipAllowed = true)
    {
        dController.pointer++;
        StartCoroutine(dController.CloseSentence(time, skipAllowed));
        yield return null;
    }


    IEnumerator glitch(float offset)
    {
        yield return new WaitForSecondsRealtime(offset);
        StartCoroutine(marshall.GetComponent<MarshallController>().glitch(0.3f));
    }

    IEnumerator fade(float offset)
    {
        yield return new WaitForSecondsRealtime(offset);
        fading.Play();
    }
}
