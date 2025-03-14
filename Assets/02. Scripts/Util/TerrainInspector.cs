using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Terrain))]
public class TerrainInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Terrain terrain = (Terrain)target;
        TerrainData terrainData = terrain.terrainData;

        if (terrainData == null) return;

        GUILayout.Space(10);
        GUILayout.Label("Terrain Data Info", EditorStyles.boldLabel);
        GUILayout.Label($"Heightmap Resolution: {terrainData.heightmapResolution}");
        GUILayout.Label($"Alphamap Resolution: {terrainData.alphamapResolution}");
        GUILayout.Label($"Detail Resolution: {terrainData.detailResolution}");
    }
}
