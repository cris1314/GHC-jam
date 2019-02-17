using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Combine all the meshes that are in childs of this object
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCombiner : MonoBehaviour {

    public Material NewMaterial;
    
    List<GameObject> childs = new List<GameObject>();

    public void CombineMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(); ;
        CombineInstance[] combine;
        combine = new CombineInstance[meshFilters.Length];
        for (int k = 0; k < transform.childCount; k++)
        {
            childs.Add(transform.GetChild(k).gameObject);
        }
        int i = 0;
        while (i < meshFilters.Length)
        {

            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material = NewMaterial;
        }
        transform.gameObject.SetActive(true);
    }

    /*public void SeeChilds() {
        foreach (GameObject g in childs)
        {
            g.SetActive(true);
        }
       
        this.gameObject.SetActive(false);
    }

    public void HideChilds()
    {
        foreach (GameObject g in childs)
        {
            g.SetActive(false);
        }
        this.gameObject.SetActive(true);
    }*/
}