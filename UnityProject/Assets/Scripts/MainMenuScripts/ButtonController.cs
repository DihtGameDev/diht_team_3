using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;

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
    private EventSystem eventSystem;


    //SETTINGS
    public InterFaceController.CurrentKey currentKey = new InterFaceController.CurrentKey();

    public Slider eff_volume, mus_volume, inter_volume;
    //Resolution
    public Dropdown resolutions;

    
    [HideInInspector]
    public List<InterFaceController.Resolution> resolutionsList;


    public Texture2D menuCursor;
    public Texture2D gameCursor;
    void Start()
    {
      
        current = this.transform.GetChild(0).gameObject;


        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").gameObject.GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(current);

        starter = GameObject.Find("Starter").gameObject.GetComponent<PlayableDirector>();
        fader = GameObject.Find("Fader").gameObject.GetComponent<PlayableDirector>();
        blueMoonDirector = GameObject.Find("Logo").gameObject.GetComponent<PlayableDirector>();

        #region startBuild
        Cursor.visible = false;
        //Cursor.SetCursor(menuCursor, new Vector2(0f, 0f), CursorMode.Auto);
        //Activation
        StartCoroutine(activate(5f));

        audioController = Global.audioController;
        StartCoroutine(audioController.Play("BackGround", 5f));
        StartCoroutine(audioController.ChangeVolume("BackGround", audioController.GetClipRelevantVolume("BackGround")));
        StartCoroutine(audioController.Play("MenuNeon", 0f, 9.95f));
        StartCoroutine(audioController.ChangeVolume("MenuNeon", audioController.GetClipRelevantVolume("MenuNeon")));


        starter.Play();


        //SETTINGS
        eff_volume.GetComponent<Slider>().value = Global.effects_volume;
        mus_volume.GetComponent<Slider>().value = Global.music_volume;
        inter_volume.GetComponent<Slider>().value = Global.interface_volume;

        eff_volume.gameObject.SetActive(false);
        mus_volume.gameObject.SetActive(false);
        inter_volume.gameObject.SetActive(false);

        //Resolution

        resolutionsList = InterFaceController.resolutionsList;

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
        foreach (var res in resolutionsList)
        {
            options.Add(res.width.ToString() + " x " + res.height.ToString());
        }
        resolutions.GetComponent<Dropdown>().AddOptions(options);
        
        resolutions.gameObject.SetActive(false);





        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRestricted)
        {
            if (Input.GetKey(KeyCode.Return) && currentKey.meaning != null && current.name != "VideoMenu")
            {
                currentKey.meaning.SetActive(false);
                eventSystem.SetSelectedGameObject(currentKey.description);
                currentKey.meaning = null;
            }

            if (eventSystem.currentSelectedGameObject == null)
            {
                eventSystem.SetSelectedGameObject(current);
            }

            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) ||
                Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)) && currentKey.meaning == null)
            {
                StartCoroutine(audioController.Play("SelectButton"));

            }

            if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(audioController.Play("SelectButton"));

                if (currentKey.meaning != null)
                {
                    currentKey.meaning.SetActive(false);
                    eventSystem.SetSelectedGameObject(currentKey.description);
                    currentKey.meaning = null;
                }
                else { 
                    ComeBack();
                }

            }

            if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && currentKey.meaning == null)
            {
                StartCoroutine(audioController.Play("SelectButton"));
                #region butIndexPlus
                current.GetComponent<MenuLevel>().buttonIndex =
                    (current.GetComponent<MenuLevel>().buttonIndex + 1) % current.transform.childCount;
                #endregion
            }

            if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && currentKey.meaning == null)
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

            if (obj.name != "AudioMenu" && obj.name != "VideoMenu")
            {
                eventSystem.SetSelectedGameObject(current.transform.
                            GetChild(current.GetComponent<MenuLevel>().buttonIndex).gameObject);
            }
            else {
                eventSystem.SetSelectedGameObject(current.transform.
                           GetChild(current.GetComponent<MenuLevel>().buttonIndex).transform.GetChild(1).gameObject);
            }
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
        Cursor.visible = false;
        Cursor.SetCursor(gameCursor, new Vector2(0f, 0f), CursorMode.Auto);
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
        Cursor.visible = false;
    }

    //COSTILES from InterfaceController
    public void changeControl(GameObject clicked)
    {
        currentKey.description = eventSystem.currentSelectedGameObject;
        clicked.SetActive(true);
        eventSystem.SetSelectedGameObject(clicked);
        StartCoroutine(costile(clicked));
    }
    IEnumerator costile(GameObject clicked)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        currentKey.meaning = clicked;
    }

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
    public void changeResolution(Int32 index)
    {
        Screen.SetResolution(resolutionsList[index].width, resolutionsList[index].height, true);
        Global.current_resolution = new InterFaceController.Resolution(resolutionsList[index].width, resolutionsList[index].height);

    }
}
