using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    public Transform startPosition = null;
    public Transform endPosition = null;


    public GameObject teapot = null;
    MeshRenderer meshRenderer = null;
    MeshRenderer teapotMeshRenderer = null;
    Color teapotColor;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        teapotMeshRenderer = teapot.GetComponent<MeshRenderer>();
        teapotColor = teapot.GetComponent<Renderer>().material.color;
    }

    public void OnSlideStart() {
        meshRenderer.material.SetColor("_Color", Color.red);
    }

    public void OnSlideStop()
    {
        meshRenderer.material.SetColor("_Color", Color.white);
    }

    public void UpdateSlider(float percent)
    {
        transform.position = Vector3.Lerp(startPosition.position, endPosition.position, percent);
        Color newTeapotColor = new Color(teapotColor.r-1*percent, teapotColor.g+1*percent, teapotColor.b+1*percent, 1);
        teapotMeshRenderer.material.SetColor("_Color", newTeapotColor);
    }
}
