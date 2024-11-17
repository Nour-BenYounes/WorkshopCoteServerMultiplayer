using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class DisableCollidersEditor : EditorWindow
{
    [MenuItem("Tools/Disable Colliders")]
    public static void ShowWindow()
    {
        GetWindow<DisableCollidersEditor>("Disable Colliders");
    }

    private GameObject selectedObject;

    void OnGUI()
    {
        EditorGUILayout.LabelField("Disable Colliders in Selected Object", EditorStyles.boldLabel);

        // Display a selection field for the GameObject
        selectedObject = (GameObject)EditorGUILayout.ObjectField("Target Object", selectedObject, typeof(GameObject), true);

        if (selectedObject == null)
        {
            EditorGUILayout.HelpBox("Please select a GameObject in the scene.", MessageType.Info);
            return;
        }

        if (GUILayout.Button("Disable All Colliders"))
        {
            DisableAllColliders(selectedObject);
        }
    }

    private void DisableAllColliders(GameObject obj)
    {
        // Get all colliders in the selected object and its children
        Collider[] colliders = obj.GetComponentsInChildren<Collider>(true);

        // Disable each collider
        foreach (Collider collider in colliders)
        {
            Undo.RecordObject(collider, "Disable Collider");
            collider.enabled = false;
            EditorUtility.SetDirty(collider);
        }

        Debug.Log("All colliders in " + obj.name + " and its children have been disabled.");
    }
}