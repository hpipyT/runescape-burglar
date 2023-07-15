using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //public List<InventoryItem> items;
    public Item[] items;
    int numberSlots = 5;
    public bool inInventory;

    private PlayerInput inventoryActions;
    private InputAction leftClick;
    private InputAction rightClick;
    private InputAction hoverPoint;

    public Item selectedItem;

    void Awake()
    {
        MapInput();

        items = new Item[5];
        selectedItem = null;
    }

    private void MapInput()
    {
        inventoryActions = new PlayerInput();
        leftClick = inventoryActions.inventory.select;
        rightClick = inventoryActions.inventory.options;
        hoverPoint = inventoryActions.inventory.hoverOn;

        leftClick.started +=
            context =>
            {
                SelectItem(GetInventoryCell());
            };

        rightClick.started +=
            context =>
            {
                
                RightClickItem(GetInventoryCell());
                // get information about the item
            };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // get slot in inventory mouse hovers over
    private int GetInventoryCell()
    {
        // get mouse position 
        Vector2 mousePoint = hoverPoint.ReadValue<Vector2>();
        // inventory panel, depends on editor hierarchy order, should change, oh well for now
        RectTransform panel = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        // convert screen coordinates to Canvas panel coordinates
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(panel, mousePoint, null, out Vector2 inventoryPoint))
        {
            // divide the whole size by slots # to get size of each, 5 items, so
            // 1200 / 5slots = 240 so, inventoryPoint / 240 gets the slot 
            int slotSize = 1200 / numberSlots;

            return (int)Mathf.Floor(inventoryPoint.x) / slotSize;

        }
        Debug.Log("Error getting inventory cell");
        return -1;
    }

    public void RightClickItem(int cell)
    {
        if (items[cell].TryGetComponent(out IOptionDisplayable itemOptions))
        {
            PopupBox boxMaker = new PopupBox();
            Vector2 mousePoint = hoverPoint.ReadValue<Vector2>();

            inventoryActions.Disable();
            boxMaker.CreateOptionsBox(itemOptions.GetActionsToDisplay(), mousePoint);
        }
    }

    // RuneScape-style left click item in backpack
    public void SelectItem(int cell)
    {
            if (items[cell] != null)
            {
                // first check that item at the selected position exists then assign it
                selectedItem = items[cell];
                selectedItem.gameObject.SetActive(true);
                Debug.Log(selectedItem.name);
            }
    }

    public void AddToInventory(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        { 
            if (items[i] == null)
            {
                items[i] = Instantiate<Item>(item);
                items[i].transform.position = new Vector3(0, 0, -1000);

                // disable rendering for inventory instance
                foreach (Transform child in item.transform)
                {
                    // Disable the Mesh Renderer component on each child object
                    MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        meshRenderer.enabled = false;
                    }
                }

                Debug.Log("Value " + item.name + " populated in cell " + i);
                return;
            }
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Entered inventory space");
        // inInventory = true;
        gameObject.transform.parent.GetComponent<Player>().playerInput.Disable();
        inventoryActions.Enable();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Left inventory space");
        // inInventory = false;
        gameObject.transform.parent.GetComponent<Player>().playerInput.Enable();
        inventoryActions.Disable();
    }

    // get canvas
    // get camera it's attached to
    // set canvas distance to camera distance 

}
