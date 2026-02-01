using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections.Generic;

[System.Serializable]
public class GrabInteractionData
{
    public GameObject uiObject;
    public string objectTag;
    public string glowTag;
    public GameObject discObject;
    public Material discGlowMaterial;
    public GameObject pressEObjectDisc;
    public GameObject pressEObjectGrabbable;
    public Transform snapTargetOnGrab;
    public Transform snapTargetOnDisc;
    public GameObject informationUI;
    public GameObject hintUI; // NEW: Hint UI
}

public class ObjectGlowOutline : MonoBehaviour
{
    [Header("Final UI")]
    public GameObject CongratulationsUI;
    public MonoBehaviour cameraScript;
    public MonoBehaviour playerMovementScript;
    private bool isMenuActive = false;

    [Header("Interaction Data Set")]
    public List<GrabInteractionData> interactionData;

    [Header("References")]
    public GameObject player;
    public Rig sharedRig;
    public float interactionDistance = 2f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Glow Animation Settings")]
    public float glowScaleAmplitude = 0.1f;
    public float glowScaleSpeed = 2f;

    [Header("Press E Animation Settings")]
    public float pressEFloatAmplitude = 0.1f;
    public float pressEFloatSpeed = 2f;

    private int currentIndex = -1;
    private GameObject currentGrabbable;
    private GameObject heldObject;
    private bool isObjectGrabbed = false;
    private List<int> usedIndices = new List<int>();
    private HashSet<GameObject> placedObjects = new HashSet<GameObject>();

    private float hintDisplayTime = 5f;
    private float hintTimer = 0f;
    private bool isHintVisible = false;

    void Start()
    {
        InitializeGlowObjects();
        PickRandomInteraction();

        CongratulationsUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraScript.enabled = true;
        playerMovementScript.enabled = true;
    }

    void Update()
    {
        if (currentIndex < 0 || currentIndex >= interactionData.Count) return;

        var data = interactionData[currentIndex];

        AnimatePressE(data.pressEObjectDisc);
        AnimatePressE(data.pressEObjectGrabbable);

        GameObject[] candidates = GameObject.FindGameObjectsWithTag(data.objectTag);
        List<GameObject> unplaced = new List<GameObject>();
        foreach (var obj in candidates)
        {
            if (!placedObjects.Contains(obj))
                unplaced.Add(obj);
        }

        currentGrabbable = GetNearestObject(unplaced.ToArray(), player.transform.position);

        float grabbableDistance = currentGrabbable != null
            ? Vector3.Distance(player.transform.position, currentGrabbable.transform.position)
            : float.MaxValue;

        bool isNearGrabbable = currentGrabbable != null && grabbableDistance <= interactionDistance;

        data.pressEObjectGrabbable.SetActive(!isObjectGrabbed && isNearGrabbable);
        AnimateGlowObjects(data, !isObjectGrabbed && isNearGrabbable);

        // Grabbing
        if (!isObjectGrabbed && isNearGrabbable && Input.GetKeyDown(interactKey))
        {
            data.pressEObjectGrabbable.SetActive(false);
            heldObject = currentGrabbable;
            heldObject.transform.SetParent(data.snapTargetOnGrab);
            SnapObject(heldObject, data.snapTargetOnGrab);
            isObjectGrabbed = true;
            if (sharedRig) sharedRig.weight = 1f;

            if (data.informationUI != null)
                data.informationUI.SetActive(true);
        }

        // Placing
        if (isObjectGrabbed)
        {
            float distToDisc = Vector3.Distance(player.transform.position, data.discObject.transform.position);
            bool isNearDisc = distToDisc <= interactionDistance;

            data.pressEObjectDisc.SetActive(isNearDisc);

            if (isNearDisc && Input.GetKeyDown(interactKey))
            {
                data.pressEObjectDisc.SetActive(false);
                heldObject.transform.SetParent(null);
                SnapObject(heldObject, data.snapTargetOnDisc);
                ApplyGlowMaterial(data.discObject, data.discGlowMaterial);
                placedObjects.Add(heldObject);

                if (data.informationUI != null)
                    data.informationUI.SetActive(false);

                heldObject = null;
                isObjectGrabbed = false;
                if (sharedRig) sharedRig.weight = 0f;

                Invoke(nameof(PickRandomInteraction), 1.5f);
            }
        }

        // Show Hint UI
        if (Input.GetKeyDown(KeyCode.H) && data.hintUI != null)
        {
            data.hintUI.SetActive(true);
            hintTimer = hintDisplayTime;
            isHintVisible = true;
        }

        // Hide Hint UI after timeout
        if (isHintVisible)
        {
            hintTimer -= Time.deltaTime;
            if (hintTimer <= 0f)
            {
                if (data.hintUI != null)
                    data.hintUI.SetActive(false);
                isHintVisible = false;
            }
        }
    }

