using UnityEngine;

public class Orbita : MonoBehaviour
{
    public float Velocidad = 10f;

    void Update()
    {
        transform.Rotate(0f, Velocidad * Time.deltaTime, 0f);
    }
}