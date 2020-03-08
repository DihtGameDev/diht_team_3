using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
 using UnityEngine.EventSystems;

public class ButtomAnimation : MonoBehaviour
{
    public void OnEnter() {
        transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
    }

    public void OnClick()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void OnExit()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
