using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO;
using UnityEngine.InputSystem;

public class MobileCamera : MonoBehaviour
{
    public GameObject character;

    Vector3 characterPos;
    Vector3 camPos;

    Vector3 selfieStick;

    private PlayerInput PlayerInput;
    private InputAction orbit;
    private InputAction zoom;

    private void Awake()
    {
        PlayerInput = new PlayerInput();
        PlayerInput.Enable();

        orbit = PlayerInput.camera.orbit;
        zoom = PlayerInput.camera.zoom;
    } 

    // Start is called before the first frame update
    void Start()
    {
        // set camera to starting distance and angle from player
        camPos = gameObject.transform.position;
        gameObject.transform.LookAt(characterPos); 

        characterPos = character.transform.position;
    }

    void Update()
    {
        // camera controls
        OrbitCamera(orbit.ReadValue<Vector2>());
        ZoomCamera(zoom.ReadValue<float>());

        camPos = gameObject.transform.position;

        // the vector distance between player and cam
        selfieStick = camPos - characterPos;
        // when player moves, the vector changes
        characterPos = character.transform.position;

        // if the distance between player and cam changed, transform cam position
        if (selfieStick != camPos - characterPos) 
        {
            camPos = selfieStick + characterPos;
            gameObject.transform.position = camPos;
        }

        gameObject.transform.position = camPos;
    }

    // a/d rotate around origin
    // w/s tilt camera up/down
    private void OrbitCamera(Vector2 dir)
    {
        // dir has x and y rotation arguments
        gameObject.transform.RotateAround(characterPos, Vector3.up, dir.x * 0.2f);
        gameObject.transform.RotateAround(characterPos, gameObject.transform.right, dir.y * 0.2f);
        // point camera at point
    }

    // zoom in/out camera
    private void ZoomCamera(float zoom)
    {
        float zoomSpeed = 0.25f;
        float dir = zoom / 120;

        gameObject.transform.position += gameObject.transform.forward * dir * zoomSpeed; 

    }

    
    // camera has to orbit
    
}
