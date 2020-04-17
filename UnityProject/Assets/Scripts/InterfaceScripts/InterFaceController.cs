using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class InterFaceController : MonoBehaviour
{

    private GameObject marshall;
    private MarshallController marshallController;

    [SerializeField]


    private GameObject healthBar;

    private GameObject pauseMenu;

    private GameObject loseMenu;

    
    public Image[] hearts;

    public Sprite fullHeart;
    public Sprite halfHeart;

    GameObject eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        marshall = GameObject.FindGameObjectWithTag("Marshall").gameObject;
        marshallController = marshall.GetComponent<MarshallController>();

        healthBar = transform.Find("HealthBar").gameObject;

        pauseMenu = transform.Find("PauseMenu").gameObject;

        loseMenu = transform.Find("LoseMenu").gameObject;

        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").gameObject;

        pauseMenu.SetActive(false);
        loseMenu.SetActive(false);

        Cursor.visible = true;

      

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < (marshallController.health + 1) / 2) {
                hearts[i].enabled = true;
            }
            else {
                hearts[i].enabled = false;
            }

            if (marshallController.health % 2 == 1) {
                    hearts[marshallController.health / 2].sprite = halfHeart;
            }
            else { hearts[(marshallController.health - 1) / 2].sprite = fullHeart; }

        }
       
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
