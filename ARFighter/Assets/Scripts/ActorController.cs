using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public FighterController[] fighters;
    public GameObject selectorScreen;
    public GameObject controlScreen;
    public GameObject healthBar;
    public int currActIdx = 0;

    private Collider floor;
    private GameObject currentSpawned;
    private FighterController player;
    private JoystickMovement StickL;
    private JoystickMovement StickR;
    // Start is called before the first frame update
    void Start()
    {
        fighters = GetComponentsInChildren<FighterController>();
        StickL = controlScreen.transform.Find("MainStick").GetComponent<JoystickMovement>();
        StickR = controlScreen.transform.Find("AttControl").GetComponent<JoystickMovement>();
        foreach(Transform t in transform)
        {
            if(t.tag == "Floor")
            {
                floor = t.GetComponent<Collider>();
            }
        }
        controlScreen.SetActive(false);
        //selectorScreen.SetActive(true);
        //selectorScreen.transform.SetParent(null);
        //controlScreen.transform.SetParent(null);
        SetPreview(currActIdx);
    }

    private void Awake()
    {
        if (!currentSpawned)
        {
            selectorScreen.SetActive(true);
        }
    }

    void Update()
    {
        if (!currentSpawned && !fighters[currActIdx].gameObject.activeInHierarchy)
        {
            fighters[currActIdx].gameObject.SetActive(true);
            floor.transform.SetParent(transform);
            floor.transform.localPosition = Vector3.zero;
            floor.transform.localRotation = Quaternion.identity;
            controlScreen.transform.SetParent(transform.parent);
            controlScreen.transform.localPosition = Vector3.zero;
            controlScreen.SetActive(false);
            selectorScreen.SetActive(true);
        }

    }

    private void FixedUpdate()
    {
        if (currentSpawned)
        {
            player.stickH = StickL.HorizontalInput();
            player.stickV = StickL.VerticalInput();
            player.yaw = StickR.HorizontalInput();

            if (floor)
            {
                SetFloor();
            }
        }
    }

    void SetPreview(int actorIdx)
    {
        for(int i=0; i < fighters.Length; i++)
        {
            fighters[i].gameObject.SetActive(false);
        }

        fighters[actorIdx].gameObject.SetActive(true);
    }

    void SetFloor()
    {
        floor.transform.position = new Vector3(currentSpawned.transform.position.x, floor.transform.position.y, currentSpawned.transform.position.z);
        floor.transform.rotation = Quaternion.identity;
    }

    public void NextActor()
    {
        currActIdx++;
        if(currActIdx > (fighters.Length - 1))
        {
            currActIdx = 0;
        }
        SetPreview(currActIdx);
    }

    public void PrevActor()
    {
        currActIdx--;
        if(currActIdx < 0)
        {
            currActIdx = fighters.Length - 1;
        }
        SetPreview(currActIdx);
    }

    public void OnFire()
    {
        if (player)
        {
            player.Firing();
        }
    }

    public FighterController SpawnActor(Transform parent)
    {
        InitFighter(parent);
        return currentSpawned.GetComponent<FighterController>();
    }

    public void InitFighter(Transform parent)
    {
        currentSpawned = Instantiate(fighters[currActIdx].gameObject, parent);
        currentSpawned.transform.position = fighters[currActIdx].transform.position;
        currentSpawned.transform.rotation = fighters[currActIdx].transform.rotation;
        currentSpawned.transform.localScale = Vector3.one * 0.2f;

        player = currentSpawned.GetComponent<FighterController>();
        player.healthIndicator = Instantiate(healthBar, currentSpawned.transform).GetComponent<HealthIndicator>();
        player.healthIndicator.player = currentSpawned.transform.Find("Indicator");
        player.isActive = true;

        Rigidbody sr = currentSpawned.GetComponent<Rigidbody>();
        sr.useGravity = true;
        sr.isKinematic = false;

        SetFloor();
        floor.transform.SetParent(null);
        currentSpawned.SetActive(true);

        fighters[currActIdx].gameObject.SetActive(false);
        selectorScreen.SetActive(false);
        controlScreen.SetActive(true);
        controlScreen.transform.SetParent(null);
    }
}
