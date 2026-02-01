using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Collections;

public class GrabAnimationTrigger : MonoBehaviour
{
    public GameObject BlankScreen;
    public GameObject ticketMesh;

    [Header("Ticket Settings")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable ticket;  // assign ticket object
    public Animator gateOne;            
    public Animator gateTwo;            

    [Header("Ticket Object")]
    public GameObject TicketObject;

    [Header("Player Distance")]
    public Transform Player;
    public Transform Robot;
    public Animator animator;

    [Header("UI")]
    public GameObject RoboText;       // parent UI panel
    public TextMeshProUGUI DiaOne;    // first dialogue text
    public TextMeshProUGUI DiaTwo;    // second dialogue text
    public float typingSpeed = 0.05f;

    private bool hasTriggered = false;

    void Start()
    {
        BlankScreen.SetActive(true);
        RoboText.SetActive(false);
        DiaOne.gameObject.SetActive(false);
        DiaTwo.gameObject.SetActive(false);
    }

    void Update()
    {
        float distance = Vector3.Distance(Player.position, Robot.position);
        if (distance <= 3 && !hasTriggered)
        {
            hasTriggered = true;

            RoboText.SetActive(true);
            DiaOne.gameObject.SetActive(true);
            animator.SetTrigger("Open Eye");

            // Start typing DiaOne text
            StartCoroutine(TypeText(DiaOne, DiaOne.text));
        }
    }

    private void Awake()
    {
        if (ticket != null)
            ticket.selectEntered.AddListener(OnTicketGrabbed);
    }

    private void OnTicketGrabbed(SelectEnterEventArgs args)
    {
        if (gateOne != null) gateOne.SetTrigger("left trigger");
        if (gateTwo != null) gateTwo.SetTrigger("Right Trigger");
        BlankScreen.SetActive(false);

        Debug.Log("Ticket grabbed → Gates opening!");
        TicketObject.GetComponent<MeshRenderer>().enabled = false; // Hide
        ticketMesh.SetActive(false);
        // TicketObject.SetActive(false);


        // Hide first text and show second
        DiaOne.gameObject.SetActive(false);
        DiaTwo.gameObject.SetActive(true);

        // Start typing DiaTwo text
        StartCoroutine(TypeText(DiaTwo, DiaTwo.text));
    }

    private IEnumerator TypeText(TextMeshProUGUI textObj, string fullText)
    {
        textObj.text = ""; // clear text
        foreach (char c in fullText)
        {
            textObj.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
