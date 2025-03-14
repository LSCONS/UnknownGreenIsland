using UnityEngine;

public class TerrainInfoLogger : MonoBehaviour
{
    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        if (terrain == null)
        {
            Debug.LogError("이 오브젝트에 Terrain 컴포넌트가 없습니다!");
            return;
        }

        TerrainData terrainData = terrain.terrainData;
        if (terrainData == null)
        {
            Debug.LogError("Terrain Data가 없습니다!");
            return;
        }

        Debug.Log($"[Terrain Info] Heightmap Resolution: {terrainData.heightmapResolution}");
        Debug.Log($"[Terrain Info] Alphamap Resolution: {terrainData.alphamapResolution}");
        Debug.Log($"[Terrain Info] Detail Resolution: {terrainData.detailResolution}");
        Debug.Log($"[Terrain Info] Tree Count: {terrainData.treeInstanceCount}");
    }
}
