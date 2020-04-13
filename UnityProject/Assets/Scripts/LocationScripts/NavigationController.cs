using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NavigationController : MonoBehaviour
{
    [HideInInspector]
    public float periodOfScaning;

    private AstarPath pathering;

    // Start is called before the first frame update
    void Start()
    {
        pathering = GetComponent<AstarPath>();

        periodOfScaning = 2.5f;
        //StartCoroutine(scaning(periodOfScaning));
    }


    // Update is called once per frame
    void Update()
    {
        // AstarPath.active.Scan(); // WORKS

    }
    IEnumerator scaning(float time) {
        while (true)
        {
            yield return new WaitForSeconds(time);
            pathering.Scan();
        }

    }
}
