using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(MeshCombiner))]
public class MeshCombinerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MeshCombiner mc_target = (MeshCombiner)target;
        EditorGUILayout.LabelField("Press the button to combine the meshes in childs :)");
        if (GUILayout.Button("Combine Meshes"))
        {
           mc_target.CombineMeshes();
        }
        /*if (GUILayout.Button("Show Childs"))
        {
            mc_target.SeeChilds();
        }
        if (GUILayout.Button("Hide Childs"))
        {
            mc_target.HideChilds();
        }*/

    }

}
