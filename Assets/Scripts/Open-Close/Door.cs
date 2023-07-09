using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Device, IUseable, IActivatable, IDoorable
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
            Activate();
        }
    }


    public void Activate()
    {
        // unlock door and open
        Debug.Log("Unlocked door");
        isLocked = false;
        // OpenDoor();
    }

    public void OpenDoor()
    {
        Debug.Log("Opened Door");
        transform.Translate(Vector3.up * 4, Space.World);
    }

    public void AttemptDoor()
    {
        if (!isLocked)
        {
            OpenDoor();
        }
    }

}
