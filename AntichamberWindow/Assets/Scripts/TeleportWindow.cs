using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportWindow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TeleportWindow otherWindow;
    [SerializeField] private Transform mainCameraTransform;
    public Camera windowCamera;

    private Vector3 cameraPositionDelta; //The difference in position between the main camera and the current window
    private Vector3 windowRotationDelta; //The difference in angle between this window and the other window.

    private void Awake()
    {
        //Get the player reference
        mainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

        if(otherWindow == null)
        {
            Debug.Log("Cannot find opposite window reference in " + gameObject.name + ".");
        }

        if(mainCameraTransform == null)
        {
            Debug.Log("Window " + gameObject.name + " could not find a main camera in the scene.");
        }

        if(windowCamera == null)
        {
            Debug.Log("Could not find a camera reference in " + gameObject.name + ".");
        }
    }

    private void Update()
    {
        setCameraParity();
    }

    //Creates relative parity between this camera and the main camera relative to the opposite window
    private void setCameraParity()
    {
        //Get the relative differences between objects
        getDeltas();

        //Set the other window's camera position based on the relative position
        otherWindow.windowCamera.transform.localPosition = cameraPositionDelta;
        //Sets the other window's camera rotation based on the player's camera
        Quaternion newOtherCameraRotation = mainCameraTransform.transform.rotation;
        newOtherCameraRotation.eulerAngles += windowRotationDelta;
        otherWindow.windowCamera.transform.rotation = newOtherCameraRotation;
    }

    private void getDeltas()
    {
        cameraPositionDelta = mainCameraTransform.position - transform.position;
        windowRotationDelta = transform.rotation.eulerAngles - otherWindow.transform.rotation.eulerAngles;
    }
}
