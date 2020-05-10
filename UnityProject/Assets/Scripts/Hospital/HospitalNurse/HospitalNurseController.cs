using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class HospitalNurseController : MonoBehaviour
{

    [HideInInspector]
    public float height;

    private GameObject marshall;
    private MarshallController marshallController;

    private VisualDetecterController visualDetecter;

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

    [SerializeField]
    private bool isCameToTarget; // не знает куда идти
    [SerializeField]
    private Vector2 target; // точка назначения


    //VELOCITY

    private float speed;
    private float walkSpeed;
    private float runSpeed;

    //DETECTING MARSHALL

    private bool isMarshallVisible; // видет ли враг ГГ

    //LIGHT TRIGGERING
    public class LightTrigger
    {  // кто то рядом выключает или включает свет
        public bool isTriggerOnTheLight;
        public GameObject light;
        public LightTrigger(bool b, GameObject l = null)
        {
            isTriggerOnTheLight = b;
            light = l;
        }
    }
    [SerializeField]
    public LightTrigger lightTrigger = new LightTrigger(false);


    public float wanderingTimer = 0f;
    [SerializeField]
    private bool isCalm; // враг спокоен
    [SerializeField]
    private bool isWanderingOnLight;
    [SerializeField]
    private bool isWandering; //враг сейчас тебя заметит и есть время чтобы спрятаться
    [SerializeField]
    private bool isAnxious; //враг почти тебя заметил и успокаивается
    [SerializeField]
    private bool isVeryAnxious; //враг тебя заметил, но упустил из виду и пытается успокоится (не преследует)
    [SerializeField]
    private bool isNeedToRush; // враг тебя видит и преследует
    [SerializeField]
    private bool isRunningToPoint; // враг тебя преследует, но не видит



    private bool hasEverNoticed; // замечал ли враг вообще

    //TIMINGS

    private float seriosnessOfAnxiety; //насколько увеличивается опасность обнаружения если не успокоился

    private float influence_of_distance_on_Wander; //как сильно расстояние влияет на скорость обнаружения

    private float timeOfWandering; // время чтобы когда можно спрятаться
    private float timeOfAnxiousWandering; // время чтобы можно спрятаться после того как почти заметили
    private float timeOfLightWandering; // время тригера на свет

    private float cur_timeOfWandering, cur_timeOfAnxiousWandering, cur_timeOfLightWandering;


    private float changable_timeOfCalming, changable_timeOfEasyCalming, 
        changable_timeOfWandering, changable_timeOfLightWandering;


    private float timeOfPatheringAfterDisappear;

    private float timeOfEasyCalming; // время успокоения после того как почти заметили
    private float timeOfCalming; // время успокоения после того как заметили

    //ANIMATION

    Animator anim;

    public bool isLookingRight;

    public List<Vector2> defaultTrajectory = new List<Vector2>();

    public float waitingOnDefaultPoint = 1.0f;
    private int pointerOfDefMove = 0;
    public float _waitingOnDefaultPoint = 0f;

    [SerializeField]
    private AudioController audioController;
    // Start is called before the first frame update
    void Start()
    {
        audioController = Global.audioController;

        alarm1 = transform.FindChild("Alarm").transform.GetChild(0).GetComponent<SpriteRenderer>();
        alarm2 = transform.FindChild("Alarm").transform.GetChild(1).GetComponent<SpriteRenderer>();
        alarm3 = transform.FindChild("Alarm").transform.GetChild(2).GetComponent<SpriteRenderer>();

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
        isMarshallVisible = visualDetecter.GetComponent<VisualDetecterController>().isMashallVisible;

        anim = GetComponent<Animator>();
        anim.SetBool("isRight", isLookingRight);
        enemySprite = transform.Find("EnemyRenderer").transform.GetComponent<SpriteRenderer>();

        pathfinder = GetComponent<AIPath>();

        isMarshallVisible = false;

        isCalm = true;
        isWandering = false;
        isWanderingOnLight = false;
        isAnxious = false;
        isVeryAnxious = false;
        isNeedToRush = false;
        isRunningToPoint = false;
        isCameToTarget = false;
        hasEverNoticed = false;

        cur_timeOfWandering = timeOfWandering;
        cur_timeOfAnxiousWandering = timeOfAnxiousWandering;

        //CHANGABLE VARIABLES
        height = 0f;

        walkSpeed = 1.2f;
        runSpeed = 2.5f;
        speed = walkSpeed;

        maxTimeOfChasing = 10f;

        seriosnessOfAnxiety = 3f;
        influence_of_distance_on_Wander = 6f;

        changable_timeOfCalming = 7f;
        changable_timeOfEasyCalming = 2f;
        changable_timeOfWandering = 3f;
        changable_timeOfLightWandering = 4f;

        timeOfPatheringAfterDisappear = 1f;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        cur_timeOfWandering = Vector2.Distance(transform.position, marshall.transform.position) * changable_timeOfWandering / influence_of_distance_on_Wander;
        cur_timeOfAnxiousWandering = cur_timeOfWandering / seriosnessOfAnxiety;
        
        if (lightTrigger.isTriggerOnTheLight)
        {
            cur_timeOfLightWandering = Vector2.Distance(transform.position, lightTrigger.light.transform.position) * changable_timeOfLightWandering / influence_of_distance_on_Wander;
        }


        //DETECTING MARSHALL
        isMarshallVisible = visualDetecter.isMashallVisible;

        #region detectingLogic
        if (lightTrigger.isTriggerOnTheLight && isCalm)
        {
            lightTrigger.isTriggerOnTheLight = false;
            isCalm = false;
            isWanderingOnLight = true;
            StartCoroutine(lightWandering(cur_timeOfLightWandering));
        }


        if (isMarshallVisible && isCalm && !marshallController.isChasable)
        {
            isCalm = false;
            isWandering = true;
            StartCoroutine(wander());
        }

        if (isMarshallVisible && marshallController.isChasable && (isCalm || isAnxious)) {
            isCalm = false; 
         
            isAnxious = false;

            isNeedToRush = true;
            marshallController.number_of_rushers++;
            filling(1f);
            exclam();
        }

        if (!isMarshallVisible && isNeedToRush)
        {
            isNeedToRush = false;
            isRunningToPoint = true;
            StartCoroutine(run());

        }
        #endregion

        //MOVING
        #region moving
        // Default moving (IEnumereator isn't resolved!!!)
        if (isCameToTarget && (isCalm || isWandering || isAnxious))
        {
            if (_waitingOnDefaultPoint > waitingOnDefaultPoint)
            {
                _waitingOnDefaultPoint = 0;
                target = defaultTrajectory[pointerOfDefMove % defaultTrajectory.Count];
                pointerOfDefMove++;
            }
            else
            {
                _waitingOnDefaultPoint += Time.deltaTime;
            }


        }
        //Defining the target to move, speed and area of viewing
        if (isNeedToRush)
        {
            target = marshall.transform.position;
        }

        if (isNeedToRush || isRunningToPoint)
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        if (isNeedToRush || isRunningToPoint || isVeryAnxious)
        {
            visualDetecter.distanceOfViewing = visualDetecter.angryDistanceOfViewing;
        }
        else {
            visualDetecter.distanceOfViewing = visualDetecter.calmDistanceOfViewing;
        }

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

        }

        if (Vector2.Distance(marshall.transform.position, this.transform.position) < 0.6f)
        {
            marshallController.isCaptured = true;
            StartCoroutine(glitch(0.4f));
        }
        #endregion

        //ANIMATION
        #region animation


        anim.speed = pathfinder.velocity.magnitude / walkSpeed;

        if (pathfinder.velocity.x > 0.001f)
        {
            anim.SetBool("isRight", true);
        }
        else if (pathfinder.velocity.x < -0.001f)
        {
            anim.SetBool("isRight", false);
        }

        if (pathfinder.velocity.magnitude > 0.1f)
        {
            anim.SetBool("isMoving", true);
        }
        else {
            anim.SetBool("isMoving", false);
        }

        if (isCalm && isCameToTarget && defaultTrajectory.Count == 1) {
            anim.SetBool("isRight", isLookingRight);
        }

        #endregion
    }

    #region detectingTimingIEnumerators


    IEnumerator lightWandering(float time)
    {
        StartCoroutine(glitch(0.18f));

        wanderingTimer = time;

        while (wanderingTimer > 0f)
        {

            wanderingTimer -= Time.deltaTime;

            filling(wanderingTimer / time);

            if (isMarshallVisible)
            {
                isWanderingOnLight = false;

                isWandering = true;
                StartCoroutine(wander());
                yield break;
            }

            if (lightTrigger.isTriggerOnTheLight)
            {
                StartCoroutine(glitch(0.21f));

                lightTrigger.isTriggerOnTheLight = false;


                target = new Vector2(lightTrigger.light.transform.position.x, lightTrigger.light.transform.position.y -
                    TriggerLogic.wallHigh);
                
                isCameToTarget = false;
                wanderingTimer = time;
                filling(1f);

                while (!isCameToTarget) {
                    wanderingTimer = time;

                    if (isMarshallVisible)
                    {
                        isWanderingOnLight = false;

                        isNeedToRush = true;
                        exclam();


                        hasEverNoticed = true;
                        marshallController.number_of_rushers++;

                        yield break;
                    }

                    yield return null;
                }

            }

            yield return null;
        }
        isWanderingOnLight = false;

        isCalm = true;
    }

    IEnumerator wander()
    {
        StartCoroutine(glitch(0.24f));
        while (wanderingTimer < cur_timeOfWandering)
        {

            wanderingTimer += Time.deltaTime;

            filling(0.5f * wanderingTimer / cur_timeOfWandering + 0.5f);


            if (lightTrigger.isTriggerOnTheLight)
            {
                StartCoroutine(glitch(0.21f));
                lightTrigger.isTriggerOnTheLight = false;

                wanderingTimer += (0.3f * wanderingTimer);
            }

            if (!isMarshallVisible)
            {
                isWandering = false;

                isAnxious = true;
                StartCoroutine(easyCalmDown());
                yield break;
            }

            yield return null;
        }

        isWandering = false;

        isNeedToRush = true;
        exclam();
        

        hasEverNoticed = true;
        marshallController.number_of_rushers++;
    }

    IEnumerator easyCalmDown()
    {
        while (wanderingTimer > 0)
        {
            wanderingTimer -= Time.deltaTime / seriosnessOfAnxiety;

            filling(0.5f * wanderingTimer / cur_timeOfWandering + 0.5f);

            if (lightTrigger.isTriggerOnTheLight)
            {
                StartCoroutine(glitch(0.21f));
                lightTrigger.isTriggerOnTheLight = false;

                wanderingTimer = cur_timeOfWandering;
            }

            if (isMarshallVisible)
            {
                isAnxious = false;

                isWandering = true;
                StartCoroutine(wander());
                yield break;
            }

            yield return null;
        }

        isAnxious = false;
        filling(0f);
        isCalm = true;
    }


    IEnumerator calmDown(float time)
    {
        float timer = time;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            filling(timer / time);


            if (isMarshallVisible)
            {
                isVeryAnxious = false;
                filling(1f);
                isNeedToRush = true;
                

                yield break;
            }
            yield return null;
        }


        isVeryAnxious = false;
        wanderingTimer = changable_timeOfEasyCalming;
        quest();

        isAnxious = true;

        marshallController.number_of_rushers--;

        StartCoroutine(easyCalmDown());
    }

    IEnumerator run()
    {
        StartCoroutine(leftOverPathering(timeOfPatheringAfterDisappear));

        while (!isCameToTarget)
        {
            if (isMarshallVisible)
            {
                isRunningToPoint = false;

                isNeedToRush = true;

                yield break;
            }

            yield return null;
        }

        isRunningToPoint = false;

        isVeryAnxious = true;
        StartCoroutine(calmDown(changable_timeOfCalming));

    }

    IEnumerator leftOverPathering(float time)
    {
        while (time > 0)
        {

            target = marshall.transform.position;
            time -= Time.deltaTime;
            yield return null;
        }
    }
    #endregion


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

    IEnumerator glitch(float time) {
        StartCoroutine(audioController.Play("Alarm"));
        enemySprite.material.SetVector("_Glitch", new Vector4(0f, 0.3f, 0f, 0f));
        enemySprite.material.SetFloat("_GlitchOffset", 0.18f);
        yield return new WaitForSeconds(time);
        enemySprite.material.SetFloat("_GlitchOffset", -0.05f);
        enemySprite.material.SetVector("_Glitch", new Vector4(0f, 0f, 0f, 0f));
        enemySprite.material.SetFloat("_GlitchOffset", 0.15f);
    }
}
