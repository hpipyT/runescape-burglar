using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Device, IUseable, IActivatable
{
    public bool isLocked;

    public void Use()
    {
        AttemptDoor();
    }

    public void UseWith(Item item)
    {
        if (item.tag == "AccessTool")
        {
            Enable();
        }
    }

    public void Enable()
    {
        Debug.Log("Unlocked door");
        isLocked = false;
    }

    public void Activate()
    {
        // unlock door and open
        Enable();
        OpenDoor();
    }

    public void AttemptDoor()
    {
        if (!isLocked)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        Debug.Log("Opened Door");
        transform.Translate(Vector3.up, Space.World);
    }
}
