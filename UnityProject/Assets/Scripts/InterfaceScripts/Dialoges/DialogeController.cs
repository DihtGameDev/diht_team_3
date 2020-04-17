using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogeController : MonoBehaviour
{

    public bool flexible_speed_of_typing;
    public int pointer = 0;
    private bool isTyping = false;
    private bool isStopTyping = false;

    public Text nameText;
    public Text dialogeText;

    public Animator animator;

    [SerializeField]
    private Queue<string> sentences = new Queue<string>();

    public DialogeTriggerHospital1 mouseTrigger;
    // Start is called before the first frame update
    void Start()
    {
        mouseTrigger = FindObjectOfType<DialogeTriggerHospital1>();
    }

    void Update()
    {
        animator.speed = 1f / Time.timeScale;
        if (Input.GetKeyDown(KeyCode.Space) && isTyping)
        {
            isStopTyping = true;
        }
    }

    public void StartDialoge(Dialoge dialoge) {

        Debug.Log("Start");
        nameText.text = dialoge.name;
        sentences.Clear();
        Debug.Log("DialogeName " + dialoge.name);
        foreach (string sentence in dialoge.sentences) {
            sentences.Enqueue(sentence);
            Debug.Log("Start" + sentence);
        }
    }

    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            EndDialoge();
            return;
        }

        string sentence = sentences.Dequeue();


        Debug.Log("Display" + sentence);
        StartCoroutine(TypeSentence(sentence));

        animator.SetBool("isOpen", true);

    }

    public void CloseSentence() {
        animator.SetBool("isOpen", false);
    }

    void EndDialoge() {

    }

    IEnumerator TypeSentence(string sentence) {
        
        yield return new WaitForSecondsRealtime(0.3f);
        
        dialogeText.text = "";
        
        isTyping = true;
        foreach (char letter in sentence.ToCharArray()) {
            if (isStopTyping) {
                isStopTyping = false;
                isTyping = false;
                dialogeText.text = sentence;
                break;
            }
            dialogeText.text += letter;
            yield return new WaitForSecondsRealtime(flexible_speed_of_typing ? 
                (1.3f / sentence.Length) : 0.05f);
        }

        isTyping = false;

        if (mouseTrigger != null)
        { mouseTrigger.tmpPosition = Input.mousePosition; }
        pointer++;
    }
}
