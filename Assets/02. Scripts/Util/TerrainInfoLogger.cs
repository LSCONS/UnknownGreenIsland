using UnityEngine;

public class TerrainInfoLogger : MonoBehaviour
{
    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        if (terrain == null)
        {
            Debug.LogError("�� ������Ʈ�� Terrain ������Ʈ�� �����ϴ�!");
            return;
        }

        TerrainData terrainData = terrain.terrainData;
        if (terrainData == null)
        {
            Debug.LogError("Terrain Data�� �����ϴ�!");
            return;
        }

        Debug.Log($"[Terrain Info] Heightmap Resolution: {terrainData.heightmapResolution}");
        Debug.Log($"[Terrain Info] Alphamap Resolution: {terrainData.alphamapResolution}");
        Debug.Log($"[Terrain Info] Detail Resolution: {terrainData.detailResolution}");
        Debug.Log($"[Terrain Info] Tree Count: {terrainData.treeInstanceCount}");
    }
}
