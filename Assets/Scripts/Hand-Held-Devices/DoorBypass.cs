using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBypass : Device, IUseable
{
    private Door door;

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
        isGrabbable = true;
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
