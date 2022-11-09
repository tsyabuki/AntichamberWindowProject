using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowVisbilityTrigger : MonoBehaviour
{
    [Header("Settings")]
    public bool isVisible;
    [Space]
    [Header("References")]
    [SerializeField] private Camera visibilityTriggerCam;
    [SerializeField] private Renderer objRenderer;

    private void Awake()
    {
        //Get the visibility trigger cam reference
        visibilityTriggerCam = GameObject.FindGameObjectWithTag("VisibilityTriggerCamera").GetComponent<Camera>();

        if(visibilityTriggerCam == null)
        {
            Debug.Log(gameObject.name + " could not find a visibility trigger camera in the scene.");
        }

        objRenderer = GetComponent<Renderer>();

        if(objRenderer == null)
        {
            Debug.Log(gameObject.name + " could not find a renderer on itself in the scene.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isVisible = checkVisibility();
    }

    //Checks if the visibility trigger cam renders the trigger plane
    private bool checkVisibility()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(visibilityTriggerCam);
        if (GeometryUtility.TestPlanesAABB(planes, objRenderer.bounds))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
