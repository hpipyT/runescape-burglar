using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class AccessPad : Device, IUseable
{
    public Device controlledDevice;

    public void Use()
    {
        Debug.Log("Trying access pad");
    }

    public void UseWith(Item item)
    {
        Debug.Log("Trying access pad with item: " + item.name);
        AttemptAccess(item.GetComponent<Device>());
    }


    // has a keycard reader
    // has a numpad
    // has an electronic bypass


    private void AttemptAccess(Device device)
    {
        Debug.Log(device.name);

        controlledDevice.TryDevice(device);


    }
}
