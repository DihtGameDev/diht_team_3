using System;
using System.Collections.Generic;
using Hospital.HospitalNurse;
using UnityEngine;

public class TriggerLogic : MonoBehaviour
{
    public static float wallHigh = 1.3f;

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
            var layers = LayerMask.GetMask("Default", "Enemy", "Obstacle");
            Vector2 diff = (listener.transform.position - transformPosition).normalized;
            var hit = Physics2D.Raycast(transformPosition, diff, Mathf.Infinity, layers);
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
