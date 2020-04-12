using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class HospitalNurseController : MonoBehaviour
{

    public float height;

    private GameObject marshall;

    private MarshallController marshallController;

    private VisualDetecterController visualDetecter;


  //  [SerializeField]
    private float maxTimeOfChasing;
   // [SerializeField]
    private float currentTimeOfChasing;

    private AIPath pathfinder;
   
    [SerializeField]
    private bool isCameToTarget; // не знает куда идти
    [SerializeField]
    private Vector2 target; // точка назначения

    public Vector2 defaultPosition;

    //VELOCITY
    [SerializeField]
    private float speed;
    private float walkSpeed;
    private float runSpeed;

    //DETECTING MARSHALL
    [SerializeField]
    private bool isMarshallVisible; // видет ли враг ГГ

    [SerializeField]
    private bool isCalm; // враг спокоен
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

    [SerializeField]
    private bool hasEverNoticed; // замечал ли враг вообще

    //TIMINGS
    [SerializeField]
    private float seriosnessOfAnxiety; //насколько увеличивается опасность обнаружения если не успокоился
    [SerializeField]
    private float influence_of_distance_on_Wander; //как сильно расстояние влияет на скорость обнаружения

    private float timeOfWandering; // время чтобы когда можно спрятаться
    private float timeOfAnxiousWandering; // время чтобы можно спрятаться после того как почти заметили

    private float cur_timeOfWandering, cur_timeOfAnxiousWandering;

    [SerializeField]
    private float changable_timeOfCalming, changable_timeOfEasyCalming, changable_timeOfWandering;

    [SerializeField]
    private float timeOfPatheringAfterDisappear;

    private float timeOfEasyCalming; // время успокоения после того как почти заметили
    private float timeOfCalming; // время успокоения после того как заметили

    //ANIMATION
    Animator alarmAnimator;
    Animator anim;


    // Start is called before the first frame update
    void Start()
    {

        //Default VARIABLES
        marshall = GameObject.FindGameObjectWithTag("Marshall").gameObject;
        marshallController = marshall.GetComponent<MarshallController>();

        visualDetecter = transform.Find("VisualDetecter").transform.GetComponent<VisualDetecterController>();
        isMarshallVisible = visualDetecter.GetComponent<VisualDetecterController>().isMashallVisible;
        alarmAnimator = transform.Find("Alarm").GetComponent<Animator>();
        anim = GetComponent<Animator>();
        anim.SetBool("isRight", false);

        pathfinder = GetComponent<AIPath>();

        isCalm = true;
        isWandering = false;
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

        defaultPosition = transform.position;

        target = defaultPosition;

        walkSpeed = 1.0f;
        runSpeed = 1.5f;
        speed = walkSpeed;

        maxTimeOfChasing = 10f;
      
        seriosnessOfAnxiety = 3f; 
        influence_of_distance_on_Wander = 4f;

        changable_timeOfCalming = 6f;
        changable_timeOfEasyCalming = 2f;
        changable_timeOfWandering = 4f;

        timeOfPatheringAfterDisappear = 0.12f;

        timeOfWandering = findAnimationClip(alarmAnimator.runtimeAnimatorController.animationClips, "Wander").length;
        timeOfAnxiousWandering = findAnimationClip(alarmAnimator.runtimeAnimatorController.animationClips, "Wander").length / seriosnessOfAnxiety;
        timeOfEasyCalming = findAnimationClip(alarmAnimator.runtimeAnimatorController.animationClips, "Anxious").length;
        timeOfCalming = findAnimationClip(alarmAnimator.runtimeAnimatorController.animationClips, "veryAnxious").length;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      
        cur_timeOfWandering = Vector2.Distance(transform.position, marshall.transform.position) * changable_timeOfWandering / influence_of_distance_on_Wander;
        cur_timeOfAnxiousWandering = cur_timeOfWandering / seriosnessOfAnxiety;

        //SPEEDING THE ANIMATION
        if (isWandering)
        {
            alarmAnimator.speed = (timeOfWandering / changable_timeOfWandering) * 
                (influence_of_distance_on_Wander / Vector2.Distance(transform.position, marshall.transform.position));
        }
        else if (isVeryAnxious)
        {
            alarmAnimator.speed = timeOfCalming / changable_timeOfCalming;
        }
        else if (isAnxious)
        {
            alarmAnimator.speed = timeOfEasyCalming / changable_timeOfEasyCalming;
        }
        else {
            alarmAnimator.speed = 1f;
        }

        //DETECTING MARSHALL
        isMarshallVisible = visualDetecter.isMashallVisible;


        #region detectingLogic
        if (isMarshallVisible)
        {         
            if (isCalm) {
                StopAllCoroutines();
                isCalm = false;
                isWandering = true;
                StartCoroutine(wander());
            }
            if (isAnxious) {
                StopAllCoroutines();
                isAnxious = false;
                isWandering = true;
                StartCoroutine(anxiousWander());
            }
            if (isVeryAnxious || isRunningToPoint) {
                StopAllCoroutines();
                isVeryAnxious = false;
                isRunningToPoint = false;
                marshallController.number_of_rushers++;
                isNeedToRush = true;         
            }
        }
        else {      
            if (isNeedToRush) {
                isNeedToRush = false;
                isRunningToPoint = true;
                StartCoroutine(run());                
            }
   
        }
        #endregion

        //MOVING
        #region moving


        //Defining the target to move, speed and area of viewing
        if (isNeedToRush)
        {
            target = marshall.transform.position;
        }

        if (isNeedToRush || isRunningToPoint)
        {
            speed = runSpeed;
            visualDetecter.distanceOfViewing = visualDetecter.angryDistanceOfViewing;
        }
        else {
            speed = walkSpeed;
            visualDetecter.distanceOfViewing = visualDetecter.calmDistanceOfViewing;
        }

        if (isCalm && isCameToTarget) {
            target = defaultPosition;
        }


       
        pathfinder.destination = target;
        pathfinder.maxSpeed = speed;

        //Conditions of coming to target
        if (Vector2.Distance(transform.position, target) < pathfinder.endReachedDistance + 0.3f)
        {
            pathfinder.canSearch = false;
            isCameToTarget = true;
            anim.SetBool("isMoving", false);
        }
        else {
            pathfinder.canSearch = true;
            isCameToTarget = false;
            anim.SetBool("isMoving", true);
      
        }

        if (Vector2.Distance(marshall.transform.position, this.transform.position) < 0.8f) {
            marshallController.isCaptured = true;
        }
        #endregion

        //ANIMATION
        #region animation

        if (pathfinder.velocity.x > 0)
        {
            anim.SetBool("isRight", true);
        }
        else if (pathfinder.velocity.x <= 0)
        {
            anim.SetBool("isRight", false);
        }

        if (isCalm)
        {
            alarmAnimator.SetBool("isCalm", true);
        }
        else { alarmAnimator.SetBool("isCalm", false); }
        if (isWandering)
        {
            alarmAnimator.SetBool("isWander", true);
        }
        else { alarmAnimator.SetBool("isWander", false); }
        if (isAnxious)
        {
            alarmAnimator.SetBool("isAnxious", true);
        }
        else { alarmAnimator.SetBool("isAnxious", false); }
        if (isNeedToRush)
        {
            alarmAnimator.SetBool("isNeedToRush", true);
        }
        else { alarmAnimator.SetBool("isNeedToRush", false); }
        if (isRunningToPoint)
        {
            alarmAnimator.SetBool("isRunToPoint", true);
        }
        else { alarmAnimator.SetBool("isRunToPoint", false); }

        if (isVeryAnxious)
        {
            alarmAnimator.SetBool("isVeryAnxious", true);
        }
        else { alarmAnimator.SetBool("isVeryAnxious", false); }

        
        #endregion
    }

    #region detectingTimingIEnunerators
    IEnumerator wander()
    {
        float timer = 0f;
        
        while (timer < cur_timeOfWandering) {
           
            timer += Time.deltaTime;
           
            yield return null;
        }
       
        isWandering = false;

        if (isMarshallVisible)
        {
            isNeedToRush = true;

            hasEverNoticed = true;
            marshallController.number_of_rushers++;
        }
        else {
            isAnxious = true;
            StartCoroutine(easyCalmDown(changable_timeOfEasyCalming));
        }        
    }

    IEnumerator anxiousWander()
    {
        float timer = 0f;
        
        while (timer < cur_timeOfAnxiousWandering)
        {
           
            timer += Time.deltaTime;
           
            yield return null;
        }
        isWandering = false;

        if (isMarshallVisible)
        {
            isNeedToRush = true;

            marshallController.number_of_rushers++;
        }
        else
        {
            isAnxious = true;
        }
    }

    IEnumerator calmDown(float time)
    {

        isVeryAnxious = true;
        yield return new WaitForSeconds(time);
        isVeryAnxious = false;

 
        isAnxious = true;
        target = marshall.transform.position;
        marshallController.number_of_rushers--;

        StartCoroutine(easyCalmDown(changable_timeOfEasyCalming));
    }

    IEnumerator run()
    {
        StartCoroutine(leftOverPathering(timeOfPatheringAfterDisappear));

        while (!isCameToTarget) {
            yield return null;
        }

        isRunningToPoint = false;
        StartCoroutine(calmDown(changable_timeOfCalming));

    }

    IEnumerator easyCalmDown(float time)
    {
        yield return new WaitForSeconds(time);

        isAnxious = false;
        isCalm = true;
    }

    IEnumerator leftOverPathering(float time) {
        while (time > 0) {

            target = marshall.transform.position;
            time -= Time.deltaTime;
            yield return null;
        }       
    }
    #endregion

    private AnimationClip findAnimationClip(AnimationClip[] array, string findName) {
        foreach (var obj in array) {
            if (obj.name == findName) {
                return obj;
            }
        }
        return null;
    }
}
