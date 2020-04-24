using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class Dialoge : MonoBehaviour
{
    public string name;

    [TextArea(2, 7)]
    public string[] sentences;

}
