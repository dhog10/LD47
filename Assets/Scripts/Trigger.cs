using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent m_OnPlayerEnter;
    public UnityEvent m_OnPlayerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null)
        {
            return;
        }

        var player = other.attachedRigidbody.GetComponentInChildren<TankCharacterController>();
        if (player != null)
        {
            m_OnPlayerEnter?.Invoke();
            Debug.Log("trigger enter " + player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody == null)
        {
            return;
        }

        var player = other.attachedRigidbody.GetComponentInChildren<TankCharacterController>();
        if (player != null)
        {
            m_OnPlayerExit?.Invoke();
            Debug.Log("trigger exit " + player);
        }
    }
}
