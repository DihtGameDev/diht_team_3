using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.Audio;

public class InterFaceController : MonoBehaviour
{

    private GameObject marshall;
    private MarshallController marshallController;

    [SerializeField]
    private GameObject healthBar;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject loseMenu;
    [SerializeField]
    private GameObject selectMenu;

    [SerializeField]
    private Stack<GameObject> panels = new Stack<GameObject>();

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;

    [SerializeField]
    EventSystem eventSystem;

    [SerializeField]
    private AudioController audioController;

    //Changing control
    [System.Serializable]
    public class CurrentKey { 
        public GameObject description;
        public GameObject meaning;
    }
    public CurrentKey currentKey = new CurrentKey();


    //Control
    public Text right, left, up, down, sit, action;
    //Camera sensitivity and audio volume
    public Slider camera_sensitivity, eff_volume, mus_volume, inter_volume;
    //Resolution
    public Dropdown resolutions;
    public class Resolution {
        public int width;
        public int height;
        public Resolution(int w, int h) {
            width = w;
            height = h;
        }
    }
    public List<Resolution> resolutionsList = new List<Resolution>() { Global.current_resolution, new Resolution(1920, 1080), new Resolution(1600, 900),
               new Resolution(1366, 768), new Resolution(1280, 760),new Resolution(1280, 720), new Resolution(1024, 768),new Resolution(800, 480)};


    bool isShift = false; // costile as shift ain't being detected by Event.isKey

    //Typing, to chane words in Instructions
    [SerializeField]
    DialogeController dController;
    [SerializeField]
    Dialoge dialoge;

    // Start is called before the first frame update
    
    void Awake()
    {
        dController = FindObjectOfType<DialogeController>();
        dialoge = FindObjectOfType<Dialoge>();

        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").gameObject.GetComponent<EventSystem>();

        audioController = GameObject.Find("AudioManager").gameObject.GetComponent<AudioController>();


        healthBar = transform.Find("HealthBar").gameObject;

        pauseMenu = transform.Find("PauseMenu").gameObject;

        loseMenu = transform.Find("LoseMenu").gameObject;

        selectMenu = transform.Find("SelectLevel").gameObject;

       
        pauseMenu.SetActive(false);
        loseMenu.SetActive(false);
        selectMenu.SetActive(false);

        marshall = GameObject.FindGameObjectWithTag("Marshall").gameObject;
        marshallController = marshall.GetComponent<MarshallController>();

    }

