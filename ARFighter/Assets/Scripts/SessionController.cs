using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class SessionController : MonoBehaviour
{
    public GameObject[] arObjectsToPlace;
    public string[] arObjKeys;
    public GameObject selectionScreen;
    public GameObject controllerScreen;
    public UnityEngine.UI.Text terminal;

    private bool isPlaying = false;
    private ARTrackedImageManager imageManager;
    private Dictionary<string, GameObject> arPrefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();
    private ActorController actor;
    private FighterController player;

    private JoystickMovement touchStickLeft;
    private JoystickMovement touchStickRight;
    private UnityEngine.UI.Text[] terminals;
    // Start is called before the first frame update
    private void Awake()
    {
        imageManager = GetComponent<ARTrackedImageManager>();
        terminals = terminal.transform.parent.GetComponentsInChildren<UnityEngine.UI.Text>();
        foreach(UnityEngine.UI.Text t in terminals)
        {
            t.text = "";
        }
        selectionScreen.SetActive(false);
        controllerScreen.SetActive(false);
        touchStickLeft = controllerScreen.transform.Find("MainStick").GetComponent<JoystickMovement>();
        touchStickRight = controllerScreen.transform.Find("AttControl").GetComponent<JoystickMovement>();
        for (int i = 0; i < arObjKeys.Length; i++)
        {
            arPrefabs[arObjKeys[i]+"_target"] = arObjectsToPlace[i];
        }
    }

    private void Update()
    {
        if(player && isPlaying)
        {
            player.stickH = touchStickLeft.HorizontalInput();
            player.stickV = touchStickLeft.VerticalInput();
            player.yaw = touchStickRight.HorizontalInput();
            terminals[1].text = "Input Stick H : " + touchStickLeft.HorizontalInput();
            terminals[2].text = "Input Stick V : " + touchStickLeft.VerticalInput();
            terminals[3].text = "Fighter Attitude : " + player.transform.localEulerAngles;
            terminals[4].text = "Fighter Location : " + player.transform.position;
        }

        if(!player && isPlaying)
        {
            isPlaying = false;
        }
    }

    private void OnEnable()
    {
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            if (trackedImage.referenceImage.name.Contains("fighters") && !isPlaying)
            {
                selectionScreen.SetActive(true);
            }
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            if (trackedImage.referenceImage.name.Contains("fighters"))
            {
                selectionScreen.SetActive(false);
            }
            arObjects[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateARImage(ARTrackedImage trackedImage)
    {
        if (terminal)
        {
            //terminal.text = trackedImage.referenceImage.name;
        }
        // Assign and Place Game Object
        AssignGameObject(trackedImage.referenceImage.name, trackedImage.transform);
    }

    private void AssignGameObject(string trackedName, Transform newTransform)
    {
        if (!arObjects.ContainsKey(trackedName))
        {
            arObjects[trackedName] = Instantiate(arPrefabs[trackedName]);
            arObjects[trackedName].transform.SetParent(transform);
            if (trackedName.Contains("fighters"))
            {
                actor = arObjects[trackedName].GetComponent<ActorController>();
            }
        }

        GameObject goARObject = arObjects[trackedName];
        goARObject.SetActive(true);
        goARObject.transform.position = newTransform.position;
        goARObject.transform.rotation = newTransform.rotation;
    }

    public void OnNextPreview()
    {
        terminal.text = "Request change actor";
        if (!actor && arObjects.ContainsKey("fighters_target"))
        {
            actor = arObjects["fighters_target"].GetComponent<ActorController>();
        }

        if (actor)
        {
            terminal.text = "go to next actor";
            actor.NextActor();
        }
    }

    public void OnPrevPreview()
    {
        if (!actor && arObjects.ContainsKey("fighters_target"))
        {
            actor = arObjects["fighters_target"].GetComponent<ActorController>();
        }

        if (actor)
        {
            actor.PrevActor();
        }
    }

    public void OnStart()
    {
        terminal.text = "Spawn Actor";
        player = actor.SpawnActor(transform);
        player.isActive = true;
        isPlaying = true;
        controllerScreen.SetActive(true);
        selectionScreen.SetActive(false);
        terminal.text = "Actor Spawned";
    }
}
