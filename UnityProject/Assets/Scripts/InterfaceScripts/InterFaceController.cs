using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;

public class InterFaceController : MonoBehaviour
{

    private GameObject marshall;

    [SerializeField]
    private int health;

    private GameObject healthBar;

    private GameObject pauseMenu;

    private GameObject loseMenu;

    [SerializeField]
    private List<GameObject> hearts;

    GameObject eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        marshall = GameObject.FindGameObjectWithTag("Marshall").gameObject;

        healthBar = transform.Find("HealthBar").gameObject;

        pauseMenu = transform.Find("PauseMenu").gameObject;

        loseMenu = transform.Find("LoseMenu").gameObject;

        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").gameObject;

        pauseMenu.SetActive(false);
        loseMenu.SetActive(false);

        Cursor.visible = true;

        health = healthBar.transform.childCount;

        for (int i = 0; i < healthBar.transform.childCount; i++) {
            hearts.Add(healthBar.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        health = healthBar.transform.childCount;

        if (Input.GetKeyDown(KeyCode.Escape) && !loseMenu.activeSelf) {
            if (pauseMenu.activeSelf)
            {               
                BackToGame();
            }
            else {               
                Pause();
            }            
        }

        if (marshall.GetComponent<MarshallController>().isCaptured == true) {
            marshall.GetComponent<MarshallController>().isCaptured = false;
            Lose();
        }
    }

    public void GoToMainMenu()
    {
        pauseMenu.SetActive(false);
        loseMenu.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Lose()
    {
        loseMenu.SetActive(true);
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(loseMenu.transform.
            Find("LoseMenuPanel").Find("TryAgain").gameObject);
        Time.timeScale = 0f;
    }

    public void TryAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {   
        pauseMenu.SetActive(true);
  
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(pauseMenu.transform.Find("PauseMenuPanel").gameObject);
        Time.timeScale = 0f;
    }

    public void BackToGame()
    {
        Time.timeScale = 1f;

        pauseMenu.SetActive(false);
    }
}
