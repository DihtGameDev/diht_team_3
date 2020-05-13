using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DialogeTriggerBob : MonoBehaviour
{
    public GameObject bob;

    private GameObject marshall;
    public GameObject coolerMarshall;

    public GameObject MHead;
    public GameObject healhBar;
    public GameObject pointer;


    [SerializeField]
    DialogeController dController;
    private Color start_dialogue_color;
    private Color other_dialogue_color;

    public PlayableDirector glitchbob;
    public PlayableDirector fading;
    public PlayableDirector furiate;
    public PlayableDirector exitTimeline;


    [SerializeField]
    private AudioController audioController;

    [SerializeField]
    EventSystem eventSystem;
    public GameObject choiceMenu;

    public Dialoge dialoge;
    // Start is called before the first frame update

    private void Awake()
    {
        // !!!Dangerous, if doesn't exist then the code below doesn't realising!!!
        marshall = GameObject.Find("Marshall").gameObject;
    }
    void Start()
    {
        start_dialogue_color = new Color(0.1334995f, 0.1535427f, 0.3773585f);
        other_dialogue_color = new Color(0.03150587f, 0.0380947f, 0.1132075f);
        dController = FindObjectOfType<DialogeController>();

        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").gameObject.GetComponent<EventSystem>();

        audioController = GameObject.Find("AudioManager").gameObject.GetComponent<AudioController>();
        MHead.SetActive(false);
        healhBar.SetActive(false);
        pointer.SetActive(false);

        Cursor.visible = false;

        TriggerDialoge();

        StartCoroutine(audioController.Play("RoomAnxiety", 3f));
        

    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;

        if (dController.pointer == 2)
        {
            StartCoroutine(Close(3f));
        }
        if (dController.pointer == 4)
        {
            StartCoroutine(changeColor(0.3f, start_dialogue_color));
            StartCoroutine(Display(0.4f));// Слушай Боб..
        }
        if (dController.pointer == 6)
        {
            StartCoroutine(Close(0.6f));
        }
        if (dController.pointer == 8)
        {
            StartCoroutine(changeColor(0.3f, other_dialogue_color));
            StartCoroutine(Display(0.4f)); //Кажется в..
        }

        if (dController.pointer == 10)
        {
            StartCoroutine(Close(1.5f));
        }

        if (dController.pointer == 12)
        {
            StartCoroutine(Display(0.4f)); // "В гробу я видел
        }

        if (dController.pointer == 14)
        {
            StartCoroutine(Close(0.4f));
        }
        if (dController.pointer == 16)
        {
            StartCoroutine(changeColor(0.39f, start_dialogue_color));
            StartCoroutine(Display(0.4f)); // Да блятб..
            //StartCoroutine(furiating(0.4f));
        }
        if (dController.pointer == 18)
        {
            StartCoroutine(Close(2f));
        }
        if (dController.pointer == 20)
        {
          
            StartCoroutine(Display(0.4f)); // А учитывая..
        }
        if (dController.pointer == 22)
        {
            StartCoroutine(Close(2f));
        }
        if (dController.pointer == 24)
        {

            StartCoroutine(Display(0.4f));// Любезностями..
        }
        if (dController.pointer == 26)
        {
            StartCoroutine(Close(3f));
        }
        if (dController.pointer == 28)
        {
            StartCoroutine(changeColor(0.4f, other_dialogue_color));
            StartCoroutine(Display(0.5f));//...
        }

        if (dController.pointer == 30)
        {
            StartCoroutine(Close(2f));
        }
        if (dController.pointer == 32)
        {

            StartCoroutine(Display(0.4f)); // Рассказывай
        }
        if (dController.pointer == 34)
        {
            StartCoroutine(Close(2f));
        }
        if (dController.pointer == 36)
        {
            StartCoroutine(changeColor(1.9f, start_dialogue_color));
            StartCoroutine(Display(2f)); // Я мало что помню..
        }

        if (dController.pointer == 38)
        {
            StartCoroutine(Close(2f));
        }

        if (dController.pointer == 40)
        {
            StartCoroutine(Display(0.4f)); // Мы ехали..
            StartCoroutine(fade(0.5f));
        }

        if (dController.pointer == 42)
        {
            StartCoroutine(Close(3f));
        }
        if (dController.pointer == 44)
        {
            StartCoroutine(Display(0.4f)); // В машине был видеорегистр..

        }

        if (dController.pointer == 46)
        {
            StartCoroutine(Close(3f));
        }
        if (dController.pointer == 48)
        {
            StartCoroutine(changeColor(0.4f, other_dialogue_color));
            StartCoroutine(Display(0.5f));// Я знаю..
        }

        if (dController.pointer == 50)
        {
            StartCoroutine(Close(4f));
        }
        if (dController.pointer == 52)
        {
            StartCoroutine(Display(0.4f)); // но учти..
        }

        if (dController.pointer == 54)
        {
            StartCoroutine(Close(4f));
        }
        if (dController.pointer == 56)
        {
            StartCoroutine(Display(0.4f)); // Если ты выполнишь..
        }

        if (dController.pointer == 58)
        {
            StartCoroutine(Close(3f));
        }
        if (dController.pointer == 60)
        {
            StartCoroutine(changeColor(0.39f, start_dialogue_color)); // ты шутишь?.. 
            StartCoroutine(Display(0.4f));

        }
        if (dController.pointer == 62)
        {
            StartCoroutine(Close(0.3f));
        }
        if (dController.pointer == 64)
        {
            StartCoroutine(changeColor(0.39f, other_dialogue_color));
            StartCoroutine(Display(0.4f)); // Слушай дружище..
           
        }

        if (dController.pointer == 66)
        {
            StartCoroutine(Close(2.5f));
        }
        if (dController.pointer == 68)
        {
            StartCoroutine(Display(0.4f));// Таких

        }

        if (dController.pointer == 70)
        {
            StartCoroutine(Close(2.5f));
        }
        if (dController.pointer == 72)
        {
            StartCoroutine(Display(0.4f));// Но ты в отч..

        }
        if (dController.pointer == 74)
        {
            StartCoroutine(Close(3f));
        }
        if (dController.pointer == 76)
        {
            StartCoroutine(Display(0.4f)); // и твое желание..

        }
        if (dController.pointer == 78)
        {
            StartCoroutine(Close(4f));
        }
        if (dController.pointer == 80)
        {
            StartCoroutine(Display(0.4f));// И что то мне..

        }
        if (dController.pointer == 82)
        {
            StartCoroutine(Close(4f));
        }
       
        if (dController.pointer == 84)
        {
            StartCoroutine(Display(0.4f)); // Ну или ты можешь..

        }
        if (dController.pointer == 86)
        {
            StartCoroutine(Close(4f));
        }
        if (dController.pointer == 88)
        {
            StartCoroutine(changeColor(0.0f, start_dialogue_color));
            StartCoroutine(Display(0.4f)); // господи

        }
        if (dController.pointer == 90)
        {
            StartCoroutine(Close(3f));
        }
        if (dController.pointer == 92)
        {
            
            StartCoroutine(Display(0.4f));// от этих..
        }
        if (dController.pointer == 94)
        {
            StartCoroutine(glitch(0.0f));
            StartCoroutine(Close(0.4f));
        }
        if (dController.pointer == 96)
        {
            StartCoroutine(Display(0.4f)); // Да что это
        }
        if (dController.pointer == 98)
        {
            StartCoroutine(Close(4f));
        }
        if (dController.pointer == 100)
        {
            StartCoroutine(Display(0.4f));// Что за работа

        }
        if (dController.pointer == 102)
        {
            StartCoroutine(Close(4f));
        }
        if (dController.pointer == 104)
        {
            StartCoroutine(changeColor(0.39f, start_dialogue_color));
            StartCoroutine(Display(0.4f)); // Нужно выкрасть..
        }

        if (dController.pointer == 106)
        {
            StartCoroutine(Close(3f));
        }
        if (dController.pointer == 108)
        {
            StartCoroutine(Display(0.4f));// Оборудование

        }

        if (dController.pointer == 110)
        {
            StartCoroutine(Close(2.5f));
        }
        if (dController.pointer == 112)
        {
            StartCoroutine(Display(0.4f));// Единственное

        }

        if (dController.pointer == 114)
        {
            StartCoroutine(Close(3f));
        }
        if (dController.pointer == 116)
        {
            StartCoroutine(Display(0.4f));// Хотя выбора
            StartCoroutine(fade(0.4f));
        }

        if (dController.pointer == 118)
        {
            StartCoroutine(Close(4f));
        }

        if (dController.pointer == 120)
        {
            StartCoroutine(Display(0.4f));// Ну так что?
 
        }

        if (dController.pointer == 122)
        {
            StartCoroutine(Close(4f));
            startChoice();
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

    IEnumerator changeColor(float offset, Color color)
    {
        yield return new WaitForSecondsRealtime(offset);
        dController.rec.GetComponent<Image>().color = color;

    }
    IEnumerator furiating(float offset)
    {
        yield return new WaitForSecondsRealtime(offset);
        furiate.Play();
    }

    IEnumerator glitch(float offset)
    {
        yield return new WaitForSecondsRealtime(offset);
        glitchbob.Play();
    }

    IEnumerator fade(float offset)
    {
        yield return new WaitForSecondsRealtime(offset);
        fading.Play();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Marshall"))
        {
            marshall.SetActive(false);
            coolerMarshall.SetActive(true);
            StartCoroutine(changeColor(0.39f, other_dialogue_color));
            StartCoroutine(Display(0.0f));

        }
    }


    void startChoice() {
 
        choiceMenu.SetActive(true);

        eventSystem.SetSelectedGameObject(choiceMenu.transform.GetChild(0).gameObject);

        Time.timeScale = 0f;

    }

    public void choiceIsDone() {
        StartCoroutine(choice());
    }

    IEnumerator choice() {
        Time.timeScale = 1f;
        exitTimeline.Play();
        StartCoroutine(audioController.Stop("RoomAnxiety", 2f, 2f));
        yield return new WaitForSecondsRealtime((float)exitTimeline.duration + 10f);
        SceneManager.LoadScene(0);
    }
}
