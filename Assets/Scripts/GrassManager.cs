using System.Collections.Generic;
using UnityEngine;

// Creatureは草を食べるときにGrassManagerに自身の座標を送る
public class GrassManager : MonoBehaviour
{
    [SerializeField] Transform grassParent;
    [SerializeField] GameObject grassCubePrefab; // 1x1
    [SerializeField] Transform wallParent;
    [SerializeField] GameObject wallPrefab; // 透明
    [SerializeField] Color grassColor;
    [SerializeField] Color soilColor;
    [SerializeField] int ranchSize = 11; // 正方形
    float wallHeight = 5f;
    float time = 0;
    [SerializeField] float growthTime = 120f;
    private List<Material> cubeMList = new List<Material>();
    private List<int> soilList = new List<int>();

    public int RanchSize { get { return ranchSize; } }

    void Start()
    {
        GenerateGrass();
        GenerateWall();
        // 10x10で30度見下ろすとy10,z15で距離が5_/13くらい
        // y = ranchSize, z = ranchSize + 1.5f でいいか
        CameraPositioning();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= growthTime)
        {
            time = 0;
            if (soilList.Count == 0) return;
            int idx = Random.Range(0, soilList.Count);
            cubeMList[idx].color = grassColor;
            soilList.RemoveAt(idx);
        }
    }

    // Creatureから呼び出される
    public void Grazing(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x);
        int z = Mathf.RoundToInt(position.z);
        int id = GetID(x, z);
        cubeMList[id].color = soilColor;
        // 同じID２回入れられるかも。草生えてないのにGrazingできるかも
        soilList.Add(id);
    }

    int GetID(int x, int z)
    {
        return x * ranchSize + z;
    }

    void GenerateGrass()
    {
        for (int x = 0; x < ranchSize; x++)
        {
            for (int z = 0; z < ranchSize; z++)
            {
                GameObject cube = Instantiate(grassCubePrefab);
                cube.name = $"GrassCube_{GetID(x, z)}";
                cube.transform.parent = grassParent;
                cube.transform.position = new Vector3(x, -0.5f, z);
                cube.transform.localScale = new Vector3(1, 1, 1);
                cubeMList.Add(cube.GetComponent<Material>());
            }
        }
    }

    // 草原を囲むcolliderの壁を四方に配置
    void GenerateWall()
    {
        float half = (ranchSize - 1) / 2f;
        GenerateWallPosition(half, -1, 0); // ↓
        GenerateWallPosition(-1, half, 90); // ←
        GenerateWallPosition(half, ranchSize, 0); // ↑
        GenerateWallPosition(ranchSize, half, 90); // →
    }

    void GenerateWallPosition(float x, float z, float yRot)
    {
        GameObject wall = Instantiate(wallPrefab);
        wall.transform.parent = wallParent;
        wall.transform.localScale = new Vector3(ranchSize, wallHeight, 1);
        wall.transform.position = new Vector3(x, wallHeight / 2f, z);
        wall.transform.rotation = Quaternion.Euler(0, yRot, 0);
    }

    // 動かさないなら本来絶対にスクリプトからやるべきではない
    // ステージの生成もeditor完結にするべき
    void CameraPositioning()
    {
        Camera.main.transform.position = new Vector3((ranchSize - 1) / 2f, ranchSize, -ranchSize * 1.5f);
        Camera.main.transform.rotation = Quaternion.Euler(30, 0, 0);
    }
}