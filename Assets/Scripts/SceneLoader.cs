using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public Material screenFade = null;
    [Min(0.001f)]
    public float fadeSpeed = 1.0f;
    [Range(0.0f, 5.0f)]
    public float addedWaitTime = 2.0f;

    public UnityEvent onBeforeUnload = new UnityEvent();
    public UnityEvent onLoadStart = new UnityEvent();
    public UnityEvent onLoadFinish = new UnityEvent();

    float m_fadeamount = 0.0f;
    Coroutine m_fadeCoroutine = null;
    static readonly int m_fadeamountPropID = Shader.PropertyToID("_FadeAmount");
    bool m_isLoading = false;
    Scene m_persistentScene;

    public void LoadScene(string name)
    {
        if (!m_isLoading)
        {
            StartCoroutine(Load(name));
        }
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += SetActiveScene;
        m_persistentScene = SceneManager.GetActiveScene();

        if (!Application.isEditor)
        {
            SceneManager.LoadSceneAsync(SceneUtils.Names.Lobby, LoadSceneMode.Additive);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SetActiveScene;
    }

    void SetActiveScene(Scene scene, LoadSceneMode mode) {
        SceneManager.SetActiveScene(scene);
        SceneUtils.AlignXRRig(m_persistentScene, scene);
    }

    IEnumerator Load(string name)
    {
        m_isLoading = true;
        onLoadStart?.Invoke();
        yield return FadeOut();
        onBeforeUnload?.Invoke();
        yield return new WaitForSeconds(0);
        yield return StartCoroutine(UnloadCurrentScene());

        yield return new WaitForSeconds(addedWaitTime);

        yield return StartCoroutine(LoadNewScene(name));
        yield return FadeIn();
        onLoadFinish?.Invoke();
        m_isLoading = false;
    }

    IEnumerator UnloadCurrentScene()
    {
        AsyncOperation unload = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        while (!unload.isDone) {
            yield return null;
        }
    }
    
    IEnumerator LoadNewScene(string name)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        while (!load.isDone)
        {
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        if (m_fadeCoroutine != null)
        {
            StopCoroutine(m_fadeCoroutine);
        }
        m_fadeCoroutine = StartCoroutine(Fade(1.0f));
        yield return m_fadeCoroutine;
    }

    IEnumerator FadeIn()
    {
        if (m_fadeCoroutine != null) {
            StopCoroutine(m_fadeCoroutine);
        }
        m_fadeCoroutine = StartCoroutine(Fade(0.0f));
        yield return m_fadeCoroutine;
    }

    IEnumerator Fade(float target)
    {
        while(!Mathf.Approximately(m_fadeamount, target))
        {
            m_fadeamount = Mathf.MoveTowards(m_fadeamount, target, fadeSpeed * Time.deltaTime);
            screenFade.SetFloat(m_fadeamountPropID, m_fadeamount);
            yield return null;
        }
        screenFade.SetFloat(m_fadeamountPropID, target);
    }
}
