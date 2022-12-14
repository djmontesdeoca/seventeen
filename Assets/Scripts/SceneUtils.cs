using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneUtils
{
    public enum SceneId
    {
        Lobby,
        Maze,
        ComplexInteractions
    }

    public static readonly string[] scenes = {Names.Lobby, Names.Maze, Names.ComplexInteractions };
    public static class Names
    {
        public static readonly string XRPersistent = "XRPersistent";
        public static readonly string Maze = "Maze";
        public static readonly string ComplexInteractions = "ComplexInteractions"; 
        public static readonly string Lobby = "DreamSpa";



    }

    public static void AlignXRRig(Scene persistentScene, Scene currentScene) {
        GameObject[] currentObjects = currentScene.GetRootGameObjects();
        GameObject[] persistentObjects = persistentScene.GetRootGameObjects();

        foreach (var origin in currentObjects) {
            if (origin.CompareTag("XRRigOrigin"))
            {
                foreach(var rig in persistentObjects)
                {
                    if (rig.CompareTag("XRRig"))
                    {
                        rig.transform.position = origin.transform.position;
                        rig.transform.rotation = origin.transform.rotation;
                        return;
                    }
                }
            }
        }
    }
}
