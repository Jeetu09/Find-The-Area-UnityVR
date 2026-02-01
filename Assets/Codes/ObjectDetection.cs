using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetection : MonoBehaviour
{
    [System.Serializable]
    public class ObjectHolderAttachment
    {
        public GameObject obj;             // The object to check
        public Transform holderPoint;      // Holder reference point
        public GameObject attachmentObj;   // Where the object should attach
    }

    [Header("Object - Holder - Attachment Pairs")]
    public ObjectHolderAttachment[] objectSets;

    [Header("Settings")]
    public float attachDistance = 1f;   // Distance threshold (adjustable)

    void Update()
    {
        foreach (var set in objectSets)
        {
            if (set.obj == null || set.holderPoint == null || set.attachmentObj == null) 
                continue;

            // Compare object position with holder point
            float dist = Vector3.Distance(set.obj.transform.position, set.holderPoint.position);

            if (dist <= attachDistance)
            {
                // Attach object to its attachment object
                set.obj.transform.SetParent(set.attachmentObj.transform);
                set.obj.transform.position = set.attachmentObj.transform.position;
                set.obj.transform.rotation = set.attachmentObj.transform.rotation;
            }
        }
    }
}
