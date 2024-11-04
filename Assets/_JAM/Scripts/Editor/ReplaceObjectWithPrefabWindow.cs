#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ReplaceObjectWithPrefabWindow : EditorWindow
{
    [SerializeField]
    private string objectName = "";
    [SerializeField]
    private GameObject prefab;

    [MenuItem("CocoJumbo/Replace Object With Prefab")]
    public static void ShowWindow()
    {
        // This method is called when the user selects the menu item in the Editor
        EditorWindow wnd = GetWindow<ReplaceObjectWithPrefabWindow>();
        wnd.titleContent = new GUIContent("Replace Object With Prefab");
    }

    private void OnGUI()
    {
        objectName = EditorGUILayout.TextField("Object Name", objectName);
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);
        if(GUILayout.Button("Replace"))
        {
            Replace();
        }
    }

    public void Replace()
    {
        List<GameObject> objs = FindObjectsByType<GameObject>(FindObjectsSortMode.InstanceID).Where(obj => obj.name.Contains(objectName)).ToList();
        objs.ForEach(obj =>
        {
            // Zachowaj pozycję, rotację i skalę oryginalnego obiektu
            Transform parent = obj.transform.parent;
            Vector3 position = obj.transform.position;
            Quaternion rotation = obj.transform.rotation;
            Vector3 scale = obj.transform.localScale;

            // Usuń stary obiekt
            DestroyImmediate(obj);

            // Stwórz instancję powiązaną bezpośrednio z prefabem w miejscu oryginalnego obiektu
            GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            newObject.transform.parent = parent;
            newObject.transform.position = position;
            newObject.transform.rotation = rotation;
            newObject.transform.localScale = scale;
        });
    }
}
#endif