using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class CameraAccess : Device, IOptionDisplayable
{
    public Camera Camera;

    public Dictionary<string, Action> GetActionsToDisplay()
    {
        Dictionary<string, Action> displayOptions = new Dictionary<string, Action>();



        return displayOptions;
    }
}
