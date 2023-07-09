using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Use a device on another device
public interface IUseable
{
    void Use();
    void UseWith(Item gameObject);    
}
