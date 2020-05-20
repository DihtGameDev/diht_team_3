using System;
using System.Collections.Generic;
using Hospital.HospitalNurse;
using UnityEngine;

public class TriggerLogic : MonoBehaviour
{
    public bool isProblemLight;
    public static float wallHigh = 1f;
    public bool isTrigger = false;

    public float distanceOfTriggering;

    private CircleCollider2D awarenessCollier;
    private readonly HashSet<Collider2D> listeners = new HashSet<Collider2D>();

    private void Start()
    {
        awarenessCollier = GetComponent<CircleCollider2D>();
    }

    public void TriggerEvent()
    {
        var transformPosition = transform.position;
        foreach (var listener in listeners)
        {
            var layers = LayerMask.GetMask("Nurse", "Obstacle");
            Vector2 diff = (listener.transform.position - transformPosition).normalized;
            Debug.DrawRay(transformPosition, diff * 10f, Color.blue, 5f);
            var hit = Physics2D.Raycast(transformPosition + transform.up, diff, Mathf.Infinity, layers);
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                hit.transform.gameObject.GetComponent<HospitalNurseController>().OnTriggered(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log(other.name);
            listeners.Add(other);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        listeners.Remove(other);
    }
}
