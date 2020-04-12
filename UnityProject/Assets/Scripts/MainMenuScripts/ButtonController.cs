using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class ButtonController : MonoBehaviour
{
    
    public PlayableDirector fader;
    // Start is called before the first frame update

    [SerializeField]
    private GameObject current;

    [SerializeField]
    private GameObject eventSystem;


    void Start()
    {
        current = this.transform.GetChild(0).gameObject;
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(current);
        
        #region startBuild
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (eventSystem.GetComponent<EventSystem>().currentSelectedGameObject == null) {
            eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(current);
        }


        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape)) {
            ComeBack();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            #region butIndexPlus
            current.GetComponent<MenuLevel>().buttonIndex = 
                (current.GetComponent<MenuLevel>().buttonIndex + 1) % current.transform.childCount;
            #endregion
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            #region butIndexMinus
            current.GetComponent<MenuLevel>().buttonIndex = 
                (current.GetComponent<MenuLevel>().buttonIndex - 1 + current.transform.childCount) % current.transform.childCount;
            #endregion
        }
    }

    public void OpenPanel(GameObject obj) {
        current.SetActive(false);
        current = obj;
        current.SetActive(true);
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(current.transform.
            GetChild(current.GetComponent<MenuLevel>().buttonIndex).gameObject);
    }

    public void ComeBack() {
        if (current.GetComponent<MenuLevel>().parent != null)
        {
            current.GetComponent<MenuLevel>().buttonIndex = 0;
            OpenPanel(current.GetComponent<MenuLevel>().parent);
        }
    } 

    public void QuitGame() {
        Application.Quit();
    }

    public void Play() {
        fader.Play();
        StartCoroutine(Wait(3f));
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
