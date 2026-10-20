using UnityEngine;

public class CinturonAsteroides : MonoBehaviour
{
    public int cantidad = 400;
    public float radioMin = 16f;
    public float radioMax = 18f;
    public float altura = 0.4f;
    public float tamanoMin = 0.05f;
    public float tamanoMax = 0.18f;
    public Material material;

    void Start()
    {
        for (int i = 0; i < cantidad; i++)
        {
            GameObject a = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Destroy(a.GetComponent<Collider>());
            a.transform.SetParent(transform, false);

            float angulo = Random.value * Mathf.PI * 2f;
            float radio = Random.Range(radioMin, radioMax);
            a.transform.localPosition = new Vector3(
                Mathf.Cos(angulo) * radio,
                Random.Range(-altura, altura),
                Mathf.Sin(angulo) * radio);

            float s = Random.Range(tamanoMin, tamanoMax);
            a.transform.localScale = new Vector3(
                s * Random.Range(0.7f, 1.3f),
                s * Random.Range(0.7f, 1.3f),
                s * Random.Range(0.7f, 1.3f));
            a.transform.localRotation = Random.rotation;

            if (material != null)
                a.GetComponent<MeshRenderer>().sharedMaterial = material;
        }
    }
}