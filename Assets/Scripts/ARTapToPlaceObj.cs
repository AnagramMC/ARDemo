using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

public class ARTapToPlaceObj : MonoBehaviour
{
    private GameObject placementIndicator;
    public GameObject placementObject;

    private ARRaycastManager raycastManager;
    
    void Start()
    {

        //get the component
        raycastManager = FindObjectOfType<ARRaycastManager>();
        placementIndicator = transform.GetChild(0).gameObject;

        //hide the visual placement
        placementIndicator.SetActive(false);
    }

    void Update()
    {
        UpdatePlacementPose();
    }

    private void UpdatePlacementPose()
    {
        Vector3 screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            transform.position = hits[0].pose.position;

            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

            transform.rotation = Quaternion.LookRotation(cameraBearing);

            if (!placementIndicator.activeInHierarchy)
            {
                placementIndicator.SetActive(true);
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                PlaceObject();
            }

        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void PlaceObject()
    {
        Instantiate(placementObject, transform.position, transform.rotation);
    }
}
