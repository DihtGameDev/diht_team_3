using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Audio;


public class DialogeTrigger : MonoBehaviour
{
    public GameObject doctor;

    public GameObject LyingMarshall;
    private GameObject marshall;

    public GameObject MHead;
    public GameObject healhBar;
    public GameObject pointer;


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
       
        start_dialogue_color = new Color(0.328202f, 0.3001513f, 0.8962264f);
        other_dialogue_color = new Color(0.3007848f, 0.1093361f, 0.5943396f);
        dController = FindObjectOfType<DialogeController>();


        audioController = GameObject.Find("AudioManager").gameObject.GetComponent<AudioController>();
        MHead.SetActive(false);
        healhBar.SetActive(false);
        pointer.SetActive(false);

        Cursor.visible = false;

        TriggerDialoge();

        StartCoroutine(ComingDoctor());
        StartCoroutine(audioController.Play("NeonLamp", 3f));
        StartCoroutine(audioController.Play("BackGround"));
        StartCoroutine(audioController.ChangeVolume("BackGround", audioController.GetClipRelevantVolume("BackGround") * 0.25f));
        StartCoroutine(audioController.Play("RoomAnxiety", 3f));
    
        dialoge.sentences[dialoge.sentences.Length - 2] = "Press " + Global.moveRight.ToString();
        dialoge.sentences[dialoge.sentences.Length - 1] = "Press " + Global.moveLeft.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        if (dController.pointer == 2) {
            StartCoroutine(Close(4f));    
        }
        if (dController.pointer == 4)
        {
            StartCoroutine(Display(0.4f));
        }
        if (dController.pointer == 6)
        {
            StartCoroutine(Close(4f));
        }
        if (dController.pointer == 8)
        {
            StartCoroutine(changeColor(0.39f, start_dialogue_color));
            StartCoroutine(Display(0.4f));
        }

        if (dController.pointer == 10)
        {
            StartCoroutine(Close(3f));
        }

        if(dController.pointer == 12) { 
            StartCoroutine(changeColor(0.39f, other_dialogue_color));
            StartCoroutine(Display(0.4f));
        }

        if (dController.pointer == 14)
        {
            StartCoroutine(Close(4f));
        }
        if (dController.pointer == 16)
        {
            StartCoroutine(Display(0.4f));
        }
        if (dController.pointer == 18)
        {
            StartCoroutine(Close(3f));
        }
        if (dController.pointer == 20)
        {
            StartCoroutine(changeColor(0.39f, start_dialogue_color));
            StartCoroutine(Display(0.4f));
        }
        if (dController.pointer == 22)
        {
            StartCoroutine(Close(0.2f));
        }
        if (dController.pointer == 24)
        {
            StartCoroutine(glitch(0.0f));
            StartCoroutine(Display(0.4f));
        }
        if (dController.pointer == 26)
        {
            StartCoroutine(Close(3.5f, false));
        }
        if (dController.pointer == 28)
        {
            StartCoroutine(Display(0.5f));
        }

        if (dController.pointer == 30)
        {
            StartCoroutine(Close(4f));
        }
        if (dController.pointer == 32)
        {
            StartCoroutine(changeColor(0.39f, other_dialogue_color));
            StartCoroutine(Display(0.4f));
        }
        if (dController.pointer == 34)
        {
            StartCoroutine(Close(4f));
        }
        if (dController.pointer == 36)
        {
            fading.Play();
            StartCoroutine(changeColor(6.39f, start_dialogue_color));
            StartCoroutine(Display(6.4f));
        }

        if (dController.pointer == 38)
        {
            StartCoroutine(Close(4f));
        }

        if (dController.pointer == 40)
        {
            StartCoroutine(changeColor(0.39f, other_dialogue_color));
            StartCoroutine(Display(0.4f));
        }

        if (dController.pointer == 42)
        {
            StartCoroutine(Close(0.2f));
        }
        if (dController.pointer == 44)
        {
            StartCoroutine(changeColor(0.39f, start_dialogue_color));
            StartCoroutine(Display(0.4f));
            StartCoroutine(furiating(0.65f));
        }

        if (dController.pointer == 46)
        {
            StartCoroutine(Close(0.5f));
        }
        if (dController.pointer == 48)
        {
            StartCoroutine(changeColor(0.39f, other_dialogue_color));
            StartCoroutine(Display(0.4f));
        }

        if (dController.pointer == 50)
        {
            StartCoroutine(Close(4f));
        }
        if (dController.pointer == 52)
        {
            StartCoroutine(Display(0.4f));
        }

        if (dController.pointer == 54)
        {
            StartCoroutine(Close(4f));
        }
        if (dController.pointer == 56)
        {
            StartCoroutine(Display(0.4f));
        }

        if (dController.pointer == 58)
        {
            StartCoroutine(Close(4f));
        }
        if (dController.pointer == 60)
        {
            StartCoroutine(Display(4f));

        }
        if (dController.pointer == 62)
        {
            StartCoroutine(Close(0f));
        }
        if (dController.pointer == 64)
        {
            StartCoroutine(changeColor(0.39f, start_dialogue_color));
            StartCoroutine(Display(12.5f));
            StartCoroutine(ExitingDoctor(0.3f));
        }

        if (dController.pointer == 66)
        {
            StartCoroutine(Close(3f, false));
        }
        if (dController.pointer == 68)
        {
            StartCoroutine(Display(9.3f));
            StartCoroutine(startGame(4.3f));
            
            StartCoroutine(audioController.Stop("RoomAnxiety", 5f));
        }

        if (Input.GetKeyDown(Global.moveRight) && dController.pointer == 70) {
            TriggerDialoge();
            StartCoroutine(Close(0.2f, false));
        }

        if (dController.pointer == 72)
        {
            StartCoroutine(Display(0.5f));
            
        }
        if (Input.GetKeyDown(Global.moveLeft) && dController.pointer == 74) {
            TriggerDialoge();
            StartCoroutine(Close(0.2f, false));
            StartCoroutine(pointer.GetComponent<PointerController>().release(0.2f));
        }

        dialoge.sentences[dialoge.sentences.Length - 2] = "Press " + Global.moveRight.ToString();
        dialoge.sentences[dialoge.sentences.Length - 1] = "Press " + Global.moveLeft.ToString();
    }
    
    public void TriggerDialoge()
    {
        dController.StartDialoge(dialoge);
    }

    IEnumerator Display(float time) {
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
        doctor.transform.Find("DoctorRenderer").GetComponent<SpriteRenderer>().flipX = false;
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
