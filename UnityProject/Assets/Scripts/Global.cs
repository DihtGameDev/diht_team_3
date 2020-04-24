using UnityEngine;

public class Global 
{


    // Start is called before the first frame update
    public static float timeOfUploadingRoom = 0.2f;
    public static float timeOfUploadingLevel = 0.1f;
    public static float timeOfOutingLevel = 0.1f;

    public static KeyCode moveUp = KeyCode.W;
    public static KeyCode moveDown = KeyCode.S;
    public static KeyCode moveLeft = KeyCode.A;
    public static KeyCode moveRight = KeyCode.D;
    public static KeyCode action = KeyCode.Space;
    public static KeyCode sitDown = KeyCode.LeftShift;

    public static float camera_sensitivity = 0.7f;

    public static float music_volume = 0.5f;
    public static float effects_volume = 0.5f;
    public static float interface_volume = 0.5f;

    public static InterFaceController.Resolution current_resolution = 
        new InterFaceController.Resolution(Screen.width, Screen.height);

    public static AudioController audioController;

}
