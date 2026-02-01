using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR.Interaction.Toolkit.UI;

[RequireComponent(typeof(Animator))]
public class HandAnimator : MonoBehaviour
{
    [SerializeField] private InputActionReference controllerActionGrip;
    [SerializeField] private InputActionReference controllerActionTrigger;
    [SerializeField] private InputActionReference controllerActionPrimary;
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactors.XRPokeInteractor xrPokeInteractor;

    private bool isUIanimationPlaying = false;
    private Animator handAnimator = null;

    [SerializeField] private XRInputValueReader<Vector2> m_StickInput = new XRInputValueReader<Vector2>("Thumbstick");
    [SerializeField] private XRInputValueReader<float> m_TriggerInput = new XRInputValueReader<float>("Trigger");
    [SerializeField] private XRInputValueReader<float> m_GripInput = new XRInputValueReader<float>("Grip");

    private readonly List<Finger> grippingFingers = new List<Finger>()
    {
        new Finger(FingerType.Middle),
        new Finger(FingerType.Ring),
        new Finger(FingerType.Pinky)
    };

    private readonly List<Finger> uiFingers = new List<Finger>()
    {
        new Finger(FingerType.Thumb),
        new Finger(FingerType.Middle),
        new Finger(FingerType.Ring),
        new Finger(FingerType.Pinky),
    };

    private readonly List<Finger> pointingFingers = new List<Finger>()
    {
        new Finger(FingerType.Index)
    };

    private readonly List<Finger> primaryFingers = new List<Finger>()
    {
        new Finger(FingerType.Thumb)
    };

    private void Start()
    {
        handAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (controllerActionGrip != null)
        {
            controllerActionGrip.action.performed += GripAction_performed;
            controllerActionGrip.action.canceled += GripAction_canceled;
        }

        if (controllerActionTrigger != null)
        {
            controllerActionTrigger.action.performed += TriggerAction_performed;
            controllerActionTrigger.action.canceled += TriggerAction_canceled;
        }

        if (controllerActionPrimary != null)
        {
            controllerActionPrimary.action.performed += PrimaryAction_performed;
            controllerActionPrimary.action.canceled += PrimaryAction_canceled;
        }

        if (xrPokeInteractor != null)
        {
            xrPokeInteractor.uiHoverEntered.AddListener(ActiveUIhandPose);
            xrPokeInteractor.uiHoverExited.AddListener(DeactiveUIhandPose);
        }
    }

    private void OnDisable()
    {
        if (controllerActionGrip != null)
        {
            controllerActionGrip.action.performed -= GripAction_performed;
            controllerActionGrip.action.canceled -= GripAction_canceled;
        }

        if (controllerActionTrigger != null)
        {
            controllerActionTrigger.action.performed -= TriggerAction_performed;
            controllerActionTrigger.action.canceled -= TriggerAction_canceled;
        }

        if (controllerActionPrimary != null)
        {
            controllerActionPrimary.action.performed -= PrimaryAction_performed;
            controllerActionPrimary.action.canceled -= PrimaryAction_canceled;
        }

        if (xrPokeInteractor != null)
        {
            xrPokeInteractor.uiHoverEntered.RemoveListener(ActiveUIhandPose);
            xrPokeInteractor.uiHoverExited.RemoveListener(DeactiveUIhandPose);
        }
    }

    private void ActiveUIhandPose(UIHoverEventArgs Args0)
    {
        isUIanimationPlaying = true;
        SetFingerAnimationValues(uiFingers, 1);
        AnimateActionInput(uiFingers);
    }

    private void DeactiveUIhandPose(UIHoverEventArgs Args0)
    {
        isUIanimationPlaying = false;
        SetFingerAnimationValues(uiFingers, 0);
        AnimateActionInput(uiFingers);
    }

    private void GripAction_performed(InputAction.CallbackContext obj)
    {
        SetFingerAnimationValues(grippingFingers, 1.0f);
        AnimateActionInput(grippingFingers);
    }

    private void TriggerAction_performed(InputAction.CallbackContext obj)
    {
        SetFingerAnimationValues(pointingFingers, 1.0f);
        AnimateActionInput(pointingFingers);
    }

    private void PrimaryAction_performed(InputAction.CallbackContext obj)
    {
        SetFingerAnimationValues(primaryFingers, 1.0f);
        AnimateActionInput(primaryFingers);
    }

    private void GripAction_canceled(InputAction.CallbackContext obj)
    {
        SetFingerAnimationValues(grippingFingers, 0.0f);
        AnimateActionInput(grippingFingers);
    }

    private void TriggerAction_canceled(InputAction.CallbackContext obj)
    {
        SetFingerAnimationValues(pointingFingers, 0.0f);
        AnimateActionInput(pointingFingers);
    }

    private void PrimaryAction_canceled(InputAction.CallbackContext obj)
    {
        SetFingerAnimationValues(primaryFingers, 0.0f);
        AnimateActionInput(primaryFingers);
    }

    private void Update()
    {
        if (isUIanimationPlaying) return;

        if (m_StickInput != null)
        {
            var stickVal = m_StickInput.ReadValue();
            SetFingerAnimationValues(primaryFingers, stickVal.y);
            AnimateActionInput(primaryFingers);
        }

        if (m_TriggerInput != null)
        {
            var triggerVal = m_TriggerInput.ReadValue();
            SetFingerAnimationValues(pointingFingers, triggerVal);
            AnimateActionInput(pointingFingers);
        }

        if (m_GripInput != null)
        {
            var gripVal = m_GripInput.ReadValue();
            SetFingerAnimationValues(grippingFingers, gripVal);
            AnimateActionInput(grippingFingers);
        }
    }

    public void SetFingerAnimationValues(List<Finger> fingersToAnimate, float targetValue)
    {
        foreach (Finger finger in fingersToAnimate)
        {
            finger.target = targetValue;
        }
    }

    public void AnimateActionInput(List<Finger> fingersToAnimate)
    {
        if (handAnimator == null) return; // ✅ Prevent crash on scene change

        foreach (Finger finger in fingersToAnimate)
        {
            var fingerName = finger.type.ToString();
            var animationBlendValue = finger.target;
            handAnimator.SetFloat(fingerName, animationBlendValue);
        }
    }
}
