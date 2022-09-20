using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    public UnityEvent onFinishDrag = new UnityEvent();
    public Transform startOrientation = null;
    public Transform endOrientation = null;

    MeshRenderer meshRenderer = null;
    public GameObject  teapot = null;
    Vector3 teapotStartingPos;

    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        teapotStartingPos = teapot.transform.position;
    }

    public void OnLeverPullStart()
    {
        meshRenderer.material.SetColor("_Color", Color.red);
        // teapot.transform.position = teapotStartingPos + new Vector3(0, 3, 0);
    }

    public void OnLeverPullStop()
    {
        meshRenderer.material.SetColor("_Color", Color.white);
        teapot.transform.position = teapotStartingPos;
        onFinishDrag?.Invoke();
    }

    public void UpdateLever(float percent)
    {
        transform.rotation = Quaternion.Slerp(startOrientation.rotation, endOrientation.rotation, percent);
        teapot.transform.position = Vector3.Lerp(teapotStartingPos, teapotStartingPos + new Vector3(0, 1, 0), Mathf.PingPong(Time.time * 0.25f, 1.0f));

    }
}
