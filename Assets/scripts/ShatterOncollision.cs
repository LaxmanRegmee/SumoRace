using UnityEngine;

public class ShatterOnCollision : MonoBehaviour
{
    public int pieces = 5;               // Number of fragments the object will break into
    public float explosionForce = 500f;  // Force applied to each fragment
    public float explosionRadius = 5f;   // Radius within which the force is applied
    public float fragmentLifetime = 2f;  // Time in seconds before fragments are destroyed

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Shatter();
        }
    }

    void Shatter()
    {
        // Get the mesh filter of the object
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshFilter == null || meshFilter.mesh == null)
        {
            Debug.LogError("MeshFilter or Mesh is missing on the object.");
            return;
        }

        // Get the mesh and create a fragmentation algorithm
        Mesh mesh = meshFilter.mesh;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        // Create fragments (basic example with triangle-based fragmentation)
        for (int i = 0; i < triangles.Length; i += 3)
        {
            CreateFragment(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]]);
        }

        // Destroy the original object
        Destroy(gameObject);
    }

    void CreateFragment(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        // Create a new fragment GameObject
        GameObject fragment = new GameObject("Fragment");
        fragment.transform.position = transform.position;
        fragment.transform.rotation = transform.rotation;

        // Add a MeshFilter and MeshRenderer to the fragment
        MeshFilter fragmentMeshFilter = fragment.AddComponent<MeshFilter>();
        MeshRenderer fragmentMeshRenderer = fragment.AddComponent<MeshRenderer>();

        // Assign the same material to the fragment
        fragmentMeshRenderer.material = GetComponent<MeshRenderer>().material;

        // Create a new mesh for the fragment
        Mesh fragmentMesh = new Mesh();
        fragmentMesh.vertices = new Vector3[] { v1, v2, v3, v1 + Vector3.up * 0.1f, v2 + Vector3.up * 0.1f, v3 + Vector3.up * 0.1f };
        fragmentMesh.triangles = new int[] { 0, 1, 2, 3, 4, 5 };

        fragmentMesh.RecalculateNormals();
        fragmentMeshFilter.mesh = fragmentMesh;

        // Add a Rigidbody and Collider to the fragment
        Rigidbody rb = fragment.AddComponent<Rigidbody>();
        fragment.AddComponent<MeshCollider>().convex = true;

        // Apply explosion force to the fragment
        rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);

        // Destroy the fragment after a delay
        Destroy(fragment, fragmentLifetime);
    }
}
