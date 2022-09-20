using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LogInteractions : MonoBehaviour
{
    public void LogHoverEnter(XRBaseInteractable interactor)
    {
        Debug.Log(gameObject.name + " Hover enter by: " + interactor.gameObject.name);
    }

    public void LogHoverExit(XRBaseInteractable interactor)
    {
        Debug.Log(gameObject.name + " Hover exit by: " + interactor.gameObject.name);
    }

    public void LogSelect(XRBaseInteractable interactor)
    {
        Debug.Log(gameObject.name + " Selected by: " + interactor.gameObject.name);
    }

    public void LogDeselect(XRBaseInteractable interactor)
    {
        Debug.Log(gameObject.name + " Deselected by: " + interactor.gameObject.name);
    }

    public void LogActivate(XRBaseInteractable interactor)
    {
        Debug.Log(gameObject.name + " Activated by: " + interactor.gameObject.name);
    }

    public void LogDeactivate(XRBaseInteractable interactor)
    {
        Debug.Log(gameObject.name + " Deactivated by: " + interactor.gameObject.name);
    }
}
