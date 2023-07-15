using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.UI;
using static PlayerInput;
using UnityEngine.EventSystems;

public class PopupBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Dictionary<string, System.Action> actions;

    public PlayerInput optionsBoxInput; // clicking the right-click box
    private InputAction selectOption;
    private InputAction hoverPoint;

    Canvas canvas;
    GameObject displayedObject;
    Image panelImage;
    RectTransform panelRectTransform;

    float panelWidth = 100f;
    float panelHeight;

    float rowHeight = 10f;
    float padding = 1.0f;

    int numberOfRows;

    public void Awake()
    {
        optionsBoxInput = new PlayerInput();
        optionsBoxInput.Disable();
        MapInput();

        canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }

    public void MapInput()
    {
        // clicking option in dialog box
        selectOption = optionsBoxInput.optionsBox.select;
        hoverPoint = optionsBoxInput.optionsBox.hoverOn;
        selectOption.started +=
            context =>
            {
                SelectOption();
            };
    }
    public void SelectOption()
    {
        Debug.Log("Option chosen: " + GetOptionCell());
        string dictVal = displayedObject.transform.GetChild(GetOptionCell()).GetComponent<TextMeshProUGUI>().text;
        actions[dictVal].Invoke();

        DestroyPopupBox();
    }

    
    private int GetOptionCell()
    {
        // get mouse position 
        Vector2 mousePoint = hoverPoint.ReadValue<Vector2>();
        // inventory panel, depends on editor hierarchy order, should change, oh well for now
        RectTransform panel = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        // convert screen coordinates to Canvas panel coordinates
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(panel, mousePoint, null, out Vector2 pt))
        {
            // divide the whole size by slots # to get size of each, 5 items, so
            // 1200 / 5slots = 240 so, inventoryPoint / 240 gets the slot 
            

            // Debug.Log("cell selected: " + (int)Mathf.Floor(pt.y) / (int)rowHeight);


            // Reversing the numbers so it properly access the array at the 'opposite' index. Ie, 1 -> 5, 2 -> 4, 3 -> 3
            // CHAT GPT WOOOOOOOOO
            int index = (int)Mathf.Floor(pt.y) / (int)rowHeight;
            // Debug.Log("rows: " + numberOfRows);

            return numberOfRows - 1 - index;

        }
        Debug.Log("Error getting option cell");
        return -1;
    }


    public void CreateOptionsBox(Dictionary<string, System.Action> options, Vector2 pt)
    {
        // assign dictionary to global scope
        actions = options;

        // <ChatGPT4>
        Vector2 cornerLocation = pt;
        numberOfRows = options.Count + 1;

        GraphicRaycaster cast = gameObject.AddComponent<GraphicRaycaster>();

        displayedObject = new GameObject("ActionsPanel");
        displayedObject.transform.SetParent(canvas.transform);
        panelImage = displayedObject.AddComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 1f);

        panelRectTransform = displayedObject.GetComponent<RectTransform>();
        panelRectTransform.anchorMin = new Vector2(0f, 0f);
        panelRectTransform.anchorMax = new Vector2(0f, 0f);
        panelRectTransform.pivot = new Vector2(0f, 0f);
        panelRectTransform.anchoredPosition = cornerLocation;

        // Calculate the height of the panel based on the number of rows and row height
        panelHeight = numberOfRows * (rowHeight + padding);
        panelRectTransform.sizeDelta = new Vector2(panelWidth, panelHeight);


        // make Inspect detailed Examine
        // make Examine a default option, (later)
        // remember cancel 


        int i = 0;
        foreach (string command in options.Keys)
        {

            // make Text object and set as child of display panel
            GameObject textObject = new GameObject(command);
            textObject.transform.SetParent(displayedObject.transform);
            TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
            textComponent.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Arial SDF"); // Change the font path if necessary
            textComponent.fontSize = 14;
            textComponent.color = Color.white;
            // set Text text
            textComponent.text = (command);

            // set Text object's position
            RectTransform textRectTransform = textObject.GetComponent<RectTransform>();
            textRectTransform.anchorMin = new Vector2(0f, 1f);
            textRectTransform.anchorMax = new Vector2(0f, 1f);
            textRectTransform.pivot = new Vector2(0f, 1f);
            textRectTransform.anchoredPosition = new Vector2(padding, -padding - (rowHeight + padding) * i);
            i++;
            textRectTransform.sizeDelta = new Vector2(100f, rowHeight);

            
        }
        // </ChatGPT4>

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.parent.GetComponent<Player>().playerInput.Disable();
        optionsBoxInput.Enable();
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        DestroyPopupBox();
        gameObject.transform.parent.GetComponent<Player>().playerInput.Enable();
        optionsBoxInput.Disable();

    }

    public void DestroyPopupBox()
    {

        Destroy(displayedObject);

    }


}

