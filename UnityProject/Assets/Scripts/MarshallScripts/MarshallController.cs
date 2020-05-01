using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Audio;

public class MarshallController : MonoBehaviour
{
    public bool isMoving = false;
    [Range(0, 6)]
    public int health;

    public bool isScript;

    public bool isRestricted = false;

    public float marshal_height;
    // Interaction With Enemies
    public bool isCaptured;
    public bool isChasable;

    public int number_of_rushers;

    //Moving
    [SerializeField]
    Vector2 direction;

    private float rightAxis, leftAxis, upAxis, downAxis;

    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float scriptMomentSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crawlSpeed;
    [SerializeField]
    private float fastCrawlSpeed;
    [SerializeField]
    private float speed;

    //Sitting    
    public bool isSitting;

    //Control
    #region ControllButtons

    KeyCode moveRight = KeyCode.D;
    KeyCode moveLeft = KeyCode.A;
    KeyCode moveDown = KeyCode.S;
    KeyCode moveUp = KeyCode.W;
    KeyCode sit = KeyCode.LeftShift;
    #endregion

    //Colliders
    static private BoxCollider2D collider;


    //Shining by Lighter

    public bool isShined;

    public int shinersCounter;

    //Animation
    private Animator anim;

    private Animator interFaceAnim;

    private SpriteRenderer sprite;

    //
    Rigidbody2D rb;
    private GameObject pointer;

    //AUDIO
    private AudioController audioController;

    IEnumerator walkSound;
    private bool change_audio = false;
    private bool change_audio_volume = false;

    //LIGHTNING
    private GameObject needToBeLighted;

    //CAMERA
    [SerializeField]
    private CameraScript cameraController;
    // Start is called before the first frame update

    private void Awake()
    {
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();

        collider = transform.GetComponent<BoxCollider2D>();
        anim = transform.GetComponent<Animator>();

        interFaceAnim = GameObject.Find("Interface").GetComponent<Animator>();

        rb = transform.GetComponent<Rigidbody2D>();


        audioController = GameObject.Find("AudioManager").gameObject.GetComponent<AudioController>();
        walkSound = audioController.PlayWithPeriod("MarshallStep", 0.28f, true);
        needToBeLighted = GameObject.Find("NeedToBeLighted").gameObject;

        cameraController = GameObject.FindObjectOfType<CameraScript>();

    }

