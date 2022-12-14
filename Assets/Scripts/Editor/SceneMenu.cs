using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEditor;

public static class SceneMenu
{
    [MenuItem("Scenes/Lobby")]
    static void OpenDreamSpa()
    {
        OpenScene(SceneUtils.Names.Lobby);
    }

    [MenuItem("Scenes/Maze")]
    static void OpenMaze()
    {
        OpenScene(SceneUtils.Names.Maze);
    }

    [MenuItem("Scenes/ComplexInteraction")]
    static void OpenComplexInteractions()
    {
        OpenScene(SceneUtils.Names.ComplexInteractions);
    }

    static void OpenScene(string name)
    {
        Scene persistentScene = EditorSceneManager.OpenScene("Assets/Scenes/"+SceneUtils.Names.XRPersistent + ".unity", OpenSceneMode.Single);
        Scene currentScene = EditorSceneManager.OpenScene("Assets/Scenes/" + name + ".unity", OpenSceneMode.Additive);

        SceneUtils.AlignXRRig(persistentScene, currentScene);

    }
}
