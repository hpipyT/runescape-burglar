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

    public Dictionary<string, System.Action> AddBaseActions(Dictionary<string, System.Action> actions)
    {


        if (isGrabbable)
        {
            actions.Add("Pickup", () => GameObject.Find("Player").GetComponent<Player>().AddToInventory(this));
        }

        return actions;

    }

}
