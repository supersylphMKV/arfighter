using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;

public class GroundDetect : MonoBehaviour
{
    public GameObject placementIndicator;
    public Camera mainCam;

    private ARSessionOrigin arOrigin;
    private ARRaycastManager arRayManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private Pose placementPose;
    private bool isValidGround;
    // Start is called before the first frame update
    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        arRayManager = arOrigin.GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    void UpdatePlacementPose()
    {
        if (isValidGround)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        } else
        {
            placementIndicator.SetActive(false);
        }
    }

    void UpdatePlacementIndicator()
    {
        Vector3 centerScreen = mainCam.ViewportToScreenPoint(Vector2.one * 0.5f);
        hits.Clear();
        arRayManager.Raycast(centerScreen, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        isValidGround = hits.Count > 0;

        if (isValidGround)
        {
            placementPose = hits[0].pose;
        }
    }
}