    void InitializeGlowObjects()
    {
        foreach (var data in interactionData)
        {
            GameObject[] glowObjects = GameObject.FindGameObjectsWithTag(data.glowTag);
            foreach (var glow in glowObjects)
            {
                glow.transform.localScale = Vector3.one;
                glow.SetActive(true);
            }

            if (data.informationUI != null)
                data.informationUI.SetActive(false);

            if (data.hintUI != null)
                data.hintUI.SetActive(false);
        }
    }

    void PickRandomInteraction()
    {
        if (interactionData.Count == 0 || usedIndices.Count == interactionData.Count)
        {
            Debug.Log("All interactions completed.");

            CongratulationsUI.SetActive(true);
            isMenuActive = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            cameraScript.enabled = false;
            playerMovementScript.enabled = false;
            return;
        }

        int newIndex;
        do
        {
            newIndex = Random.Range(0, interactionData.Count);
        } while (usedIndices.Contains(newIndex));

        usedIndices.Add(newIndex);
        currentIndex = newIndex;

        for (int i = 0; i < interactionData.Count; i++)
        {
            bool isActive = i == currentIndex;
            interactionData[i].uiObject.SetActive(isActive);
            interactionData[i].pressEObjectDisc.SetActive(false);
            interactionData[i].pressEObjectGrabbable.SetActive(false);

            if (interactionData[i].informationUI != null)
                interactionData[i].informationUI.SetActive(false);

            if (interactionData[i].hintUI != null)
                interactionData[i].hintUI.SetActive(false);
        }

        isHintVisible = false;
        hintTimer = 0f;
    }

    void SnapObject(GameObject obj, Transform target)
    {
        obj.transform.SetPositionAndRotation(target.position, target.rotation);
    }

    void ApplyGlowMaterial(GameObject disc, Material glowMat)
    {
        var renderer = disc.GetComponent<Renderer>();
        if (renderer) renderer.material = glowMat;
    }

    GameObject GetNearestObject(GameObject[] objects, Vector3 from)
    {
        GameObject nearest = null;
        float minDist = float.MaxValue;

        foreach (var obj in objects)
        {
            float dist = Vector3.Distance(obj.transform.position, from);
            if (dist < minDist)
            {
                nearest = obj;
                minDist = dist;
            }
        }

        return nearest;
    }

    void AnimateGlowObjects(GrabInteractionData data, bool shouldGlow)
    {
        GameObject[] glowObjects = GameObject.FindGameObjectsWithTag(data.glowTag);

        foreach (var glow in glowObjects)
        {
            if (shouldGlow)
            {
                glow.SetActive(true);
                float pulse = 1f + Mathf.Sin(Time.time * glowScaleSpeed) * glowScaleAmplitude;
                glow.transform.localScale = Vector3.one * pulse;
            }
            else
            {
                glow.SetActive(true);
                glow.transform.localScale = Vector3.one;
            }
        }
    }

    void AnimatePressE(GameObject pressEObject)
    {
        if (pressEObject == null || !pressEObject.activeSelf) return;

        Vector3 originalPos = pressEObject.transform.position;
        float offsetY = Mathf.Sin(Time.time * pressEFloatSpeed) * pressEFloatAmplitude;
        pressEObject.transform.position = new Vector3(
            originalPos.x,
            originalPos.y + offsetY,
            originalPos.z
        );
    }
}
