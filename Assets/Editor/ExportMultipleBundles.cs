using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class ExportMultipleBundles : EditorWindow
{

    private const string helpMsg = "\n\n-Choose prefab's folder\n\n -Chosse destination \n";

    [MenuItem("Exporter/Android/Export Prefabs Folder - Android - Track dependencies")]
    
    static void ExportMultplieResourcesAndroid()
    {
 
        string _path = EditorUtility.OpenFolderPanel("Select Prefabs Folder", "", "");

        if (_path == "")
        {
            Debug.Log("Export canceled, No Prefabs Folder");
            return;
        }

        //Read all prefabs from folder
        var prefabs = LoadAllPrefabsAt(_path);


        if (prefabs.Count == 0)
        {
            //Show Alert
            EditorUtility.DisplayDialog("Error", "Founded " + prefabs.Count + " prefabs" + ", operation will be canceled.", "Cancel");
            Debug.Log("Export canceled");
            return;
        }


        //Show Alert
        bool confirm = EditorUtility.DisplayDialog("Confirm?",
            "Founded " + prefabs.Count
            + " prefabs", "Export", "Cancel");

        
        if (!confirm)
        {
            Debug.Log("Export canceled");
            return;
        }

        //Pergunta caminho para salvar.  *nome do arquivo será o nome do objeto
        string destination_path = EditorUtility.OpenFolderPanel("Select destination Folder", "", "");
        if (destination_path == "")
        {
            Debug.Log("Export canceled, no destination Folder");
            return;
        }


        for (int i = 0; i < prefabs.Count; i++)
        {
            
            string path = destination_path + "/" + prefabs[i].name + "_Android.unity3d";
            
            if (path.Length != 0)
            {
                BuildPipeline.BuildAssetBundle(prefabs[i], null, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android);
            }

        }

        //Se deu certo , mostra a pasta
        if (prefabs.Count > 0)
            EditorUtility.RevealInFinder(destination_path);

        Debug.Log("Finish export");
    }

    [MenuItem("Exporter/iOS/Export Prefabs Folder - iOS - Track dependencies")]
    static void ExportMultplieResourcesiOS()
    {        
        string _path = EditorUtility.OpenFolderPanel("Select Prefabs Folder", "", "");
     
        if (_path == "")
        {
            Debug.Log("Export canceled, no Prefabs Folder");
            return;
        }

        //Read all prefabs from folder
        var prefabs = LoadAllPrefabsAt(_path);


        if (prefabs.Count == 0)
        {
            //Show Alert
            EditorUtility.DisplayDialog("Error", "Founded " + prefabs.Count + " prefabs" + ",operação will be canceled.", "Cancel");
            Debug.Log("Export canceled");
            return;
        }


        //Mostra Alert
        bool confirm = EditorUtility.DisplayDialog("Confirm?",
            "Founded " + prefabs.Count
            + " prefabs", "Export", "Cancel");
        
        if (!confirm)
        {
            Debug.Log("Export canceled");
            return;
        }

        //Ask destination folder.  *file name wil be object name
        string destination_path = EditorUtility.OpenFolderPanel("Select destination Folder", "", "");
        if (destination_path == "")
        {
            Debug.Log("Export canceled, no destination Folder");
            return;
        }


        for (int i = 0; i < prefabs.Count; i++)
        {
            
            string path = destination_path + "/" + prefabs[i].name + "_IOS.unity3d";
            
            if (path.Length != 0)
            {
                BuildPipeline.BuildAssetBundle(prefabs[i], null, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.iOS);
            }

        }

        //Worked ? Show folder
        if (prefabs.Count > 0)
            EditorUtility.RevealInFinder(destination_path);

        Debug.Log("Finish export");
    }

    [MenuItem("Exporter/Help")]
    static void Help()
    {
        ExportMultipleBundles window = (ExportMultipleBundles)EditorWindow.GetWindow(typeof(ExportMultipleBundles), false, "Help", true);
    }

    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// This function can be called multiple times per frame (one call per event).
    /// </summary>
    void OnGUI()
    {
        GUILayout.Label(helpMsg);
    }

    public static List<GameObject> LoadAllPrefabsAt(string path)
    {
        if (path != "")
        {
            if (path.EndsWith("/"))
            {
                path = path.TrimEnd('/');
            }
        }

        DirectoryInfo dirInfo = new DirectoryInfo(path);
        FileInfo[] fileInf = dirInfo.GetFiles("*.prefab");

        //loop through directory loading the game object and checking if it has the component you want
        List<GameObject> prefabs = new List<GameObject>();
        foreach (FileInfo fileInfo in fileInf)
        {
            string fullPath = fileInfo.FullName.Replace(@"\", "/");
            string assetPath = "Assets" + fullPath.Replace(Application.dataPath, "");
            GameObject prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

            if (prefab != null)
            {
                prefabs.Add(prefab);
            }
        }
        return prefabs;
    }
}