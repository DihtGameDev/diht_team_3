using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public bool isRestricted = false;

    private Vector3 mousePosition;

    public float camera_Offset;

    [SerializeField]
    private float camera_position; // The Bigger - The closer To Player

    [SerializeField]
    private float camera_focus_speed;

    private Transform marshall;
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.backgroundColor = Color.black;
        Cursor.visible = true;
        camera_Offset = -10f;
        camera_position = 2f;
        camera_focus_speed = 5f;
        marshall = GameObject.FindGameObjectWithTag("Marshall").transform;
        
        transform.position = new Vector2(marshall.transform.position.x, marshall.transform.position.y + 1.5f);

    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 newCameraPosition = new Vector3((camera_position * marshall.position.x + mousePosition.x) / (camera_position + 1f),
                (camera_position * marshall.position.y + mousePosition.y) / (camera_position + 1f), camera_Offset);

        if (!isRestricted)
        {
            transform.position = Vector3.MoveTowards(transform.position, newCameraPosition, camera_focus_speed * Time.deltaTime * Time.timeScale);
        }

    }
}
