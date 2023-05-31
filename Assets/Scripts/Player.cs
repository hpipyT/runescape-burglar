using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private GameObject character;


    private PlayerInput playerInput;

    private InputAction click;
    private InputAction mousePos;
    private NavMeshAgent agent;



    // Start is called before the first frame update
    void Start()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();

        MapInput();

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
                ClickScreen();
            };
    }

    void Update()
    {

    }

    private void ClickScreen()
    {
        // on click,
        // if in inventory screen 
        if (inventory.inInventory)
        {
            Debug.Log("Clicked inventory");
        }
        else
        {
            Vector2 pos = mousePos.ReadValue<Vector2>();
            RaycastHit hit;

            if (!Physics.Raycast(cam.ScreenPointToRay(pos), out hit, 100f))
                return;
            GameObject selection = hit.collider.gameObject;
            switch (selection.tag)
            {
                case "Ground":
                    MovePlayer(false);
                    break;
                case "Device":
                    MovePlayer(true);
                    selection.GetComponent<ControlledDevice>().TryDevice();
                    break;
                default:
                    break;
            };
        }
    }

/*    private void ClickScene() 
    {
        Vector2 pos = mousePos.ReadValue<Vector2>();
        RaycastHit hit;

        if (!Physics.Raycast(cam.ScreenPointToRay(pos), out hit, 100f))
            return;
        GameObject selection = hit.collider.gameObject;
        switch (selection.tag)
        {
            case "Ground":
                MovePlayer();
                break;
            case "Device":
                MovePlayer();
                selection.GetComponent<ControlledDevice>().TryDevice();
                break;
            default:
                break;
        }; 
    }*/

    private void MovePlayer(bool isDevice)
    {
        Vector2 pos = mousePos.ReadValue<Vector2>();

        // convert point on screen to point in world space
        if (Physics.Raycast(cam.ScreenPointToRay(pos), out RaycastHit hit, 100f))
        {
            // exception for navigating to devices
            if (isDevice)
            {
                agent.destination = hit.point;
                return;
            }
            // calculate path to point ; if path is incomplete set destination to farthest point
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(transform.position, hit.point, NavMesh.AllAreas, path))
            {
                agent.destination = path.corners[path.corners.Length - 1];
            }

        }
    }
}
