using System.Collections;
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
    public float sin = 20.0f;//波の高さ(大きくなるほど高低差がある)
    public float smoothness = 1;//波の滑らかさ(大きいほど滑らか)

    public float x_cnt = 100;

    public float y_up = 1;//y軸上
    public float y_down = -1;//y軸下

    public bool isPlayer = false;//Playerに触れているかどうか

    public bool WabeMotionFlag;//Playerとの当たり判定の後の処理用フラグ

    private float x = 4;
    private float y = 4;


    void Start()
    {
        mesh = CreatePlaneMesh();
        meshFilter.mesh = mesh;
        isPlayer = false;//Playerに触れていない
        WabeMotionFlag = false;
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
            vertextList.Add(new Vector3(i , y_up, 0));
            vertextList.Add(new Vector3(i , y_down, 0));
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

        Wabe();
        mesh.SetVertices(vertextList);

    }

    private void Wabe()//波の処理
    {
        for (var i = 0; i < vertextList.Count; i += 2)
        {
            var v = vertextList[i];
            if (WabeMotionFlag && !isPlayer) 
            {
                v.y = Mathf.Sin((i + cnt) / x) / y;
                x+=Time.deltaTime;y += Time.deltaTime;
                if (x > sin)
                {
                    x = sin;
                }
                if (y > smoothness)
                {
                    y = smoothness;
                }

                if (x == sin && y == smoothness)
                {
                    WabeMotionFlag = false;
                }
            }
            else if (!isPlayer)
            {
                v.y = Mathf.Sin((i + cnt) / sin) / smoothness;//通常時の波
            }
            else
            {
                v.y = Mathf.Sin((i + cnt) / 4f) / (4f);//Playerと当たっているときの波
                x = 4;
                y = 4;
            }
            vertextList[i] = v;
        }

        cnt += 1f;
    }

    private void OnCollisionEnter(Collision collision)//あたったとき
    {
        
    }

    private void OnCollisionExit(Collision collision)//離れたとき
    {
        
    }

    private void OnCollisionStay(Collision collision)//あたっているとき
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            isPlayer = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            isPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            isPlayer = false;
            WabeMotionFlag = true;
        }
    }
}
