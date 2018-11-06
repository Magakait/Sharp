using System.IO;

using UnityEngine;

using UnityEditor;
using UnityEditor.Callbacks;

public class BuildComposer
{
    [PostProcessBuildAttribute]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        Debug.Log(pathToBuiltProject);
        //if (Directory.Exists(pathToBuiltProject + "/Build"))
    }
}