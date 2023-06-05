using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessPad : ControlledDevice
{

    public ControlledDevice device;

    // has a keycard reader
    // has a numpad
    // has an electronic bypass

    public override void TryDevice()
    {
        base.TryDevice();
    }

    public override void TryDevice(GameObject input)
    {
        base.TryDevice();
        AttemptAccess(input);
    }

    public void AttemptAccess(GameObject item)
    {
        if (item.tag == "AccessTool")
        {
            device.ActivateDevice();
        }
    }
}
