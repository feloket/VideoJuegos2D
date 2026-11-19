/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: La cámara sigue al jugador en el eje X de forma suave, sin salirse del nivel.
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [HERENCIA], [OBJETO], [MÉTODO DE UNITY].
*/
using UnityEngine;

// [HERENCIA] hereda de MonoBehaviour
// [OBJETO] objeto en la escena: Main Camera
public class CamaraSeguimiento : MonoBehaviour
{
    // [OBJETO] referencia al objeto Transform del Explorador (se asigna en el Inspector)
    public Transform objetivo;        // El jugador
    public float suavizado = 5f;      // Más alto = sigue más pegada
    public float adelanto = 2f;       // Muestra un poco más hacia adelante
    public float limiteIzquierdo = 0f;
    public float limiteDerecho = 80f;

    // LateUpdate: se ejecuta después de que el jugador ya se movió en este frame
    // [MÉTODO DE UNITY] LateUpdate: cada cuadro, después de todos los Update
    void LateUpdate()
    {
        if (objetivo == null) return;

        float xDeseada = Mathf.Clamp(objetivo.position.x + adelanto, limiteIzquierdo, limiteDerecho);
        float x = Mathf.Lerp(transform.position.x, xDeseada, suavizado * Time.deltaTime);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}
