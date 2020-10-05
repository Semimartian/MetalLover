using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    [SerializeField] private PhysicMaterial physicMaterial;
    private void Start()
    {
        CombineChildren();
    }

    private void CombineChildren()
    {
        MeshFilter myMeshFilter = GetComponent<MeshFilter>();
        //ombineInstance[] combineInstances = new CombineInstance[];
        List<CombineInstance> combineInstances = new List<CombineInstance>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            MeshFilter meshFilter = t.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                CombineInstance combineInstance = new CombineInstance();
                combineInstance.mesh = meshFilter.mesh;
                combineInstance.transform = t.localToWorldMatrix;
                //combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                combineInstances.Add(combineInstance);
            }
            Destroy(t.gameObject);
        }

        Debug.Log("combineInstances Count" + combineInstances.Count);
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combineInstances.ToArray());
        myMeshFilter.mesh = combinedMesh;
        gameObject.AddComponent<MeshCollider>().material = physicMaterial;


        //CleanUp
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

}