    private void Start()
    {
        health = 6;


        //Default VARIABLES

        direction = Vector2.zero;

        isCaptured = false;
        isChasable = false;
        number_of_rushers = 0;
        shinersCounter = 0;

        isSitting = false;
        isShined = false;

        rightAxis = 0f;
        leftAxis = 0f;
        upAxis = 0f;
        downAxis = 0f;

        //CHANGABLE VARIABLES
        marshal_height = 0f;

        scriptMomentSpeed = 0.8f;
        walkSpeed = 1.75f;
        runSpeed = 2.25f;
        crawlSpeed = 1.2f;
        fastCrawlSpeed = 1.6f;
        speed = walkSpeed;


        pointer = GameObject.FindGameObjectWithTag("Pointer");
        if (SceneManager.GetActiveScene().name == "Room") {
            this.gameObject.SetActive(false);
        }        
      

    }
    // Update is called once per frame
    void FixedUpdate()
    {

        //sprite.material.SetFloat("_Fade", Mathf.PingPong(Time.time, 1f));
        // LIGHTERS
        if (shinersCounter == 0)
        {
            isShined = false;
        }
        else
        {
            isShined = true;
        }

        //ENEMIES
        if (number_of_rushers == 0)
        {
            if (isChasable)
            {
                change_audio = true;
                change_audio_volume = true;
                audioController.StopChasingSound();
                isChasable = false;
            }
        }
        else
        {
            if (!isChasable)
            {
                change_audio = true;
                change_audio_volume = true;
                audioController.StartChasingSound();
                StartCoroutine(cameraController.Shake(0.3f, 0.12f, 0.1f));

                isChasable = true;
            }
        }

        #region moving

        rb.velocity = Vector2.zero;

        if (isChasable)
        {
            speed = (isSitting) ? fastCrawlSpeed : runSpeed;
        }
        else
        {
            speed = (isSitting) ? crawlSpeed : walkSpeed;
        }

        if (isScript)
        {
            speed = scriptMomentSpeed;
        }

        if (Input.GetKey(Global.moveDown) || Input.GetKey(Global.moveUp) ||
            Input.GetKey(Global.moveLeft) || Input.GetKey(Global.moveRight))
        {
            if (!isMoving) {
                change_audio = true;
                isMoving = true;
            }
        }
        else {
            if (isMoving)
            {
                change_audio = true;
                isMoving = false;
            }
        }

        if (Input.GetKey(Global.moveRight))
        {
            rightAxis = 1f;
        }
        else { rightAxis = 0f; }

        if (Input.GetKey(Global.moveLeft))
        {
            leftAxis = -1f;
        }
        else { leftAxis = 0f; }

        if (Input.GetKey(Global.moveUp) && !isScript)
        {
            upAxis = 1f;
        }
        else { upAxis = 0f; }

        if (Input.GetKey(Global.moveDown) && !isScript)
        {
            downAxis = -1f;
        }
        else { downAxis = 0f; }

        direction = new Vector2(rightAxis + leftAxis, upAxis + downAxis);
        if (!isRestricted)
        {
            move(direction);
        }
        #endregion


        //ANIMATION
        if (!isRestricted && Time.timeScale != 0)
        {
            anim.speed = speed / walkSpeed;
        }
        else {
            anim.speed = 0f;
        }

        if (direction == new Vector2(0f, 0f))
        {
            anim.SetBool("isMoving", false);

        }
        else { 
            anim.SetBool("isMoving", true);

        }

        if (rightAxis + leftAxis > 0f)
        {
            sprite.flipX = false;
        }
        else if (rightAxis + leftAxis < 0f) { sprite.flipX = true; }

        if (direction.y == -1f && direction.x == 0f)
        {
            anim.SetBool("isUp", false);
            anim.SetBool("isDown", true);
        }
        if (direction.y == 1f && direction.x == 0f)
        {
            anim.SetBool("isDown", false);
            anim.SetBool("isUp", true);
        }
        if (direction.x != 0f) {
            anim.SetBool("isDown", false);
            anim.SetBool("isUp", false);
        }

        //AUDIO
        if (change_audio) {
            change_audio = false;
            if (isMoving)
            {
                StopCoroutine(walkSound);
                walkSound = audioController.PlayWithPeriod("MarshallStep", 0.28f * walkSpeed/ speed, true);
                StartCoroutine(walkSound);
            }
            else {

                StopCoroutine(walkSound);
            }
        }

        if (change_audio_volume) {
            change_audio_volume = false;
            
            if (isSitting)
            {
                StartCoroutine(audioController.ChangeVolume("MarshallStep", isChasable ? audioController.GetClipRelevantVolume("MarshallStep") * 0.7f : audioController.GetClipRelevantVolume("MarshallStep") * 0.5f));
            }
            else {
                StartCoroutine(audioController.ChangeVolume("MarshallStep", isChasable ? audioController.GetClipRelevantVolume("MarshallStep") * 1.4f :
                    audioController.GetClipRelevantVolume("MarshallStep")));
            }
        }

        //SITTING
        if (Input.GetKey(Global.sitDown) && !isScript && Time.timeScale != 0 && !isRestricted)
        {
            if (!isSitting) {
                change_audio_volume = true;
                change_audio = true;
            }
            isSitting = true;

            anim.SetBool("isSitting", true);
        }
        else
        {
            if (isSitting)
            {
                change_audio_volume = true;
                change_audio = true;
            }
            isSitting = false;
            anim.SetBool("isSitting", false);
        }
    }

    void move(Vector3 direction)
    {
        transform.position = Vector2.MoveTowards(transform.position,
            transform.position + direction, speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Link"))
        {
            if (pointer != null)
            {
                Destroy(pointer.gameObject);
            }
            if (SceneManager.GetActiveScene().name == "Room") {
                StartCoroutine(audioController.Stop("BackGround", findAnimationClip(interFaceAnim.runtimeAnimatorController.animationClips, "FadeAway").length));
                StartCoroutine(audioController.Stop("NeonLamp", findAnimationClip(interFaceAnim.runtimeAnimatorController.animationClips, "FadeAway").length));
            }
            if (interFaceAnim != null)
            {
                interFaceAnim.SetBool("isEnding", true);
            }
            audioController.StopChasingSound();
            StartCoroutine(Wait(findAnimationClip(interFaceAnim.runtimeAnimatorController.animationClips, "FadeAway").length));
            transform.position = new Vector2(transform.position.x + 10f, transform.position.y);
        }
    }

    IEnumerator Wait(float time)
    {
        yield return  new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public IEnumerator glitch(float time)
    {
        StartCoroutine(audioController.Play("Alarm"));
        sprite.material.SetVector("_Glitch", new Vector4(0f, 0.3f, 0f, 0f));
        sprite.material.SetFloat("_GlitchOffset", 0.18f);
        yield return new WaitForSeconds(time);
        sprite.material.SetFloat("_GlitchOffset", -0.05f);
        sprite.material.SetVector("_Glitch", new Vector4(0f, 0f, 0f, 0f));
        sprite.material.SetFloat("_GlitchOffset", 0.15f);
    }

    private AnimationClip findAnimationClip(AnimationClip[] array, string findName)
    {
        foreach (var obj in array)
        {
            if (obj.name == findName)
            {
                return obj;
            }
        }
        return null;
    }
}
