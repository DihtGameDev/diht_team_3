using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public bool isRestricted = false;

    private Vector3 mousePosition;

    public float camera_Offset;

    [SerializeField]
    private float camera_position_X; // The Bigger - The closer To Player
    private float camera_position_Y;

    [SerializeField]
    private float camera_focus_speed;

    private Transform marshall;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Camera>().orthographicSize = 2.5f;
        Camera.main.backgroundColor = Color.black;
        Cursor.visible = true;
        camera_Offset = -10f;
        camera_position_X = 1.5f;
        camera_position_Y = 1.2f;
        camera_focus_speed = 9f;
        marshall = GameObject.FindGameObjectWithTag("Marshall").transform;
        
        transform.position = new Vector2(marshall.transform.position.x, marshall.transform.position.y + 1.5f);

    }

    // Update is callewd once per frame
    void Update()
    {

        this.GetComponent<Camera>().orthographicSize = 
            Mathf.Clamp(Vector2.Distance(transform.position, marshall.transform.position) * 0.5f + 1.75f, 2.5f, 3f);
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 newCameraPosition = new Vector3((camera_position_X * marshall.position.x + mousePosition.x) / (camera_position_X + 1f),
                (camera_position_Y * marshall.position.y + mousePosition.y) / (camera_position_Y + 1f), camera_Offset);

        if (!isRestricted)
        {
            transform.position = Vector3.MoveTowards(transform.position, newCameraPosition, camera_focus_speed * Time.deltaTime * Time.timeScale);
        }

    }
}
