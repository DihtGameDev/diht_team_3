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
    private float camera_focus_speed_start;

    private Transform marshall;
    private MarshallController marshallController;
    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponent<AudioListener>() == null) {
            this.gameObject.AddComponent<AudioListener>();
            
        }
        this.GetComponent<Camera>().orthographicSize = 2.5f;
        Camera.main.backgroundColor = Color.black;
        Cursor.visible = true;
        camera_Offset = -10f;
        camera_position_X = 1.5f;
        camera_position_Y = 1.2f;

        camera_focus_speed_start = 7f;
        marshall = GameObject.FindGameObjectWithTag("Marshall").transform;
        marshallController = marshall.GetComponent<MarshallController>();
        transform.position = new Vector2(marshall.transform.position.x, marshall.transform.position.y + 1.5f);

    }

    // Update is callewd once per frame
    void Update()
    {

        this.GetComponent<Camera>().orthographicSize = 
            Mathf.Clamp(Vector2.Distance(transform.position, marshall.transform.position) * 0.4f + 1.5f + 1f * Global.camera_sensitivity,
            2f + Global.camera_sensitivity, 2.8f + Global.camera_sensitivity);
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 newCameraPosition = new Vector3((camera_position_X * marshall.position.x + mousePosition.x) / (camera_position_X + 1f),
                (camera_position_Y * marshall.position.y  + mousePosition.y) / (camera_position_Y + 1f), camera_Offset);

        if (!isRestricted)
        {
            transform.position = Vector3.MoveTowards(transform.position, newCameraPosition, 
                (camera_focus_speed_start + 5f * Global.camera_sensitivity) * Time.deltaTime);
        }

    }

    public IEnumerator Shake(float duration, float magnitude, float offset = 0f) {

        yield return new WaitForSeconds(offset);
        isRestricted = true;
        Vector3 start_position = transform.localPosition;

        float timer = 0f;

        Debug.Log("Camera");
        while (timer < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;


            transform.localPosition = new Vector3(start_position.x + x, start_position.y, camera_Offset);

            timer += Time.deltaTime;

            yield return  null;
        }

        isRestricted = false;


    }
}
