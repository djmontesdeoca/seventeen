using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public enum HandType
{
    Left,
    Right
}
public class Hand : MonoBehaviour
{
    public HandType type = HandType.Left;
    public bool isHidden { get; private set; } = false;

    public InputAction trackedAction = null;

    public InputAction gripAction = null;

    public Animator handAnimator = null;

    public InputAction triggerAction = null;

    int m_gripAmtParam = 0;
    int m_pointAmtParam = 0;

    private bool m_isCurrentlyTracked = false;

    List<Renderer> m_currentRenderers = new List<Renderer>();

    Collider[] m_colliders = null;

    public bool isCollisionEnabled { get; private set; } = false;

    public XRBaseInteractor interactor = null;

    private void Awake()
    {
        if (!interactor)
        {
            interactor = GetComponentInParent<XRBaseInteractor>();
        }
    }

    private void OnDisable()
    {
        interactor.onSelectEntered.RemoveListener(OnGrab);
        interactor.onSelectExited.RemoveListener(OnRelease);

    }

    private void OnEnable()
    {
        interactor.onSelectEntered.AddListener(OnGrab);
        interactor.onSelectExited.AddListener(OnRelease);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_colliders = GetComponentsInChildren<Collider>().Where(childCollider => !childCollider.isTrigger).ToArray();
        trackedAction.Enable();

        m_gripAmtParam = Animator.StringToHash("GripAmount");
        m_pointAmtParam = Animator.StringToHash("PointAmount");
        gripAction.Enable();
        triggerAction.Enable();

        Hide();
    }

    void UpdateAnimations() {
        float pointAmount = triggerAction.ReadValue<float>();
        handAnimator.SetFloat(m_pointAmtParam, pointAmount);

        float gripAmount = gripAction.ReadValue<float>();
        handAnimator.SetFloat(m_gripAmtParam, Mathf.Clamp01(gripAmount + pointAmount));

        //     handAnimator.SetFloat(m_pointAmtParam, pointAmount);
    }

    // Update is called once per frame
    void Update()
    {
        float isTracked = trackedAction.ReadValue<float>();
        if (isTracked == 1.0f && !m_isCurrentlyTracked)
        {
            m_isCurrentlyTracked = true;
            Show();
        } else if (isTracked == 0 && m_isCurrentlyTracked) {
            m_isCurrentlyTracked = false;
            Hide();
        }

        UpdateAnimations();
    }

    public void Show() {
        foreach (Renderer renderer in m_currentRenderers)
        {
            renderer.enabled = true;
        }
        isHidden = false;
        EnableCollisions(true);
    }

    public void Hide() {
        m_currentRenderers.Clear();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
            m_currentRenderers.Add(renderer);
        }
        isHidden = true;
        EnableCollisions(false);
    }

    public void EnableCollisions(bool enabled)
    {
        if (isCollisionEnabled == enabled) return;

        isCollisionEnabled = enabled;
        foreach (Collider collider in m_colliders)
        {
            collider.enabled = isCollisionEnabled;
        }
    }

    private void OnGrab(XRBaseInteractable grabbedObj)
    {
        HandControl ctrl = grabbedObj.GetComponent<HandControl>();
        if (ctrl)
        {
            if (ctrl.hideHand)
            {
                Hide();
            }
        }
    }

    private void OnRelease(XRBaseInteractable releasedObj)
    {
        HandControl ctrl = releasedObj.GetComponent<HandControl>();
        if (ctrl)
        {
            if (ctrl.hideHand)
            {
                Show();
            }
        }
    }

    public void InteractionCleanup(){
        XRDirectInteractor directInteractor = interactor as XRDirectInteractor;

        if (directInteractor != null) {
            List<XRBaseInteractable> validTargets = new List<XRBaseInteractable>();
            interactor.GetValidTargets(validTargets);
            interactor.interactionManager.ClearInteractorSelection(interactor);
            interactor.interactionManager.ClearInteractorHover(interactor, validTargets);


            foreach (var target in validTargets) {
                if (target.gameObject.scene != interactor.gameObject.scene) {

                    target.transform.position = new Vector3(100, 100, 100); // hack to trigger exit, by moving obj far away

                }
            }
        }
    }
}
