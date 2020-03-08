using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    //playMenu, 1
    //settingsMenu, 2
    //exitMenu, 3
    //newGameMenu, 4
    //savingsMenu, 5
    //audioMenu, 6

    //Panels
    public GameObject startMenu;
    public GameObject playMenu;
    public GameObject settingsMenu;
    public GameObject exitMenu;

    //MenuLevels
    public MenuLevel start_menu;
    public MenuLevel play_menu;
    public MenuLevel settings_menu;
    public MenuLevel exit_menu;

    static public MenuLevels controller;

    //Buttoms
    public GameObject play;
    public GameObject settings;
    public GameObject exit;
    public GameObject newGame;
    public GameObject savings;
    public GameObject audio;
    public GameObject video;
    public GameObject yes;
    public GameObject no;


    public class Buttom {
        public Buttom(GameObject Butt, MenuLevel LoadLevel = null) {
            buttom = Butt;
            loadLevel = LoadLevel;
        }
        
        public GameObject buttom;
        public MenuLevel loadLevel;
    
    }

    public class MenuLevel {
        public GameObject mySelf;
        public int myIndex;

        public List<Buttom> buttoms;
        public MenuLevel parent;

        public int buttomPointer;
        
        public MenuLevel(GameObject MySelf, int MyIndex, List<Buttom> Buttoms, MenuLevel Parent = null) {
            mySelf = MySelf;
            buttoms = Buttoms;
            parent = Parent;
            buttomPointer = 0;
            myIndex = MyIndex;
        }

        public void GoRight() {

            buttomPointer = (++buttomPointer) % buttoms.Count;
        }

        public void GoLeft()
        {

            buttomPointer = (--buttomPointer + buttoms.Count) % buttoms.Count;
        }

        public void GoIn()
        {
            if (buttoms[buttomPointer].loadLevel != null) {
                mySelf.SetActive(false);
                buttoms[buttomPointer].loadLevel.mySelf.SetActive(true);

                controller.levelPointer = buttoms[buttomPointer].loadLevel.myIndex; //2
            }
        }

        public void GoOut()
        {
            if (parent != null) {

                buttomPointer = 0; // 2
                mySelf.SetActive(false);
                parent.mySelf.SetActive(true);
                controller.levelPointer = parent.myIndex; //3

            }
            
        }
    }

    public class MenuLevels {
        public List<MenuLevel> levels;
        public int levelPointer;

        public MenuLevels(List<MenuLevel> Levels, int LevelPointer = 0) {
            levels = Levels;
            levelPointer = LevelPointer;
        }
    }


    void Start() {
        for (int i = 0; i < 2; i++)
        {
            start_menu = new MenuLevel(startMenu, 0, new List<Buttom> { new Buttom(play, play_menu),
            new Buttom(settings, settings_menu), new Buttom(exit, exit_menu) }, null);
            play_menu = new MenuLevel(playMenu, 1, new List<Buttom> { new Buttom(newGame, null),
            new Buttom(savings, null) }, start_menu);
            settings_menu = new MenuLevel(settingsMenu, 2, new List<Buttom> { new Buttom(audio, null),
            new Buttom(video, null) }, start_menu);
            exit_menu = new MenuLevel(exitMenu, 3, new List<Buttom> { new Buttom(yes, null),
            new Buttom(no, start_menu) }, start_menu);
        }

        controller = new MenuLevels(new List<MenuLevel> { start_menu, play_menu, settings_menu, exit_menu});
    }

    void Update()
    {
        //SUGAR STAFF
        MenuLevel cur_level = controller.levels[controller.levelPointer];

        //ANIMATION
        Scaling(cur_level.buttomPointer, 1.2f);
        for (int i = 0; i < cur_level.buttoms.Count; i++) {
            if (i != cur_level.buttomPointer) {
                Scaling(i, 1f);
            }
        }
        

        //INPUT
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace)) {
            cur_level.GoOut();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            cur_level.GoIn();
            if (cur_level.buttoms[cur_level.buttomPointer].buttom == yes) {
                UnityEditor.EditorApplication.isPlaying = false;
                Application.Quit();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            cur_level.GoLeft();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            cur_level.GoRight();

        }

    }

    
    public void Scaling(int index, float limit, float speed = 15f) {
        Transform current_buttom = controller.levels[controller.levelPointer].buttoms[index].buttom.transform;
        float scaling = Mathf.Lerp(current_buttom.localScale.x, limit, speed * Time.deltaTime);
        current_buttom.localScale = new Vector3(scaling, scaling, scaling);
    }
}
