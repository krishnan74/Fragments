using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MergeProBuilderMeshes : MonoBehaviour
{
    public GameObject[] objectsToCombine;  // Assign this array in the Inspector
    private MeshFilter[] meshFilters;      // MeshFilters of the objects to combine

    void Start()
    {
        // Initialize meshFilters array to hold references to MeshFilter components
        meshFilters = new MeshFilter[objectsToCombine.Length];

        // Populate meshFilters array
        for (int j = 0; j < objectsToCombine.Length; j++)
        {
            meshFilters[j] = objectsToCombine[j].GetComponent<MeshFilter>();
        }

        // Create an array of CombineInstance to store the mesh and transformations
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        Vector3 combinedCenter = Vector3.zero;

        // Store the meshes and transformations in CombineInstance array
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

            // Calculate the position of each object and update the combined center
            combinedCenter += meshFilters[i].transform.position;
            meshFilters[i].gameObject.SetActive(false);  // Disable original objects

            i++;
        }

                // Combine all meshes into a single new mesh
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine);

        // Calculate the center of the combined mesh in local space
        Vector3 combinedMeshCenter = combinedMesh.bounds.center;

        // Adjust the vertices of the combined mesh to make it centered at origin
        Vector3[] vertices = combinedMesh.vertices;
        for (int v = 0; v < vertices.Length; v++)
        {
            vertices[v] -= combinedMeshCenter; // Offset each vertex by the center
        }
        combinedMesh.vertices = vertices;
        combinedMesh.RecalculateBounds();

        // Create a new GameObject to hold the combined mesh
        GameObject combinedObject = new GameObject("CombinedMesh");

        // Assign the combined mesh to the new GameObject's MeshFilter
        MeshFilter combinedMeshFilter = combinedObject.AddComponent<MeshFilter>();
        MeshRenderer combinedMeshRenderer = combinedObject.AddComponent<MeshRenderer>();
        combinedMeshFilter.sharedMesh = combinedMesh;

        // Position the GameObject at the true center of the combined meshes
        combinedObject.transform.position = combinedCenter / meshFilters.Length;

        // Handle materials (assumes each object has one material; adjust if needed)
        CombineMaterials(combinedObject);

        // Re-enable the new combined GameObject
        combinedObject.SetActive(true);

    }

    void CombineMaterials(GameObject combinedObject)
    {
        // Collect all materials from the meshFilters
        Material[] combinedMaterials = new Material[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
        {
            // Assuming each object has one material (this could be extended to handle multiple materials)
            Material[] materials = meshFilters[i].GetComponent<Renderer>().sharedMaterials;
            combinedMaterials[i] = materials[0]; // Only taking the first material for now
        }

        // Set the materials on the new combined GameObject's MeshRenderer
        combinedObject.GetComponent<MeshRenderer>().sharedMaterials = combinedMaterials;
    }
}
