using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorBypass : Device, IOptionDisplayable
{
    private Door door;

    Dictionary<string, Action> IOptionDisplayable.GetActionsToDisplay()
    {
        Dictionary<string, Action> displayOptions = new Dictionary<string, Action>();


        displayOptions["Examine"] = () => // displayOptions comes from Item class
        {
            Debug.Log("Used to electronically breach doors");
        };


        return displayOptions;
    }

    public void Use()
    {
        Debug.Log("");
    }

    public void UseWith(Item item)
    {
        Debug.Log("");
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BindToDoor(Door door)
    {
        this.door = door;
    }


}
