using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Device, IUseable, IActivatable, IOptionDisplayable
{
    public bool isLocked;
    public bool isOpen;


    public Dictionary<string, Action> GetActionsToDisplay()
    {

        Dictionary<string, Action> displayOptions = new Dictionary<string, Action>();

        // Map display strings to actions
        if (!isOpen)
            displayOptions["Open Door"] = () => // displayOptions comes from Item class
            {
                AttemptDoor();
            };

        else
            displayOptions["Close Door"] = () =>
            {
                AttemptDoor();
            };



        return displayOptions;
    }

    public void Use()
    {
        AttemptDoor();
    }

    public void AttemptDoor()
    {
        if (!isLocked && !isOpen)
        {
            OpenDoor();
            isOpen = true;
        }
        else if (isOpen)
        {
            CloseDoor();
            isOpen = false;
        }
        else
        {
            Debug.Log("Door is Locked");
        }
    }

    public void UseWith(Item item)
    {
        // electronic door opener

        // crowbar
    }

    // Unlock
    public void Activate()
    {
        Debug.Log("Unlocked door");
        isLocked = false;
        // OpenDoor();
    }

    public virtual void OpenDoor()
    {
        Debug.Log("Opened Door");
        transform.Translate(Vector3.up * 4, Space.World);
    }

    public virtual void CloseDoor()
    {
        Debug.Log("Closed Door");
        transform.Translate(Vector3.up * -4, Space.World);
    }

}
