using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dial : MonoBehaviour
{
    Vector3 m_startRot;
    Vector3 dialStartRot;
    MeshRenderer m_meshRenderer = null;
    public GameObject teapot = null;

    private void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    public void StartTurn()
    {
        m_startRot = transform.localEulerAngles;
        dialStartRot = teapot.transform.localEulerAngles;
        m_meshRenderer.material.SetColor("_Color", Color.red);
    }
    
    public void StopTurn()
    {
        m_meshRenderer.material.SetColor("_Color", Color.white);
    }


    public void DialUpdate(float angle)
    {
        Vector3 dialAngles = m_startRot;
        Vector3 teaPotAngles = dialStartRot;
        dialAngles.y += angle * -1;
        teaPotAngles.y += angle * -1;

        transform.localEulerAngles = dialAngles;
        teapot.transform.localEulerAngles = teaPotAngles;

    }
}
