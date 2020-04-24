using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionState : MonoBehaviour
{

    public bool hasBeenInMainMenu = false;


    public static SessionState instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;          
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            hasBeenInMainMenu = true;
        }
    }
}
