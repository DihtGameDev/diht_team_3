using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;


public class DialogeTrigger : MonoBehaviour
{
    public GameObject doctor;

    public GameObject LyingMarshall;
    public GameObject marshall;

    public Vector2 room_position;
    public Vector2 un_room_position;

    [SerializeField]
    DialogeController dController;
    private Color start_dialogue_color;
    private Color other_dialogue_color;

    public PlayableDirector glitchDoctor;
    public PlayableDirector fading;
    public PlayableDirector furiate;
    public PlayableDirector patience;

    public Dialoge dialoge;
    // Start is called before the first frame update
    void Start()
    {
       
        start_dialogue_color = new Color(0.328202f, 0.3001513f, 0.8962264f);
        other_dialogue_color = new Color(0.3007848f, 0.1093361f, 0.5943396f);
        dController = FindObjectOfType<DialogeController>();

        TriggerDialoge();

        StartCoroutine(ComingDoctor());

    }

    // Update is called once per frame
    void Update()
    {

        if (dController.pointer == 2) {
            StartCoroutine(Close(1.5f));
            StartCoroutine(Display(1.9f));
        }
        if (dController.pointer == 4)
        {
            StartCoroutine(Close(1.5f));
            StartCoroutine(changeColor(1.9f, start_dialogue_color));
            StartCoroutine(Display(2f));
        }
        if (dController.pointer == 6)
        {
            StartCoroutine(Close(1.5f));
            StartCoroutine(changeColor(1.8f, other_dialogue_color));
            StartCoroutine(Display(2.1f));
        }
        if (dController.pointer == 8)
        {
            StartCoroutine(Close(1.6f));
            StartCoroutine(Display(2f));
        }
        if (dController.pointer == 10)
        {
            StartCoroutine(Close(0.6f));
            StartCoroutine(changeColor(1f, start_dialogue_color));
            StartCoroutine(Display(1.1f));
        }
        if (dController.pointer == 12)
        {
            StartCoroutine(Close(0.0f));
            StartCoroutine(glitch(0.18f));
            StartCoroutine(Display(0.1f));
        }
        if (dController.pointer == 14)
        {
            StartCoroutine(Close(3.5f));
            StartCoroutine(Display(3.9f));
        }

        if (dController.pointer == 16)
        {
            StartCoroutine(Close(1.8f));
            StartCoroutine(changeColor(2.1f, other_dialogue_color));
            StartCoroutine(Display(2.2f));
        }
        if (dController.pointer == 18)
        {
            StartCoroutine(Close(2.2f));
            fading.Play();
            StartCoroutine(changeColor(6.3f, start_dialogue_color));
            StartCoroutine(Display(6.4f));
        }

        if (dController.pointer == 20)
        {
            StartCoroutine(Close(2f));
            StartCoroutine(changeColor(2.3f, other_dialogue_color));
            StartCoroutine(Display(2.4f));
        }

        if (dController.pointer == 22)
        {
            StartCoroutine(Close(0.3f));
            StartCoroutine(changeColor(0.3f, start_dialogue_color));
            StartCoroutine(Display(0.6f));
            StartCoroutine(furiating(0.78f));
        }

        if (dController.pointer == 24)
        {
            StartCoroutine(Close(0.1f));
            StartCoroutine(changeColor(0.4f, other_dialogue_color));
            StartCoroutine(Display(0.5f));

        }

        if (dController.pointer == 26)
        {
            StartCoroutine(Close(1f));
            StartCoroutine(Display(1.4f));
        }

        if (dController.pointer == 28)
        {
            StartCoroutine(Close(1f));
            StartCoroutine(Display(1.4f));
        }

        if (dController.pointer == 30)
        {
            StartCoroutine(Close(1.5f));
            StartCoroutine(Display(4f));

        }
        if (dController.pointer == 32)
        {
            StartCoroutine(Close(0f));
            StartCoroutine(changeColor(0.3f, start_dialogue_color));
            StartCoroutine(Display(12.5f));
            StartCoroutine(ExitingDoctor(0.3f));
        }

        if (dController.pointer == 34)
        {
            StartCoroutine(Close(3f));
            StartCoroutine(Display(9.5f));
            StartCoroutine(startGame(4.1f));
        }

        if (Input.GetKeyDown(KeyCode.D) && dController.pointer == 36) {

            StartCoroutine(Close(0.1f));
            StartCoroutine(Display(0.5f));
        }
        if (Input.GetKeyDown(KeyCode.A) && dController.pointer == 38) {
            StartCoroutine(Close(0.1f));
        }
    }
    
    public void TriggerDialoge()
    {
        Debug.Log("Trig");
        dController.StartDialoge(dialoge);
    }

    IEnumerator Display(float time) {
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

    IEnumerator ComingDoctor() {

        doctor.GetComponent<Animator>().SetBool("isMoving", true);
        while (Vector2.Distance(doctor.transform.position, room_position) > 0.1f) {
            doctor.transform.position = Vector2.MoveTowards(doctor.transform.position, room_position,
                0.8f * Time.deltaTime);
            yield return null;
        }
        doctor.GetComponent<Animator>().SetBool("isMoving", false);
        StartCoroutine(changeColor(0.4f, other_dialogue_color));
        StartCoroutine(Display(1.5f));

    }

    IEnumerator ExitingDoctor(float offset)
    {
        yield return new WaitForSecondsRealtime(offset);
        patience.Play();
        doctor.GetComponent<Animator>().SetBool("isMoving", true);
        doctor.transform.FindChild("DoctorRenderer").GetComponent<SpriteRenderer>().flipX = false;
        while (Vector2.Distance(doctor.transform.position, un_room_position) > 0.1f)
        {
            doctor.transform.position = Vector2.MoveTowards(doctor.transform.position, un_room_position,
                0.8f * Time.deltaTime);
            yield return null;
        }
        
        doctor.GetComponent<Animator>().SetBool("isMoving", false);
        

    }

    IEnumerator startGame(float offset) {
        yield return new WaitForSecondsRealtime(offset);
        
        LyingMarshall.SetActive(false);
        marshall.SetActive(true);

    }
    IEnumerator changeColor(float offset, Color color) {
        yield return new WaitForSecondsRealtime(offset);
        Debug.Log("Changed");
        dController.animator.GetComponent<Image>().color = color;

    }
    IEnumerator furiating(float offset) {
        yield return new WaitForSecondsRealtime(offset);
        furiate.Play();
    }

    IEnumerator glitch(float offset)
    {
        yield return new WaitForSecondsRealtime(offset);
        glitchDoctor.Play();
    }

}
