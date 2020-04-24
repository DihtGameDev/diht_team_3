using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class ButtonController : MonoBehaviour
{
    private PlayableDirector starter;
    private PlayableDirector fader;
    private PlayableDirector blueMoonDirector;
    public bool isRestricted = true;

    // Start is called before the first frame update
    [SerializeField]
    private AudioController audioController;
  

    [SerializeField]
    private GameObject current;


    [SerializeField]
    private GameObject eventSystem;


  
    void Start()
    {
      

        current = this.transform.GetChild(0).gameObject;
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(current);

        starter = GameObject.Find("Starter").gameObject.GetComponent<PlayableDirector>();
        fader = GameObject.Find("Fader").gameObject.GetComponent<PlayableDirector>();
        blueMoonDirector = GameObject.Find("MaterialChange").gameObject.GetComponent<PlayableDirector>();

        #region startBuild
        Cursor.visible = false;

        //Activation
        StartCoroutine(activate(5f));

        audioController = Global.audioController;
        StartCoroutine(audioController.Play("BackGround", 5f));
        StartCoroutine(audioController.ChangeVolume("BackGround", audioController.GetClipRelevantVolume("BackGround")));
        StartCoroutine(audioController.Play("MenuNeon", 0f, 9.95f));
        StartCoroutine(audioController.ChangeVolume("MenuNeon", audioController.GetClipRelevantVolume("MenuNeon")));


        starter.Play();
       
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRestricted)
        {
            if (eventSystem.GetComponent<EventSystem>().currentSelectedGameObject == null)
            {
                eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(current);
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) ||
                Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                StartCoroutine(audioController.Play("SelectButton"));

            }

            if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(audioController.Play("SelectButton"));

                ComeBack();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(audioController.Play("SelectButton"));
                #region butIndexPlus
                current.GetComponent<MenuLevel>().buttonIndex =
                    (current.GetComponent<MenuLevel>().buttonIndex + 1) % current.transform.childCount;
                #endregion
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(audioController.Play("SelectButton"));
                #region butIndexMinus
                current.GetComponent<MenuLevel>().buttonIndex =
                    (current.GetComponent<MenuLevel>().buttonIndex - 1 + current.transform.childCount) % current.transform.childCount;
                #endregion
            }
        }
    }

    public void OpenPanel(GameObject obj) {
        if (!isRestricted) {
            current.SetActive(false);
            current = obj;
            current.SetActive(true);
            eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(current.transform.
                GetChild(current.GetComponent<MenuLevel>().buttonIndex).gameObject);
        }
    }

    public void ComeBack() {
        if (!isRestricted)
        {
            if (current.GetComponent<MenuLevel>().parent != null)
            {
                current.GetComponent<MenuLevel>().buttonIndex = 0;
                OpenPanel(current.GetComponent<MenuLevel>().parent);
            }
        }
    } 

    public void QuitGame() {
        if (!isRestricted)
        {
            Application.Quit();
        }
    }

    public void Play(int buildIndex) {
        if (!isRestricted)
        {
            if (current.gameObject.name == "PlayMenu")
            {
                StartCoroutine(audioController.Stop("BackGround", 5.5f, 1f));
                StartCoroutine(audioController.Stop("MenuNeon", 5.5f, 1));
            }
            else if (current.gameObject.name == "SelectLevelMenu") {
                StartCoroutine(audioController.Stop("BackGround", (float)fader.duration - 1.2f));
                StartCoroutine(audioController.Stop("MenuNeon", (float)fader.duration - 1.2f));
            }
            StartCoroutine(PlayGame(buildIndex));
        }
    }


    IEnumerator PlayGame(int buildIndex)
    {
        isRestricted = true;
        if (current.gameObject.name == "PlayMenu")
        {
            fader.Play();
            yield return new WaitForSecondsRealtime((float)fader.duration);

            blueMoonDirector.Play();
            yield return new WaitForSecondsRealtime((float)blueMoonDirector.duration);
        }
        else if (current.gameObject.name == "SelectLevelMenu")
        {
            current.gameObject.SetActive(false);
            fader.Play();
            yield return new WaitForSecondsRealtime((float)fader.duration - 1.2f);
        }
        StartCoroutine(audioController.turnOffSound(buildIndex));
        SceneManager.LoadScene(buildIndex);

    }

    IEnumerator activate(float offset) {
        yield return new WaitForSecondsRealtime(offset);
        isRestricted = false;
    }
}
