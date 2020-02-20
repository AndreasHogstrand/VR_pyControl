using UnityEngine;
using System;

/**
 * This interface is used to implement the Delegation pattern: a target will delegate part of its responsability
 * to the class that implements the main task.
 */
public interface VGOTargetDelegate
{
    /**
     * This function should be called whenever a target detects a collision
     */
    void VGOCollisionOccurred(DateTime ts);
}

/**
 * This class implements the behavior of a generic target for the Visual Guided Oriented Task
 */
public class VGOTarget : MonoBehaviour {

    /* Configuration variables */
    public float CollisionDuration = 0.5f;
    public bool DestroyOnCollision = true;
    public VGOTargetDelegate Delegate { get; set; }
    /*********/

    private float currentTimer;
    private DateTime timestamp;

    void Start() {}
    void Update() {}

    /**
     * This function is automatically called when a collision with the target starts
     */
    void OnTriggerEnter(Collider other)
    {
        currentTimer = CollisionDuration;      
        timestamp = DateTime.Now;
    }

    /**
     * This function is automatically called once per frame, if the collision persists
     */
    void OnTriggerStay(Collider other)
    {
        currentTimer -= Time.deltaTime;
        Debug.Log(currentTimer);
        if (currentTimer < 0)
        {
            if(DestroyOnCollision) Destroy(gameObject);
            if(DestroyOnCollision) Destroy(transform.parent.gameObject);
            Delegate?.VGOCollisionOccurred(timestamp);
        }
    }

    /**
     * This function is automatically called when a collision with the target ends
     */
    void OnTriggerExit(Collider other)
    {
        currentTimer = CollisionDuration;
    }

}
