using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEditor;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private Camera cam; // orbit camera

    [SerializeField]
    private GameObject character; // WIP

    // popup box
    [SerializeField]
    private PopupBox popupBox;

    public PlayerInput playerInput; // clicking the World

    private InputAction click;
    private InputAction rightClick;
    private InputAction mousePos;
    

    // navigation
    private NavMeshAgent agent;

    NavMeshPath path; // compare old path to new path to check if complete
    NavMeshPath oldPath;

    Vector2 rightClickPos;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();



        MapInput();

        // rightClickOptions = new OptionsBox();

    }


    // Start is called before the first frame update
    void Start()
    {
        mousePos = playerInput.player.move;
        cam = Camera.main;

        agent = character.GetComponent<NavMeshAgent>();
    }

    void Update()
    {

    }


    // left click and right click in World space
    private void MapInput()
    {
        click = playerInput.player.click;
        click.started +=
            context =>
            {
                // check what object was clicked
                Vector2 pos = mousePos.ReadValue<Vector2>();
                RaycastHit hit;

                if (!Physics.Raycast(cam.ScreenPointToRay(pos), out hit, 100f))
                    return;

                Click(hit.collider.gameObject);
            };

        rightClick = playerInput.player.rightClick;
        rightClick.started +=
            context =>
            {
                // check what object was rightClicked
                Vector2 pos = mousePos.ReadValue<Vector2>();
                rightClickPos = pos;
                RaycastHit hit;

                if (!Physics.Raycast(cam.ScreenPointToRay(pos), out hit, 100f))
                    return;

                // actions for the item
                Dictionary<string, System.Action> actions = GetActions(hit.collider.gameObject);

                // avoid duplicates
                if(!popupBox.GetComponentInChildren<Image>())
                    popupBox.CreateOptionsBox(actions, pos);

            };
    }

    // creates dictionary of strings and functions that a player can take on selected object
    private Dictionary<string, System.Action> GetActions(GameObject selection)
    {
        Vector2 pos = mousePos.ReadValue<Vector2>();
        Dictionary<string, System.Action> actions = new Dictionary<string, System.Action>();
        switch (selection.tag)
        {
            // environment
            case "Ground":
                // Move here
                
                actions.Add("Move Here", () => MovePlayer(pos));
                // Cancel

                return actions;

            case "Device":

                // get object's display options
                if (selection.TryGetComponent(out IOptionDisplayable display))
                {
                    actions = display.GetActionsToDisplay();
                    selection.GetComponent<Item>().AddBaseActions(actions);
                    actions.Add("Move Here", () => MovePlayer(pos));
                };

                // if an inventory item is selected, add "use with selected item" option
                if (inventory.selectedItem != null)
                {
                    if (selection.TryGetComponent(out IUseable use))
                    {
                        actions.Add("Use with " + inventory.selectedItem.name, () =>
                        {
                            //actions.Add("Move Here", () => MovePlayer(rightClickPos));
                            MovePlayer(rightClickPos);
                            ArriveThenExecute((item) => use.UseWith(inventory.selectedItem), inventory.selectedItem); 
                        });
                    }
                }

                return actions;

            default:
                return actions;
        };
    }

    // pass a function that takes a parameter, as a parameter to this function
    // will wait until the player agent arrives at the destination before executing
    private void ArriveThenExecute<T>(System.Action<T> func, T selection)
    {
        StartCoroutine(ArriveThenExecuteHelper(func, selection));
    }

    private IEnumerator ArriveThenExecuteHelper<T>(System.Action<T> func, T selection)
    {
        Debug.Log("In coroutine");
        oldPath = path;
        if (path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
        {
            Debug.Log("Broken path to destination");
            yield break;
        }

        // wait for agent to reach the device before activating it
        // answers.unity.com/questions/324589/how-can-i-tell-when-a-navmesh-has-reached-its-dest.html
        while (true)
        {
            // case when player clicks an object then before reaching it changes paths
            if (oldPath != path)
            {
                yield return null;
                break;
            }

            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        Debug.Log("Item: " + selection);
                        func.Invoke(selection);
                        yield return null;
                        break;
                    }
                }
            }
            yield return null;
        }

        yield return null;
    }

    // left clicking various game elements
    private void Click(GameObject selection)
    {



        // cases for types of clicked object
        switch (selection.tag)
        {
            case "Ground":
                MovePlayer(mousePos.ReadValue<Vector2>());

                break;
            case "Device":
                MovePlayer(mousePos.ReadValue<Vector2>());
                InteractDevice(selection);
                break;

            default:
                // loose items
                if (selection.TryGetComponent(out Item item))
                {
                    if (item.isGrabbable)
                        inventory.AddToInventory(selection.GetComponent<Item>());
                }
                break;
        };
    }



    // returns true if player clicked on accessible destination
    private bool MovePlayer(Vector2 pos)
    {
        Debug.Log("Moving to " + pos);
        // convert point on screen to point in world space
        if (Physics.Raycast(cam.ScreenPointToRay(pos), out RaycastHit hit, 100f))
        {
            // calculate path to point ; if path is incomplete set destination to farthest point
            path = new NavMeshPath();

            if (NavMesh.CalculatePath(transform.position, hit.point, NavMesh.AllAreas, path))
            {
                if (path.status == NavMeshPathStatus.PathPartial)
                {
                    agent.destination = path.corners[path.corners.Length - 1];
                    return false;
                }
                else
                    agent.destination = hit.point;

                return true;
            }

        }
        return false;
    }

    private void InteractDevice(GameObject selection)
    {
        StartCoroutine(InteractDeviceHelper(selection));
    }

    private IEnumerator InteractDeviceHelper(GameObject selection) 
    {
        // if device can't be reached, no interact
        oldPath = path;
        if (path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
        {
            Debug.Log("Broken path to device");
            yield break;
        }

        // wait for agent to reach the device before activating it
        // answers.unity.com/questions/324589/how-can-i-tell-when-a-navmesh-has-reached-its-dest.html
        while (true)
        {
            // case when player clicks an object then before reaching it changes paths
            if (oldPath != path)
            {
                yield return null;
                break;
            }

            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        Debug.Log("reached destination");

                        Item item = inventory.selectedItem;

                        // picking up loose device
                        if (selection.GetComponent<Item>().isGrabbable == true)
                        {
                            inventory.AddToInventory(selection.GetComponent<Item>());
                            StartCoroutine(waita());
                            Destroy(selection);
                            break;
                        }

                        // use item on device
                        if (item != null && selection.TryGetComponent(out IUseable tryableDeviceWithObject))
                        {
                            tryableDeviceWithObject.UseWith(item);
                            inventory.selectedItem = null;
                        }

                        // use device
                        else if (selection.TryGetComponent(out IUseable tryableDevice))
                        {
                            tryableDevice.Use();
                        }

                        yield return null;
                        break;
                    }
                }
            }
            yield return null;
        }

        yield return null;
    }

    public IEnumerator waita()
    {
        yield return new WaitForSeconds(1.0f);
    }

    public void AddToInventory(Item item)
    {
        MovePlayer(rightClickPos);
        InteractDevice(item.gameObject);
    }
}
