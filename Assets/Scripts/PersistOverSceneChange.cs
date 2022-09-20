using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistOverSceneChange : MonoBehaviour
{

    public bool applyToChildren = true;
    int m_persistentLayer = 0;
    int m_currentLayer = 0;

    void Awake()
    {
        m_persistentLayer = LayerMask.NameToLayer("XR Persistent");
        m_currentLayer = gameObject.layer;
    }

    private void OnEnable()
    {
        SceneLoader.Instance.onLoadStart.AddListener(StartPersist);
        SceneLoader.Instance.onLoadFinish.AddListener(EndPersist);
    }

    private void OnDisable()
    {
        var loader = SceneLoader.Instance;
        if (loader != null)
        {
            loader.onLoadStart.RemoveListener(StartPersist);
            loader.onLoadFinish.RemoveListener(EndPersist);
        }

    }

    void StartPersist() {
        m_currentLayer = gameObject.layer;
        SetLayer(gameObject, m_persistentLayer, applyToChildren);
    }

    void EndPersist()
    {
        SetLayer(gameObject, m_currentLayer, applyToChildren);
    }

    void SetLayer(GameObject obj, int newLayer, bool applyToChildren)
    {
        obj.layer = newLayer;
        if (applyToChildren)
        {
            foreach (Transform child in obj.transform) {
                SetLayer(child.gameObject, newLayer, applyToChildren);
            }
        }
    }
}
