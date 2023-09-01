using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class AccessPad : Device, IUseable, IOptionDisplayable
{
    public bool isHacked;

    public Dictionary<string, System.Action> GetActionsToDisplay()
    {
        Dictionary<string, Action> displayOptions = new Dictionary<string, Action>();
        return displayOptions;
    }

    // list of devices
    public List<Device> controlledDevices;
    // select device to control

    public void Use()
    {
        Debug.Log("Trying access pad");
        if (!isHacked)
        {
            Debug.Log("Pad is password protected");
        }
        else
        {
            // bring up controlled devices popup

            // attempt to access device from pad
        }
    }

    public void UseWith(Item item)
    {
        Debug.Log("Trying access pad with " + item.name);

        // if device can hack, allow access to pad
        if (item.TryGetComponent(out Device hackDevice))
        {
            if (hackDevice.hacksPanels)
                isHacked = true;
            
        }    
    }

    private void AttemptAccessFromPad(Device device)
    {
        Debug.Log("Accessing " + controlledDevices[0].name + " from " + this.name);

        // get the object's function meant for activating from remote
        if (controlledDevices[0].TryGetComponent(out IActivatable activates))
        {
            activates.Activate();
            controlledDevices[0].InspectDevice();
        }
    }
}
