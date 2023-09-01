using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Item : MonoBehaviour
{

    [SerializeField]
    public Sprite icon;

    public bool isGrabbable = false;

    public String examineText;

    public Dictionary<string, System.Action> AddBaseActions(Dictionary<string, System.Action> actions)
    {
        actions.Add("Examine", () => Debug.Log(examineText));


        if (isGrabbable)
        {
            actions.Add("Pickup", () => GameObject.Find("Player").GetComponent<Player>().AddToInventory(this));
        }


        return actions;

    }

}