    void Start() {

        right.text = Global.moveRight.ToString();
        left.text = Global.moveLeft.ToString();
        up.text = Global.moveUp.ToString();
        down.text = Global.moveDown.ToString();
        sit.text = Global.sitDown.ToString();
        action.text = Global.action.ToString();
        camera_sensitivity.GetComponent<Slider>().value = Global.camera_sensitivity;
        eff_volume.GetComponent<Slider>().value = Global.effects_volume;
        mus_volume.GetComponent<Slider>().value = Global.music_volume;
        inter_volume.GetComponent<Slider>().value = Global.interface_volume;


        //Resolution
        for (int i = 1; i < resolutionsList.Count; i++)
        {
            if (Global.current_resolution.width == resolutionsList[i].width &&
                Global.current_resolution.height == resolutionsList[i].height)
            {
                resolutionsList.Remove(resolutionsList[i]);
                break;
            }
        }
        List<string> options = new List<string>();
        foreach (var res in resolutionsList) {
            options.Add(res.width.ToString() + " x " + res.height.ToString());
        }
        resolutions.GetComponent<Dropdown>().AddOptions(options);
       



    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Return) && currentKey.meaning != null && panels.Peek().name != "VideoMenu") {
            eventSystem.SetSelectedGameObject(currentKey.description);
            currentKey.meaning = null;
        }

        //HEALTH
        if (healthBar.activeSelf)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < (marshallController.health + 1) / 2)
                {
                    hearts[i].enabled = true;
                }
                else
                {
                    hearts[i].enabled = false;
                }

                if (marshallController.health % 2 == 1)
                {
                    hearts[marshallController.health / 2].sprite = halfHeart;
                }
                else { hearts[(marshallController.health - 1) / 2].sprite = fullHeart; }
            }
        }

      
        // AUDIO
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape)) && panels.Count != 0) {
            StartCoroutine(audioController.Play("SelectButtonGameMenu"));
        }
        // Pause

        if (Input.GetKeyDown(KeyCode.Escape)) {

            if (panels.Count == 1 && !loseMenu.activeSelf)
            {
                BackToGame();
            }
            else if (panels.Count > 1 && currentKey.meaning == null)
            {
                ComeBack();
            }
            else if (panels.Count > 1 && currentKey.meaning != null) {
               
                eventSystem.SetSelectedGameObject(currentKey.description);
                currentKey.meaning = null;
            }
            else if (panels.Count == 0)
            {
                StartCoroutine(StartPanel(pauseMenu, 0f));
                
            }
          
        }

        // Lose
        if (marshallController.isCaptured == true) {
            marshallController.isCaptured = false;
            StartCoroutine(StartPanel(loseMenu, 0.2f));
            StartCoroutine(marshallController.glitch(0.4f));
        }
    }

   
    public void TryAgain()
    {
        panels.Pop().SetActive(false);
        panels.Clear();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        //AUDIO
        audioController.StopChasingSound(1f);
        pauseSound(1f);
    }
    public void BackToGame()
    {
        panels.Pop().SetActive(false);
        panels.Clear();
        Time.timeScale = 1f;

        //AUDIO
        pauseSound(1f);
    }

    IEnumerator StartPanel(GameObject panel, float offset) {
        yield return  new WaitForSecondsRealtime(offset);
        OpenPanel(panel);

        //AUDIO
        pauseSound(0.2f);
    }

    public void OpenPanel(GameObject obj)
    {
        if (panels.Count > 0)
        {
            panels.Peek().SetActive(false);

        }
        panels.Push(obj);
        panels.Peek().SetActive(true);
        eventSystem.SetSelectedGameObject(panels.Peek().transform.GetChild(0).gameObject);
        Time.timeScale = 0f;
    }

     
    public void LoadLevel(int buildIndex)
    {
        panels.Peek().SetActive(false);
        panels.Clear();
        Time.timeScale = 1f;

        StartCoroutine(audioController.turnOffSound(buildIndex));

        SceneManager.LoadScene(buildIndex);
        //AUDIO
        audioController.StopChasingSound(1f);
        pauseSound(1f);
    }


    public void ComeBack()
    {

        if (panels.Count > 1)
        {
            panels.Pop().SetActive(false);

        }
        panels.Peek().SetActive(true);
        eventSystem.SetSelectedGameObject(panels.Peek().transform.GetChild(0).gameObject);
    }

    //CONTROL
    public void changeControl(GameObject clicked)
    {
        currentKey.description = eventSystem.currentSelectedGameObject;
        eventSystem.SetSelectedGameObject(clicked);
        StartCoroutine(costile(clicked));
    }
    IEnumerator costile(GameObject clicked) {
        yield return new WaitForSecondsRealtime(0.1f);    
        currentKey.meaning = clicked;
    }

    public void changeSensitivity()
    {
        Global.camera_sensitivity = camera_sensitivity.GetComponent<Slider>().value;

    }
    //AUDIO
    public void changeEffectsVolume()
    {
        audioController.changeVolumeSettings("effects", eff_volume.GetComponent<Slider>().value);
        Global.effects_volume = eff_volume.GetComponent<Slider>().value;
        if (Time.timeSinceLevelLoad > 2f)
        {
            StartCoroutine(audioController.Play("Alarm"));
        }
    }

    public void changeMusicVolume()
    {
        audioController.changeVolumeSettings("music", mus_volume.GetComponent<Slider>().value);
        Global.music_volume = mus_volume.GetComponent<Slider>().value;
    }

    public void changeInterfaceVolume()
    {
        audioController.changeVolumeSettings("interface", inter_volume.GetComponent<Slider>().value);
        Global.interface_volume = inter_volume.GetComponent<Slider>().value;
        if (Time.timeSinceLevelLoad > 2f)
        {
            StartCoroutine(audioController.Play("Typing"));
        }
    }

    //RESOLUTION
    public void changeResolution(Int32 index) {
        Screen.SetResolution(resolutionsList[index].width, resolutionsList[index].height, true);
        Global.current_resolution = new InterFaceController.Resolution(resolutionsList[index].width, resolutionsList[index].height);

    } 
    private void OnGUI()
    {

        Event e = Event.current;
        if (currentKey.meaning != null) {

            if ((isShift || e.isKey) && !(e.keyCode == KeyCode.Return) && !(e.keyCode == KeyCode.Tab) &&
                !(e.keyCode == KeyCode.Escape) &&!(e.keyCode == KeyCode.Backspace)) {
               
                KeyCode code = KeyCode.None;
                if (e.isKey && !(e.keyCode == KeyCode.Return))
                {
                    currentKey.meaning.transform.FindChild("Text").GetComponent<Text>().text = e.keyCode.ToString();
                    code = e.keyCode;
                }
                else if (isShift) {
                    currentKey.meaning.transform.FindChild("Text").GetComponent<Text>().text = KeyCode.LeftShift.ToString();
                    code = KeyCode.LeftShift;
                }


                switch (Int32.Parse(currentKey.meaning.gameObject.name)) {
                    case 1:
                        Global.moveRight = code;
                        break;
                    case 2:
                        Global.moveLeft = code;
                        break;
                    case 3:
                        Global.moveUp = code;
                        break;
                    case 4:
                        Global.moveDown = code;
                        break;
                    case 5:
                        Global.sitDown = code;
                        break;
                    case 6:
                        Global.action = code;
                        break;
                }
                    
                isShift = false;
                eventSystem.SetSelectedGameObject(currentKey.description);

                currentKey.meaning = null;
            }
            
            if (e.keyCode == KeyCode.Tab || e.keyCode == KeyCode.Backspace) {
                eventSystem.SetSelectedGameObject(currentKey.description);
                currentKey.meaning = null;
            }

        }

    }

    void pauseSound(float localVolume) {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            if (marshallController.isChasable)
            {
                StartCoroutine(audioController.ChangeVolume("Tension", audioController.GetClipRelevantVolume("Tension") * localVolume));

            }
            else { 
                StartCoroutine(audioController.ChangeVolume("HospitalTrack", audioController.GetClipRelevantVolume("HospitalTrack") * localVolume));
            
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            StartCoroutine(audioController.ChangeVolume("RoomAnxiety", audioController.GetClipRelevantVolume("RoomAnxiety") * localVolume));
        }
    }
}
