using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class DialogeController : MonoBehaviour
{

    public bool flexible_speed_of_typing;
    public int pointer = 0;
    public int index = 0;
    public bool isTyping = false;
    private bool isStopTyping = false;

    private bool isTypedAndShow = false;
    private bool isStopTypedAndShow = false;

    public Text nameText;
    public Text dialogeText;


    public RectTransform rec;


    [SerializeField]
    private List<string> sentences = new List<string>();

    public DialogeTriggerHospital1 mouseTrigger;
    private AudioController audioController;

    public bool isTypingScaled;
    IEnumerator typing;

    // Start is called before the first frame update
    void Start()
    {

        rec.anchoredPosition = new Vector2(0f, -rec.rect.height / 2f);


        mouseTrigger = FindObjectOfType<DialogeTriggerHospital1>();

        audioController = GameObject.Find("AudioManager").gameObject.GetComponent<AudioController>();
        typing = audioController.PlayWithPeriod("Typing", 0.10f, false);
    }

    void Update()
    {
       
        if ((Input.GetKeyDown(Global.action) || Input.GetKeyDown(KeyCode.Return)) && isTyping)
        {
            isStopTyping = true;
        }
        if ((Input.GetKeyDown(Global.action) || Input.GetKeyDown(KeyCode.Return)) && isTypedAndShow) {
            isStopTypedAndShow = true;
        }
    }

    public void StartDialoge(Dialoge dialoge) {

        nameText.text = dialoge.name;
        sentences.Clear();
      
        foreach (string sentence in dialoge.sentences) {
            sentences.Add(sentence);
        }
    }

    public void DisplayNextSentence() {
        if (sentences.Count == index) {
            EndDialoge();
            return;
        }
        string sentence = sentences[index++];
        StartCoroutine(TypeSentence(sentence));

        StartCoroutine(openPanel(210f));
       
    }

    public IEnumerator CloseSentence(float offset, bool skipAllowed) {
        isTypedAndShow = true;

       
        float timer = 0;
        while (timer < offset)
        {
            timer += Time.deltaTime;
            Debug.Log("Close " + timer);
            if (isStopTypedAndShow && skipAllowed)
            {
                isStopTypedAndShow = false;
                isTypedAndShow = false;
                timer = offset;
                break;
            }
            yield return null;
        }


        isTypedAndShow = false;
        StartCoroutine(closePanel(210f));

        pointer++;

    }

    void EndDialoge() {

    }

    IEnumerator TypeSentence(string sentence) {
        
        yield return new WaitForSecondsRealtime(0.1f);
        
        dialogeText.text = "";

        
        typing = audioController.PlayWithPeriod("Typing", 0.10f, false);
        StartCoroutine(typing);
        isTyping = true;
        foreach (char letter in sentence.ToCharArray()) {
            if (isStopTyping) {
                isStopTyping = false;
                isTyping = false;
                dialogeText.text = sentence;
                break;
            }
            dialogeText.text += letter;
            if (Time.timeScale == 0)
            {
                yield return new WaitForSeconds(flexible_speed_of_typing ?
                    (1.3f / sentence.Length) : 0.03f);
            }
            else {
                yield return new WaitForSecondsRealtime(flexible_speed_of_typing ?
                    (1.3f / sentence.Length) : 0.03f);
            }
        }
        StopCoroutine(typing);
        isTyping = false;



        if (mouseTrigger != null)
        { mouseTrigger.tmpPosition = Input.mousePosition; }
        pointer++;
    }


    IEnumerator openPanel(float speed) {

        rec.anchoredPosition = new Vector2(0f, -rec.rect.height / 2f);
        float timer = 0f;
        while (rec.anchoredPosition.y <= rec.rect.height / 2f)
        {
            timer += Time.deltaTime;
            if (Time.timeScale == 0)
            {
                rec.anchoredPosition = new Vector2(0f, rec.anchoredPosition.y + Time.deltaTime * speed);
            }
            else
            {
                rec.anchoredPosition = new Vector2(0f, rec.anchoredPosition.y + Time.unscaledDeltaTime * speed);
            }
            yield return null;
        }
        Debug.Log(timer);

    }
    IEnumerator closePanel( float speed)
    {
        rec.anchoredPosition = new Vector2(0f, rec.rect.height / 2f);


        while(rec.anchoredPosition.y >= -rec.rect.height / 2f)
        {
            if (Time.timeScale == 0)
            {
                rec.anchoredPosition = new Vector2(0f, rec.anchoredPosition.y - Time.deltaTime * speed);
            }
            else {
                rec.anchoredPosition = new Vector2(0f, rec.anchoredPosition.y - Time.unscaledDeltaTime * speed);
            }
            yield return null;
        }
    }
}
