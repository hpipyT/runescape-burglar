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
        Debug.Log("Trying access pad with " + item.name);
        if (item.TryGetComponent(out IHackable hackDevice))
        {
            AttemptAccessFromPad(item.GetComponent<Device>());
        }    
    }

    private void AttemptAccessFromPad(Device device)
    {
        Debug.Log("Accessing " + controlledDevice.name + " from " + this.name);

        // if the input device has property to activate doors,
        if (controlledDevice.TryGetComponent(out IActivatable activates))
        {
            activates.Activate();
            controlledDevice.InspectDevice();
        }

        // controlledDevice.TryDevice(device);
    }
}
