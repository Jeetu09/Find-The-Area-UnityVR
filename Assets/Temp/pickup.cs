using UnityEngine;
using UnityEngine.InputSystem;  // Use new Input System namespace

public class pickup : MonoBehaviour
{
    [Header("Pickup Setup")]
    public Transform handBone;
    public GameObject[] pickupObjects;
    public Key pickupKey = Key.E;

    private GameObject currentHeldObject = null;
    private bool[] isPickedFlags;

    void Start()
    {
        isPickedFlags = new bool[pickupObjects.Length];
    }

    void Update()
    {
        if (Keyboard.current[pickupKey].wasPressedThisFrame)
        {
            if (currentHeldObject == null)
                TryPickup();
            else
                DropObject();
        }
    }

    void TryPickup()
    {
        float closestDist = Mathf.Infinity;
        int closestIndex = -1;

        for (int i = 0; i < pickupObjects.Length; i++)
        {
            if (isPickedFlags[i]) continue;

            float dist = Vector3.Distance(transform.position, pickupObjects[i].transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestIndex = i;
            }
        }

        if (closestIndex != -1)
        {
            currentHeldObject = pickupObjects[closestIndex];
            currentHeldObject.transform.SetParent(handBone);
            currentHeldObject.transform.localPosition = Vector3.zero;
            currentHeldObject.transform.localRotation = Quaternion.identity;
            isPickedFlags[closestIndex] = true;
        }
    }

    void DropObject()
    {
        if (currentHeldObject == null) return;

        for (int i = 0; i < pickupObjects.Length; i++)
        {
            if (pickupObjects[i] == currentHeldObject)
            {
                isPickedFlags[i] = false;
                break;
            }
        }

        currentHeldObject.transform.SetParent(null);
        currentHeldObject = null;
    }
}
