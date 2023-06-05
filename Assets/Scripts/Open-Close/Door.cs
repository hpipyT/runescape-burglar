using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : ControlledDevice
{
    public bool isLocked;

    public override void TryDevice()
    {
        base.TryDevice();
        AttemptDoor();
    }

    public override void EnableDevice()
    {
        isLocked = false;
    }

    public override void ActivateDevice()
    {
        // unlock door and open
        EnableDevice();
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
