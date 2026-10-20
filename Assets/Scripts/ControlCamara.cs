using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class ControlCamara : MonoBehaviour
{
    public float velocidad = 25f;
    public float sensibilidad = 1f;
    public float suavizado = 4f;

    string[] nombres = { "Sol", "Mercurio", "Venus", "Tierra", "Marte", "Jupiter", "Saturno", "Urano", "Neptuno" };
    Transform objetivo;
    float distancia;
    float yaw, pitch;          // vuelo libre
    float orbYaw, orbPitch;    // giro alrededor del planeta

    void Start()
    {
        SincronizarAngulos();
    }

    void SincronizarAngulos()
    {
        Vector3 e = transform.eulerAngles;
        yaw = e.y;
        pitch = e.x > 180f ? e.x - 360f : e.x;
    }

    void Update()
    {
        // Saltar a un planeta con las teclas 0 a 8
        int n = LeerNumero();
        if (n >= 0)
        {
            GameObject go = GameObject.Find(nombres[n]);
            if (go != null)
            {
                objetivo = go.transform;
                distancia = Mathf.Max(go.transform.lossyScale.x * 4f, 3f);
                orbYaw = 0f;
                orbPitch = 20f;
            }
        }

        // Si tocas W A S D Q E, la cámara se suelta del planeta
        Vector3 mov = LeerMovimiento();
        if (mov != Vector3.zero && objetivo != null)
        {
            objetivo = null;
            SincronizarAngulos();
        }

        float rueda = LeerRueda();

        if (objetivo != null)
        {
            distancia = Mathf.Clamp(distancia - rueda * distancia * 0.2f, 1.5f, 200f);
            Seguir();
        }
        else
        {
            transform.position += transform.forward * rueda * velocidad * 0.5f;
            VolarLibre(mov);
        }
    }

    // Modo órbita: la cámara sigue al planeta y gira a su alrededor
    void Seguir()
    {
        if (ClicIzquierdo())
        {
            Vector2 r = LeerRaton();
            orbYaw += r.x * sensibilidad;
            orbPitch = Mathf.Clamp(orbPitch - r.y * sensibilidad, -80f, 80f);
        }

        Quaternion giro = Quaternion.Euler(orbPitch, orbYaw, 0f);
        Vector3 destino = objetivo.position + giro * new Vector3(0f, 0f, -distancia);

        transform.position = Vector3.Lerp(transform.position, destino, suavizado * Time.deltaTime);
        Quaternion mirar = Quaternion.LookRotation(objetivo.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, mirar, suavizado * Time.deltaTime);
    }

    // Vuelo libre: W A S D, Q/E y clic derecho para mirar
    void VolarLibre(Vector3 mov)
    {
        if (ClicDerecho())
        {
            Vector2 r = LeerRaton();
            yaw += r.x * sensibilidad;
            pitch = Mathf.Clamp(pitch - r.y * sensibilidad, -89f, 89f);
        }
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        float v = velocidad * (Shift() ? 3f : 1f);
        transform.position += (transform.right * mov.x + transform.up * mov.y + transform.forward * mov.z) * v * Time.deltaTime;
    }

    // ---------- Lectura de teclado y mouse ----------

    Vector3 LeerMovimiento()
    {
        float x = 0f, y = 0f, z = 0f;
#if ENABLE_INPUT_SYSTEM
        Keyboard k = Keyboard.current;
        if (k == null) return Vector3.zero;
        if (k.wKey.isPressed) z += 1f;
        if (k.sKey.isPressed) z -= 1f;
        if (k.dKey.isPressed) x += 1f;
        if (k.aKey.isPressed) x -= 1f;
        if (k.eKey.isPressed) y += 1f;
        if (k.qKey.isPressed) y -= 1f;
#else
        if (Input.GetKey(KeyCode.W)) z += 1f;
        if (Input.GetKey(KeyCode.S)) z -= 1f;
        if (Input.GetKey(KeyCode.D)) x += 1f;
        if (Input.GetKey(KeyCode.A)) x -= 1f;
        if (Input.GetKey(KeyCode.E)) y += 1f;
        if (Input.GetKey(KeyCode.Q)) y -= 1f;
#endif
        return new Vector3(x, y, z);
    }

    bool ClicDerecho()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null && Mouse.current.rightButton.isPressed;
#else
        return Input.GetMouseButton(1);
#endif
    }

    bool ClicIzquierdo()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null && Mouse.current.leftButton.isPressed;
#else
        return Input.GetMouseButton(0);
#endif
    }

    Vector2 LeerRaton()
    {
#if ENABLE_INPUT_SYSTEM
        if (Mouse.current == null) return Vector2.zero;
        return Mouse.current.delta.ReadValue() * 0.1f;
#else
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 2f;
#endif
    }

    float LeerRueda()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null ? Mouse.current.scroll.ReadValue().y / 120f : 0f;
#else
        return Input.mouseScrollDelta.y;
#endif
    }

    bool Shift()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
#else
        return Input.GetKey(KeyCode.LeftShift);
#endif
    }

    int LeerNumero()
    {
#if ENABLE_INPUT_SYSTEM
        Keyboard k = Keyboard.current;
        if (k == null) return -1;
        Key[] teclas = { Key.Digit0, Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4,
                         Key.Digit5, Key.Digit6, Key.Digit7, Key.Digit8 };
        for (int i = 0; i < teclas.Length; i++)
            if (k[teclas[i]].wasPressedThisFrame) return i;
#else
        for (int i = 0; i <= 8; i++)
            if (Input.GetKeyDown(KeyCode.Alpha0 + i)) return i;
#endif
        return -1;
    }
}