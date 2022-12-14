using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportWindow : MonoBehaviour
{
    [Header("Settings")]
    public bool canTeleport = true;
    [Space]
    [Header("References")]
    [SerializeField] private TeleportWindow otherWindow;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform mainCameraTransform;
    [SerializeField] private WindowStandingTrigger standTrigger;
    [SerializeField] private WindowLookTrigger lookTrigger;
    [SerializeField] private WindowVisbilityTrigger visibilityTrigger;
    [SerializeField] private GameObject displayPlane;
    public Camera windowCamera;

    private bool isVisible;
    private Vector3 playerPositionDelta; //The difference in position between the player and the current window
    private Vector3 cameraPositionDelta; //The difference in position between the main camera and the current window
    private Vector3 windowRotationDelta; //The difference in angle between this window and the other window.

    private void Awake()
    {
        //Get the player reference
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        mainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

        if (otherWindow == null)
        {
            Debug.Log("Cannot find opposite window reference in " + gameObject.name + ".");
        }

        if (mainCameraTransform == null)
        {
            Debug.Log("Window " + gameObject.name + " could not find a main camera in the scene.");
        }

        if (playerTransform == null)
        {
            Debug.Log("Player " + gameObject.name + " could not find a player in the scene.");
        }

        if (windowCamera == null)
        {
            Debug.Log("Could not find a camera reference in " + gameObject.name + ".");
        }

        if (standTrigger == null)
        {
            Debug.Log("Could not find a standing trigger reference in " + gameObject.name + ".");
        }

        if (visibilityTrigger == null)
        {
            Debug.Log("Could not find a visibility trigger reference in " + gameObject.name + ".");
        }

        if (lookTrigger == null)
        {
            Debug.Log("Could not find a look trigger reference in " + gameObject.name + ".");
        }
    }

    private void Update()
    {
        //Get the relative differences between objects
        getDeltas();

        //Create parity between the camera on the other side of the window
        setCameraParity();

        //Check the conditions for ren-enabling itself after the player has teleported to this window
        if(!canTeleport && (!visibilityTrigger.isVisible || checkPastWindow()))
        {
            canTeleport = true;
            displayPlane.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        bool isTeleport = checkTeleportConditions();
        //If all teleportation conditions are met, teleport
        if (isTeleport)
        {
            teleport();
        }
    }

    //Creates relative parity between this camera and the main camera relative to the opposite window
    private void setCameraParity()
    {
        //Set the other window's camera position based on the relative position
        Vector3 newCameraPosition = otherWindow.transform.TransformPoint(cameraPositionDelta);
        otherWindow.windowCamera.transform.position = newCameraPosition;
        //Sets the other window's camera rotation based on the player's camera
        Quaternion newOtherCameraRotation = mainCameraTransform.transform.rotation;
        newOtherCameraRotation.eulerAngles -= windowRotationDelta;
        otherWindow.windowCamera.transform.rotation = newOtherCameraRotation;
    }

    private void getDeltas()
    {
        playerPositionDelta = transform.InverseTransformPoint(playerTransform.position);
        cameraPositionDelta = transform.InverseTransformPoint(mainCameraTransform.position);
        windowRotationDelta = transform.rotation.eulerAngles - otherWindow.transform.rotation.eulerAngles;
    }

    //Returns true if all teleportation conditions are met
    private bool checkTeleportConditions()
    {
        if (canTeleport && standTrigger.isPlayerIn && lookTrigger.playerIsLookingFull)
        {
            standTrigger.isPlayerIn = false;
            lookTrigger.playerIsLookingFull = false;
            return true;
        }
        return false;
    }

    //Teleportation functionality
    private void teleport()
    {
        //Debug.Log("Teleporting from " + gameObject.name);

        //Disable teleportation on the other window
        otherWindow.canTeleport = false;

        //Disable the other window's display plane
        otherWindow.displayPlane.SetActive(false);

        //Set the new player position to the local position based on the player position delta
        Vector3 newPlayerPosition = otherWindow.transform.TransformPoint(playerPositionDelta);
        playerTransform.position = newPlayerPosition;

        //Rotate the player based on the difference in rotation
        playerTransform.Rotate(-windowRotationDelta);

        //Debug.Log("Teleport Complete");
    }

    //Checks if the player has walked past the current window
    private bool checkPastWindow()
    {
        if(playerPositionDelta.z < 0)
        {
            return true;
        }
        return false;
    }
}
