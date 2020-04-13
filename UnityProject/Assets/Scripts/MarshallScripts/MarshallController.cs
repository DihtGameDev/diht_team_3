using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class MarshallController : MonoBehaviour
{
    public bool isScript;

    public bool isRestricted = false;

    public float marshal_height;
    // Interaction With Enemies
    public bool isCaptured;
    public bool isChasable;

    public int number_of_rushers;

    //Moving
    [SerializeField]
    Vector2 diretion;

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

    private PlayableDirector fader;

    private Animator anim;

    private SpriteRenderer sprite;

    //
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        fader = GameObject.Find("Fader").GetComponent<PlayableDirector>();

        sprite = transform.FindChild("Sprite").GetComponent<SpriteRenderer>();

        //Default VARIABLES
        collider = transform.GetComponent<BoxCollider2D>();

        anim = transform.GetComponent<Animator>();
        rb = transform.GetComponent<Rigidbody2D>();

        diretion = Vector2.zero;

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
        walkSpeed = 2f;
        runSpeed = 2.5f;
        crawlSpeed = 1.2f;
        fastCrawlSpeed = 1.5f;
        speed = walkSpeed;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // LIGHTERS
        if (shinersCounter > 0)
        {
            isShined = true;
        }
        else
        {
            isShined = false;
        }

        //ENEMIES
        if (number_of_rushers == 0)
        {
            isChasable = false;
        }
        else
        {
            isChasable = true;
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

        if (Input.GetKey(moveRight))
        {
            rightAxis = 1f;
        }
        else { rightAxis = 0f; }

        if (Input.GetKey(moveLeft))
        {
            leftAxis = -1f;
        }
        else { leftAxis = 0f; }

        if (Input.GetKey(moveUp) && !isScript)
        {
            upAxis = 1f;
        }
        else { upAxis = 0f; }

        if (Input.GetKey(moveDown) && !isScript)
        {
            downAxis = -1f;
        }
        else { downAxis = 0f; }

        diretion = new Vector2(rightAxis + leftAxis, upAxis + downAxis);
        if (!isRestricted)
        {
            move(diretion);
        }
        #endregion

        //ANIMATION
        if (!isRestricted)
        {
            anim.speed = speed / walkSpeed;
        }
        else {
            anim.speed = 0f;
        }

        if (diretion == new Vector2(0f, 0f))
        {
            anim.SetBool("isMoving", false);
        }
        else { anim.SetBool("isMoving", true); }

        if (rightAxis + leftAxis > 0f)
        {
            sprite.flipX = false;
        }
        else if (rightAxis + leftAxis < 0f) { sprite.flipX = true; }


        //SITTING
        if (Input.GetKey(sit) && !isScript)
        {
            rb.sleepMode = RigidbodySleepMode2D.StartAsleep;
            isSitting = true;

            anim.SetBool("isSitting", true);

        }
        else
        {
            rb.sleepMode = RigidbodySleepMode2D.StartAwake;
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
            fader.Play();
            StartCoroutine(Wait(2f));
            transform.position = new Vector2(transform.position.x + 10f, transform.position.y);
        }
    }


    IEnumerator Wait(float time)
    {
        yield return  new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
