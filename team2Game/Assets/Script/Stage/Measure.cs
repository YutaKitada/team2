using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Measure : MonoBehaviour
{
    [SerializeField]
    GameObject measure1;
    [SerializeField]
    GameObject measure2;

    int objectCount1;
    int objectCount2;

    // Start is called before the first frame update
    void Start()
    {
        objectCount1 = 0;
        objectCount2 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Measurement();
        Debug.Log(objectCount1);
        Debug.Log(objectCount2);
    }

    void Measurement()
    {
        var measureList1 = new List<RaycastHit>(Physics.BoxCastAll(
            measure1.transform.position, measure1.transform.localScale * 0.5f, Vector3.up));
        for(int i = 0; i < measureList1.Count; i++)
        {
            objectCount1++;
        }

        var measureList2 = new List<RaycastHit>(Physics.BoxCastAll(
            measure2.transform.position, measure2.transform.localScale * 0.5f, Vector3.up));
        for (int i = 0; i < measureList2.Count; i++)
        {
            objectCount2++;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(measure1.transform.position+Vector3.up, measure1.transform.localScale);

        Gizmos.color = Color.red;
        Gizmos.DrawCube(measure2.transform.position + Vector3.up, measure2.transform.localScale);
    }
}
