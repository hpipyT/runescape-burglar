using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEditor;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private GameObject character;


    public PlayerInput playerInput;

    private InputAction click;
    private InputAction rightClick;
    private InputAction mousePos;

    private NavMeshAgent agent;
    NavMeshPath path;
    NavMeshPath oldPath;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();
        MapInput();
    }


    // Start is called before the first frame update
    void Start()
    {
        mousePos = playerInput.player.move;
        cam = Camera.main;

        agent = character.GetComponent<NavMeshAgent>();
    }

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
                RaycastHit hit;

                if (!Physics.Raycast(cam.ScreenPointToRay(pos), out hit, 100f))
                    return;

                RightClick(hit.collider.gameObject);                
            };
    }

    void Update()
    {

    }

    // left clicking various game elements
    private void Click(GameObject selection)
    {


        // cases for types of clicked object
        switch (selection.tag)
        {
            case "Ground":
                MovePlayer();

                break;
            case "Device":
                MovePlayer();
                StartCoroutine(InteractDevice(selection));
                break;
            default:
                break;
        };
    }


    private void RightClick(GameObject selection)
    {
        switch (selection.tag)
        {
            case "Ground":
                MovePlayer();
                break;
            case "Device":
                MovePlayer();
                StartCoroutine(InteractDevice(selection));
                break;
            case "LooseDevice":
                // may have to change this later
                inventory.AddToInventory(selection.transform.parent.GetComponent<Item>());
                Destroy(selection);
                StartCoroutine(waita());
                break;
            default:
                break;
        };
    }
    
    private IEnumerator waita()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log(inventory.items[0]);
    }

    // returns true if player clicked on accessible destination
    private bool MovePlayer()
    {
        Vector2 pos = mousePos.ReadValue<Vector2>();

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


    private IEnumerator InteractDevice(GameObject selection) 
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
                        yield return null;
                        break;
                    }
                }
            }
            yield return null;
        }

        yield return null;
    }



    private void RightClickItem()
    {
        Debug.Log("Arrived");
    }
}
