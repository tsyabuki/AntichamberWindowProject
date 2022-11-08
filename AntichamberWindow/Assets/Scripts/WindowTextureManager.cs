using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowTextureManager : MonoBehaviour
{
    [SerializeField] private Camera cameraA;
    [SerializeField] private Camera cameraB;
    [SerializeField] private Material cameraMatA;
    [SerializeField] private Material cameraMatB;

    void Start()
    {
        if (cameraA.targetTexture != null)
        {
            cameraA.targetTexture.Release();
        }

        cameraA.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMatA.mainTexture = cameraA.targetTexture;

        if (cameraB.targetTexture != null)
        {
            cameraB.targetTexture.Release();
        }

        cameraB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMatB.mainTexture = cameraA.targetTexture;
    }
}
