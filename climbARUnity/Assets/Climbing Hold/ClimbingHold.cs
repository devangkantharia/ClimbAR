﻿using UnityEngine;
using System.Collections;
using System;
using System.Threading;

public class ClimbingHold : MonoBehaviour
{
    public int enterCount = 0;
    public float grabbedSmoothing = 200 / 1000; // divided by 1000 to put as fraction of second
    public float releasedSmoothing = 200 / 1000;

    public bool smoothingEnabled = true;
    public enum States { Released, GrabbedPending, Grabbed, ReleasePending }
    public States currentState = States.Released;

    void Start()
    {
    }

    void Update()
    {
        if (currentState == States.Grabbed)
        {
            ClimbARHandhold.setHoldColor(gameObject, ClimbARHandhold.ENTERED_COLOR);
        }
        else if (currentState == States.Released)
        {
            ClimbARHandhold.setHoldColor(gameObject, ClimbARHandhold.RESET_COLOR);
        }
    }

    private IEnumerator grabbedPending()
    {
        yield return new WaitForSeconds(grabbedSmoothing);

        // if after the sleep we are still in pending state, change to grabbed
        if (currentState == States.GrabbedPending)
        {
            currentState = States.Grabbed;
        }
    }

    private IEnumerator releasedPending()
    {

        yield return new WaitForSeconds(releasedSmoothing);

        // if after the sleep we are still in pending state, change to released
        if (currentState == States.ReleasePending)
        {
            currentState = States.Released;
        }
    }

    private void updateStateOnCollisionExit()
    {
        switch (currentState)
        {
            case States.Released:
                break;
            case States.GrabbedPending:
                if (enterCount == 0)
                {
                    currentState = States.Released;
                }
                break;
            case States.Grabbed:
                if (enterCount == 0)
                {
                    currentState = States.ReleasePending;
                    StartCoroutine("releasedPending");
                }
                break;
            case States.ReleasePending:
                break;
        }
    }

    private void updateStateOnCollisionEnter()
    {
        switch (currentState)
        {
            case States.Released:
                currentState = States.GrabbedPending;
                StartCoroutine("grabbedPending");
                break;
            case States.GrabbedPending:
                break;
            case States.Grabbed:
                break;
            case States.ReleasePending:
                currentState = States.Grabbed;
                break;
        }
    }

    public void ShouldRegisterHoldGrabbed(Collider2D col)
    {
        // Check that not colliding with overlapping hold
        if (col != null && col.gameObject.tag == "Hold")
        {
            return;
        }
        // If we don't want smoothing, just return true
        if (!smoothingEnabled)
        {
            return;
        }

        enterCount++;
        updateStateOnCollisionEnter();
    }

    public void ShouldRegisterHoldReleased(Collider2D col)
    {
        // Check that not colliding with overlapping hold
        if (col != null && col.gameObject.tag == "Hold")
        {
            return;
        }

        // If we don't want smoothing, just return true
        if (!smoothingEnabled)
        {
            return;
        }

        // We don't ever want to go negative. 
        // Going negative could happend if the kinect finds hands and 
        // creates collider insider hold. This will never trigger 
        // OnCollisionEnter (where enterCount++) and then on exit will 
        // decrement count below zero
        if (enterCount > 0)
        {
            enterCount--;
        }
        updateStateOnCollisionExit();
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        ShouldRegisterHoldReleased(col);
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        ShouldRegisterHoldGrabbed(col);
    }

    /*
    void OnMouseDown()
    {
        Destroy(gameObject);
    }*/
}
