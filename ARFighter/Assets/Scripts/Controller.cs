using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public bool actorEnabled;
    public bool actorSpawned;
    public bool isPlaying;

    public UnityEngine.UI.Image guiRoot;

    public GameObject actor;
    public GameObject actorPlaceholder;
    public FighterController actorController;

    public GameObject actorPrefab;
    public GameObject trackerObj;
    public GameObject groundPlane;
    
    // Start is called before the first frame update
    void Start()
    {
        guiRoot.gameObject.SetActive(false);
        //imgTgt.RegisterTrackableEventHandler(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
        {
            if (actorEnabled && !actorPlaceholder && actorPrefab)
            {
                Instantiate(actorPrefab);
            }

            if (!actorEnabled && actorPlaceholder)
            {
                Destroy(actorPlaceholder);
                if (guiRoot && guiRoot.gameObject.activeInHierarchy)
                {
                    guiRoot.gameObject.SetActive(false);
                }
            }
        }
    }

    public void DisplayActor(FighterController actor)
    {
        actorController = actor;
        actorPlaceholder = actorController.gameObject;
        if (guiRoot && !guiRoot.gameObject.activeInHierarchy)
        {
            guiRoot.gameObject.SetActive(true);
        }
        actorEnabled = true;
    }

    public void NextActorPreview()
    {
        if (actorController)
        {
            //actorController.NextActor();
        }
    }

    public void PrevActorPreview()
    {
        if (actorController)
        {
            //actorController.PrevActor();
        }
    }

    public void StartGame()
    {
        //actorController.fighters[actorController.currActIdx].transform.SetParent(groundPlane.transform);
        isPlaying = true;
        actorEnabled = false;
    }

    /*public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
    
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            OnTrackingLost();
        }
        else
        {
            OnTrackingLost();
        }
    }

    private void OnTrackingFound()
    {
        if (!isPlaying)
        {
            actorController = Instantiate(actorPrefab).GetComponent<FighterController>();
            DisplayActor(actorController);
            actorController.transform.SetParent(trackerObj.transform);
            actorController.transform.localPosition = Vector3.zero;
            actorController.transform.localRotation = Quaternion.identity;
        }   
    }

    private void OnTrackingLost()
    {
        actorEnabled = false;
    }*/
}
