using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AnilloPlaneta : MonoBehaviour
{
    public float radioInterior = 0.6f;
    public float radioExterior = 1.2f;
    public int segmentos = 100;

    void Start()
    {
        Vector3[] vertices = new Vector3[(segmentos + 1) * 2];
        Vector2[] uvs = new Vector2[vertices.Length];
        Vector3[] normales = new Vector3[vertices.Length];
        int[] triangulos = new int[segmentos * 6];

        for (int i = 0; i <= segmentos; i++)
        {
            float angulo = i * 2f * Mathf.PI / segmentos;
            float cos = Mathf.Cos(angulo);
            float sin = Mathf.Sin(angulo);

            vertices[i * 2] = new Vector3(cos * radioInterior, 0f, sin * radioInterior);
            vertices[i * 2 + 1] = new Vector3(cos * radioExterior, 0f, sin * radioExterior);
            uvs[i * 2] = new Vector2(0f, 0.5f);
            uvs[i * 2 + 1] = new Vector2(1f, 0.5f);
            normales[i * 2] = Vector3.up;
            normales[i * 2 + 1] = Vector3.up;
        }

        int t = 0;
        for (int i = 0; i < segmentos; i++)
        {
            int a = i * 2, b = i * 2 + 1, c = i * 2 + 2, d = i * 2 + 3;
            triangulos[t++] = a; triangulos[t++] = b; triangulos[t++] = c;
            triangulos[t++] = b; triangulos[t++] = d; triangulos[t++] = c;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.normals = normales;
        mesh.triangles = triangulos;
        GetComponent<MeshFilter>().mesh = mesh;
    }
}