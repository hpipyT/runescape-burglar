using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledDevice : MonoBehaviour
{
    public string displayName;
    public string descriptiveName;
    public string alteredName;

    // base try device
    public virtual void TryDevice()
    {
        displayName = descriptiveName;
    }


    // try device with selected item
    public virtual void TryDevice(GameObject input)
    {
        TryDevice();
    }

    // for bypassing impedences, like locks or firewalls
    // unlock, smash, etc
    public virtual void EnableDevice()
    {

    }

    // perform the device's main function
    public virtual void ActivateDevice()
    {

    }

    // when objects can be un-activated, like closing a door
    public virtual void DeactiveDevice()
    {

    }

}
