using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    // Start is called before the first frame update
    VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = this.GetComponent<VideoPlayer>();
    }
    void Start()
    {
        Camera.main.backgroundColor = Color.black;
        StartCoroutine(loadRoom());
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
    }

    IEnumerator loadRoom() {
        yield return new WaitForSeconds((float)videoPlayer.clip.length - 3.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
