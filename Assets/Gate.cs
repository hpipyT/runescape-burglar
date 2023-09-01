using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Gate : Door, IOptionDisplayable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Dictionary<string, Action> GetActionsToDisplay()
    {
        Dictionary<string, Action> displayOptions = new Dictionary<string, Action>();

        displayOptions = base.GetActionsToDisplay();

        displayOptions["touch test"] = () =>
        {
            Debug.Log(examineText);
        };

        return displayOptions;
    }

    public override void OpenDoor()
    {
        Debug.Log("Gate opened");

        // get the component and translate it

        gameObject.transform.GetChild(0).rotation = Quaternion.Euler(0f, 360f, -90f);
    }

    public override void CloseDoor()
    {
        Debug.Log("Gate closed");

        gameObject.transform.GetChild(0).rotation = Quaternion.Euler(-90f, 360f, -90f);

    }

}
