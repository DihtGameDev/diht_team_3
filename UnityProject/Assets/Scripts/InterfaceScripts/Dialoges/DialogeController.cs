using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogeController : MonoBehaviour
{
    public Text nameText;
    public Text dialogeText;

    public Animator animator;
    public int pointer = 0;

    [SerializeField]
    private Queue<string> sentences = new Queue<string>();

    public DialogeTriggerHospital1 mouseTrigger;
    // Start is called before the first frame update
    void Start()
    {
        mouseTrigger = FindObjectOfType<DialogeTriggerHospital1>();
    }

    void Update() {
        animator.speed = 1f / Time.timeScale;
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
        foreach (char letter in sentence.ToCharArray()) {
            dialogeText.text += letter;
            yield return new WaitForSecondsRealtime(0.8f / sentence.Length);
        }
        pointer++;
        if (mouseTrigger != null)
        { mouseTrigger.tmpPosition = Input.mousePosition; }
    }
}
