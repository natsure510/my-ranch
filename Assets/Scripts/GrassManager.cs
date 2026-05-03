using System.Collections.Generic;
using UnityEngine;

// Creatureは草を食べるときにGrassManagerに自身の座標を送る
public class GrassManager : MonoBehaviour
{
    [SerializeField] Transform grassParent;
    [SerializeField] Color grassColor;
    [SerializeField] Color soilColor;
    [SerializeField] GameObject grassCubePrefab; // 1x1
    [SerializeField] int ranchSize = 11; // 正方形
    float time = 0;
    [SerializeField] float growthTime = 120f;
    private List<Material> cubeMList = new List<Material>();
    private List<int> soilList = new List<int>();

    void Start()
    {
        for (int x = 0; x < ranchSize; x++)
        {
            for (int z = 0; z < ranchSize; z++)
            {
                GameObject cube = Instantiate(grassCubePrefab);
                cube.transform.parent = grassParent;
                cube.transform.position = new Vector3(x, -0.5f, z);
                cube.transform.localScale = new Vector3(1, 1, 1);
                cubeMList.Add(cube.GetComponent<Material>());
            }
        }
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
        soilList.Add(id);
    }

    int GetID(int x, int z)
    {
        return x * ranchSize + z;
    }
}