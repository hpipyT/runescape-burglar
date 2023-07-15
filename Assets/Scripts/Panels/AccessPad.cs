using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class AccessPad : Device, IUseable, IOptionDisplayable
{
    public Dictionary<string, System.Action> GetActionsToDisplay()
    {
        Dictionary<string, Action> displayOptions = new Dictionary<string, Action>();

        displayOptions["Examine"] = () => // displayOptions comes from Item class
        {
            Debug.Log("Electronic interface for communicating with connected devices");
        };

        return displayOptions;
    }

    // list of devices
    public Device controlledDevice;
    // select device to control

    public void Use()
    {
        Debug.Log("Trying access pad");
    }

    public void UseWith(Item item)
    {
        Debug.Log("Trying access pad with " + item.name);

        // if device can hack, allow access to pad
        if (item.TryGetComponent(out Device hackDevice))
        {
            if (hackDevice.hacksPanels)
            AttemptAccessFromPad(hackDevice);
        }    
    }

    private void AttemptAccessFromPad(Device device)
    {
        Debug.Log("Accessing " + controlledDevice.name + " from " + this.name);

        if (controlledDevice.TryGetComponent(out IActivatable activates))
        {
            activates.Activate();
            controlledDevice.InspectDevice();
        }
    }
}
