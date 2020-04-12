using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RenderingController : MonoBehaviour
{

    public bool isStatic;
    public bool isFadable;

    GameObject checkCollider;

    [SerializeField]
    private int order;

    Rigidbody2D rb;

    BoxCollider2D collider;

    private float defaultY;
    // Start is called before the first frame update
    void Start()
    {

        collider = GetComponent<BoxCollider2D>();
        //checkerToFade = Instantiate(Resources.Load("CheckerToFade", typeof(GameObject))) as GameObject;
        rb = GetComponent<Rigidbody2D>();
        defaultY = 0f;
        checkCollider = null;

        if (rb != null && rb.bodyType == RigidbodyType2D.Static) {
            isStatic = true;
        }

        #region FADING

        if (isFadable) {
            checkCollider = transform.Find("CheckerToFade").gameObject;
            if (checkCollider != null)
            {
                checkCollider.AddComponent<FadeChecker>();
            }
        }
        #endregion

        if (isStatic)
        {
            if (this.CompareTag("Floor"))
            {
                order = -10000;
            }
            else
            {
                order = (int)((-100f) * (this.transform.position.y +
                    ((collider != null) ? (collider.offset.y - collider.size.y / 2f + defaultY) : 0)));
            }
        }

        changeSortOrder(this.gameObject, order);

    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (!isStatic) {
        
            order = (int)((-100f) * (this.transform.position.y +
                               ((collider != null) ? (collider.offset.y - collider.size.y / 2f + defaultY) : 0)));
            changeSortOrder(this.gameObject, order);
        }

    }


    void changeSortOrder(GameObject obj, int order) {
        if (obj.transform.GetComponent<SpriteRenderer>() != null)
        {
            obj.transform.GetComponent<SpriteRenderer>().sortingOrder = order;
        }

        if (obj.transform.GetComponent<TilemapRenderer>() != null) {
            obj.transform.GetComponent<TilemapRenderer>().sortingOrder = order;
        }


        if (obj.transform.childCount == 0)
        {
            return;
        }
        else {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                changeSortOrder(obj.transform.GetChild(i).gameObject, order);
            }
        }
    }

}
