using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ObjectGrab : MonoBehaviour
{
    public GameObject FinalUI;

    [System.Serializable]
    public class SnapElement
    {
        public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable sourceObject;
        public Rigidbody sourceRigidbody;
        public Transform destinationObject;
        public Transform relocationObject;
        public ParticleSystem snapEffect;
        public GameObject tmpText;   // TMP UI reference
        public GameObject infoPanel; // ✅ New info panel
        [HideInInspector] public bool isSnapped = false;

        // ✅ Store original position & rotation
        [HideInInspector] public Vector3 originalPosition;
        [HideInInspector] public Quaternion originalRotation;
    }

    public SnapElement[] elements;
    public float snapDistance = 0.2f;
    private int putCount = 0;
    private int currentActiveIndex = -1;

    void Start()
    {
        FinalUI.SetActive(false);

        foreach (var element in elements)
        {
            element.tmpText.gameObject.SetActive(false);
            if (element.infoPanel != null) 
                element.infoPanel.SetActive(false);

            // ✅ Save original transform at start
            element.originalPosition = element.sourceObject.transform.position;
            element.originalRotation = element.sourceObject.transform.rotation;
        }

        currentActiveIndex = Random.Range(0, elements.Length);

        elements[currentActiveIndex].tmpText.gameObject.SetActive(true);
        TMP_Text tmpTextComp = elements[currentActiveIndex].tmpText.GetComponent<TMP_Text>();
        StartCoroutine(TypeText(tmpTextComp, tmpTextComp.text));
    }

    void Update()
    {
        // ✅ Check if any object falls below -10 in Y
        foreach (var element in elements)
        {
            if (!element.isSnapped && element.sourceObject.transform.position.y < -10f)
            {
                // Reset position & rotation
                element.sourceObject.transform.position = element.originalPosition;
                element.sourceObject.transform.rotation = element.originalRotation;

                // Reset velocity so it doesn't keep falling
                if (element.sourceRigidbody != null)
                {
                    element.sourceRigidbody.velocity = Vector3.zero;
                    element.sourceRigidbody.angularVelocity = Vector3.zero;
                }
            }
        }
    }

    private void OnEnable()
    {
        foreach (var element in elements)
        {
            element.sourceObject.selectExited.AddListener((args) => OnRelease(element));
            if (element.snapEffect != null)
                element.snapEffect.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        foreach (var element in elements)
            element.sourceObject.selectExited.RemoveAllListeners();
    }

    private void OnRelease(SnapElement element)
    {
        if (element.isSnapped) return;

        if (currentActiveIndex < 0 || elements[currentActiveIndex] != element)
            return;

        float distance = Vector3.Distance(element.sourceObject.transform.position, element.destinationObject.position);

        if (distance <= snapDistance)
        {
            element.sourceObject.transform.position = element.relocationObject.position;
            element.sourceObject.transform.rotation = element.relocationObject.rotation;
            element.sourceObject.enabled = false;
            element.sourceRigidbody.isKinematic = true;
            element.isSnapped = true;

            if (element.snapEffect != null)
                StartCoroutine(PlaySnapEffect(element.snapEffect));

            if (element.infoPanel != null)
                element.infoPanel.SetActive(true);

            elements[currentActiveIndex].tmpText.gameObject.SetActive(false);

            List<int> unsnappedIndices = new List<int>();
            for (int i = 0; i < elements.Length; i++)
            {
                if (!elements[i].isSnapped)
                    unsnappedIndices.Add(i);
            }

            if (unsnappedIndices.Count > 0)
            {
                currentActiveIndex = unsnappedIndices[Random.Range(0, unsnappedIndices.Count)];
                elements[currentActiveIndex].tmpText.gameObject.SetActive(true);
                TMP_Text tmpTextComp = elements[currentActiveIndex].tmpText.GetComponent<TMP_Text>();
                StartCoroutine(TypeText(tmpTextComp, tmpTextComp.text));
            }
            else
            {
                currentActiveIndex = -1;
            }
        }
    }

    private IEnumerator PlaySnapEffect(ParticleSystem effect)
    {
        effect.gameObject.SetActive(true);
        effect.Play();
        yield return new WaitForSeconds(3f);
        effect.Stop();
        effect.gameObject.SetActive(false);

        putCount++;

        if (putCount == 7)
        {
            Debug.Log("You Won");
            FinalUI.SetActive(true);
            Invoke("ExitGame", 2f);
        }
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Starting Scene");
    }

    private IEnumerator TypeText(TMP_Text tmpText, string fullText, float delay = 0.05f)
    {
        tmpText.text = "";

        foreach (char c in fullText)
        {
            tmpText.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
}
