using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // item has a sprite
    // has a behavior
    // can be an access device
    // can be an accessed device

    public Inventory inventory;

    private Sprite icon;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Use()
    {
    }

    public void RightClick()
    {

    }

    void Examine()
    {

    }

}
