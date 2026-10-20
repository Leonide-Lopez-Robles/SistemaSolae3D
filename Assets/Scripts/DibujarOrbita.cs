using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DibujarOrbita : MonoBehaviour
{
    public float radio = 10f;
    public float ancho = 0.15f;
    public int puntos = 200;

    void Start()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = false;
        lr.loop = true;
        lr.positionCount = puntos;
        lr.startWidth = ancho;
        lr.endWidth = ancho;
        lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lr.receiveShadows = false;

        for (int i = 0; i < puntos; i++)
        {
            float angulo = i * 2f * Mathf.PI / puntos;
            lr.SetPosition(i, new Vector3(Mathf.Cos(angulo) * radio, 0f, Mathf.Sin(angulo) * radio));
        }
    }
}