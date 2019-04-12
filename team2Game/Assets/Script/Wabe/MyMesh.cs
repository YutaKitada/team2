﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMesh : MonoBehaviour
{
    [SerializeField]
    private MeshFilter meshFilter;

    private Mesh mesh;
    private List<Vector3> vertextList = new List<Vector3>();
    private List<Vector2> uvList = new List<Vector2>();
    private List<int> indexList = new List<int>();
    private float cnt = 0;
    public float sin = 20.0f;
    public float smoothness = 1;

    public float x_cnt = 100;

    void Start()
    {
        mesh = CreatePlaneMesh();
        meshFilter.mesh = mesh;

    }
    private Mesh CreatePlaneMesh()
    {
        var mesh = new Mesh();

        //vertextList.Add(new Vector3(-1, -1, 0));//0番頂点
        //vertextList.Add(new Vector3(1, -1, 0)); //1番頂点
        //vertextList.Add(new Vector3(-1, 1, 0)); //2番頂点
        //vertextList.Add(new Vector3(1, 1, 0));  //3番頂点
        for (float i = 0; i < x_cnt * 10; i += 0.1f)
        {
            vertextList.Add(new Vector3(i - 0.1f, 1, 0));
            vertextList.Add(new Vector3(i - 0.1f, -1, 0));
        }


        //uvList.Add(new Vector2(0, 0));
        //uvList.Add(new Vector2(1, 0));
        //uvList.Add(new Vector2(0, 1));
        //uvList.Add(new Vector2(1, 1));

        //indexList.AddRange(new[] { 0, 2, 1, 1, 2, 3 });//0-2-1の頂点で1三角形。 1-2-3の頂点で1三角形。
        for (int i = 0; i < x_cnt * 10; i++)
        {
            indexList.AddRange(new[] { i, i + 2, i + 1, i + 1, i + 2, i + 3 });
        }

        mesh.SetVertices(vertextList);//meshに頂点群をセット
        //mesh.SetUVs(0, uvList);//meshにテクスチャのuv座標をセット（今回は割愛)
        mesh.SetIndices(indexList.ToArray(), MeshTopology.Triangles, 0);//メッシュにどの頂点の順番で面を作るかセット
        return mesh;
    }

    void Update()
    {
        for (var i = 0; i < vertextList.Count; i += 2)
        {
            var v = vertextList[i];
            v.y = Mathf.Sin((i + cnt) / sin) / smoothness;
            vertextList[i] = v;
        }
        cnt += 1f;
        mesh.SetVertices(vertextList);

    }
}
