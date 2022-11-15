using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool canMoveMouse = true;
    [SerializeField] private float mouseSensitivity = 100;
    [SerializeField] private LayerMask lookTriggerLayermask;
    [Space]
    [Header("References")]
    [SerializeField] private Camera playerCam;
    [SerializeField] private Transform playerBody;

    private float xRotation = 0f; //cameraRotation on the Y axis
    private WindowLookTrigger WLT = null; //The current window looking trigger the player is looking at

    private void Awake()
    {
        playerCam = gameObject.GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //toggleMouseMove();
        }

        if(canMoveMouse)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90, 90);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up, mouseX);
        }

        //Draw the rays for the window triggers
        checkLookTriggers();
    }

    //Check if looking at a look trigger. Update it whether they're looking or not looking.
    private void checkLookTriggers()
    {
        //First check to see if the center of a character's view model hits a look trigger
        Ray rayCenter = playerCam.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2));
        Debug.DrawRay(rayCenter.origin, rayCenter.direction * 10, Color.yellow);
        RaycastHit rayCenterHit;
        //If it does, set the look trigger is looking to true, check the edges of the player's view to see if they're full view is on the look trigger.
        if(Physics.Raycast(rayCenter, out rayCenterHit, 10, lookTriggerLayermask))
        {
            //Set the Window Looking Trigger if it's not already set
            if(WLT == null)
            {
                WLT = rayCenterHit.collider.gameObject.GetComponent<WindowLookTrigger>();
            }

            WLT.playerIsLooking = true;

            //Check rays at all 4 corners to see if they're all looking at the window look trigger
            bool cornerRays = true;

            //Bottom left
            Ray ray = playerCam.ScreenPointToRay(new Vector3(0, 0));
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            if(!Physics.Raycast(ray, 10, lookTriggerLayermask))
            {
                cornerRays = false;
            }

            //Bottom Right
            ray = playerCam.ScreenPointToRay(new Vector3(Screen.width, 0));
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            if (!Physics.Raycast(ray, 10, lookTriggerLayermask))
            {
                cornerRays = false;
            }

            //Top left
            ray = playerCam.ScreenPointToRay(new Vector3(0, Screen.height));
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            if (!Physics.Raycast(ray, 10, lookTriggerLayermask))
            {
                cornerRays = false;
            }

            //Top Right
            ray = playerCam.ScreenPointToRay(new Vector3(Screen.width, Screen.height));
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            if (!Physics.Raycast(ray, 10, lookTriggerLayermask))
            {
                cornerRays = false;
            }

            WLT.playerIsLookingFull = cornerRays;
        }
        else if(WLT != null)
        {
            WLT.playerIsLooking = false;
            WLT.playerIsLookingFull = false;
            WLT = null;
        }
    }

    private void toggleMouseMove()
    {
        canMoveMouse = !canMoveMouse;
    }
}


