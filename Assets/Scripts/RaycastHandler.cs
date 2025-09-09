using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK; // Required for SpatialBridge

public class RaycastHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Your input trigger
        {
            // Get ray from Spatial.io camera
            Ray ray = SpatialBridge.cameraService.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float maxDistance = 100f;

            // Optional debug line
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red, 1.0f);

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                var uiElement = hit.collider.GetComponent<IUIRaycastable>();
                if (uiElement != null)
                {
                    uiElement.OnRaycastHit();
                }
            }
        }
    }
}

// Interface for interactable UI
public interface IUIRaycastable
{
    void OnRaycastHit();
}
