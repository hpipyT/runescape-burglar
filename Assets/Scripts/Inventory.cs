using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public List<GameObject> items;

    public bool inInventory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToInventory(GameObject item)
    {
        items.Add(item);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Entered inventory space");
        inInventory = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Left inventory space");
        inInventory = false;
    }

    // get canvas
    // get camera it's attached to
    // set canvas distance to camera distance 

}
