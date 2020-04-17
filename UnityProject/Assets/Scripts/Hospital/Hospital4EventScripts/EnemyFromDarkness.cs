using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFromDarkness : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private bool isDone = false;
    public HospitalNurseController nurse;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Marshall"))
         {
            if (!isDone) {
                isDone = true;
                nurse.defaultTrajectory.Add(new Vector2(12.25f, 0.54f));
                nurse._waitingOnDefaultPoint = 10f;

            }
        }
    }
}
