using System;
using System.Collections;
using System.Collections.Generic;
using Hospital.HospitalNurse;
using UnityEngine;
using Pathfinding;
using UnityEngine.Serialization;

public class HospitalNurseController : MonoBehaviour, ILightTriggerable
{

    [HideInInspector]
    public float height;

    public GameObject marshall;
    public MarshallController marshallController;

    public VisualDetecterController visualDetecter;

    private SpriteRenderer alarm1;
    private SpriteRenderer alarm2;
    private SpriteRenderer alarm3;

    public Sprite question;
    public Sprite exclamation;

    private SpriteRenderer enemySprite;


    //  [SerializeField]
    private float maxTimeOfChasing;
    // [SerializeField]
    private float currentTimeOfChasing;

    private AIPath pathfinder;

    [SerializeField] public bool isCameToTarget; // не знает куда идти
    [SerializeField]
    public Vector2 target; // точка назначения


    //VELOCITY

    public float speed;
    public float walkSpeed;
    public float runSpeed;

    //DETECTING MARSHALL

    public bool isMarshallVisible; // видет ли враг ГГ

    [SerializeField]
    private float _annoyment = 0f;
    
    public float Annoyment
    {
        get => _annoyment;
        set
        {
            _annoyment = Math.Max(Math.Min(value, RUSHING_TRESHHOLD), 0);
            if (Math.Abs(_annoyment) < 0.01f)
            {
                filling(0f);
            }
            else
            {
                filling(0.5f * _annoyment / RUSHING_TRESHHOLD + 0.5f);
            }
        }
    }

    public GameObject lastDistractor;
    public static readonly float WANDERING_TRESHHOLD = 60f;
    public static readonly float RUSHING_TRESHHOLD = 100f;

    [SerializeField]
    private NurseState state;

    //ANIMATION

    Animator anim;

    public bool isLookingRight;

    public List<Vector2> defaultTrajectory = new List<Vector2>();

    public float waitingOnDefaultPoint = 1.0f;
    public float waitingOnUndefaultPoint = 3.0f;
    public int pointerOfDefMove = 0;
    [FormerlySerializedAs("_waitingOnDefaultPoint")] public float currentWaitingOnPoint = 0f;

    [SerializeField]
    private AudioController audioController;
    // Start is called before the first frame update
    void Start()
    {
        audioController = Global.audioController;

        alarm1 = transform.Find("Alarm").transform.GetChild(0).GetComponent<SpriteRenderer>();
        alarm2 = transform.Find("Alarm").transform.GetChild(1).GetComponent<SpriteRenderer>();
        alarm3 = transform.Find("Alarm").transform.GetChild(2).GetComponent<SpriteRenderer>();

        alarm1.material.SetFloat("_Fill", 0.0f);
        alarm2.material.SetFloat("_Fill", 0.0f);
        alarm3.material.SetFloat("_Fill", 0.0f);

        if (defaultTrajectory.Count == 0)
        {
            defaultTrajectory.Add(transform.position);
        }

        /*for (int i = 0; i < defaultTrajectory.Count; i++) {
            defaultTrajectory[i] += new Vector2(transform.parent.position.x, transform.parent.position.y);
        }*/
        target = defaultTrajectory[0];

        //Default VARIABLES
        marshall = GameObject.FindGameObjectWithTag("Marshall").gameObject;
        marshallController = marshall.GetComponent<MarshallController>();

        visualDetecter = transform.Find("VisualDetecter").transform.GetComponent<VisualDetecterController>();

        anim = GetComponent<Animator>();
        anim.SetBool("isRight", isLookingRight);
        enemySprite = transform.Find("EnemyRenderer").transform.GetComponent<SpriteRenderer>();

        pathfinder = GetComponent<AIPath>();

        isMarshallVisible = false;

        state = IdleState.GetInstance();
        state.Start(this);
        
        isCameToTarget = false;

        //CHANGABLE VARIABLES
        height = 0f;

        walkSpeed = 1.2f;
        runSpeed = 2.5f;
        speed = walkSpeed;

        maxTimeOfChasing = 10f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //DETECTING MARSHALL
        isMarshallVisible = visualDetecter.isMashallVisible;

        if (isMarshallVisible)
        {
            lastDistractor = marshall;
        }

        var newState = state.Tick(this);
        if (newState != state)
        {
            state.End(this);
            state = newState;
            state.Start(this);
            Debug.Log(newState);
        }

        //MOVING
        #region moving

        pathfinder.destination = target; //+ new Vector2(transform.parent.position.x, transform.parent.position.y);
        pathfinder.maxSpeed = speed;

        //Conditions of coming to target
        if (Vector2.Distance(transform.position, target) < pathfinder.endReachedDistance + 0.3f)
        {
            pathfinder.canSearch = false;
            isCameToTarget = true;
        }
        else
        {
            pathfinder.canSearch = true;
            isCameToTarget = false;
            anim.SetBool("isMoving", true);
        }

        if (Vector2.Distance(marshall.transform.position, transform.position) < 0.6f)
        {
            marshallController.isCaptured = true;
            StartCoroutine(glitch(0.4f));
        }
        #endregion

        //ANIMATION
        #region animation


        anim.speed = pathfinder.velocity.magnitude/ walkSpeed;

        if (pathfinder.velocity.x > 0.001f)
        {
            anim.SetBool("isRight", true);
        }
        else if (pathfinder.velocity.x < -0.001f)
        {
            anim.SetBool("isRight", false);
        }

        anim.SetBool("isMoving", pathfinder.velocity.magnitude > 0.1f);

        if (state is IdleState && isCameToTarget && defaultTrajectory.Count == 1) {
            anim.SetBool("isRight", isLookingRight);
        }

        #endregion
    }

    public void exclam()
    {
        alarm1.sprite = exclamation;
        alarm2.sprite = exclamation;
        alarm3.sprite = exclamation;
    }

    public void quest()
    {
        alarm1.sprite = question;
        alarm2.sprite = question;
        alarm3.sprite = question;
    }
    
    public void filling(float fill)
    {
        alarm1.material.SetFloat("_Fill", fill);
        alarm2.material.SetFloat("_Fill", fill);
        alarm3.material.SetFloat("_Fill", fill);
    }

    public IEnumerator glitch(float time) {
        StartCoroutine(audioController.Play("Alarm"));
        enemySprite.material.SetVector("_Glitch", new Vector4(0f, 0.3f, 0f, 0f));
        enemySprite.material.SetFloat("_GlitchOffset", 0.18f);
        yield return new WaitForSeconds(time);
        enemySprite.material.SetFloat("_GlitchOffset", -0.05f);
        enemySprite.material.SetVector("_Glitch", new Vector4(0f, 0f, 0f, 0f));
        enemySprite.material.SetFloat("_GlitchOffset", 0.15f);
    }

    public void OnTriggered(GameObject triggerSource)
    {
        lastDistractor = triggerSource;
        Annoyment += (WANDERING_TRESHHOLD / 1.1f) * (1.2f - Vector2.Distance(transform.position, triggerSource.transform.position) / 8f);
    }
}
