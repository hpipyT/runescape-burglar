using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBypass : ControlledDevice
{
    private Door door;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActivateDevice()
    {
        // unlock the door
        door.EnableDevice();
    }

    public void BindToDoor(Door door)
    {
        this.door = door;
    }
}
