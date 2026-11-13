/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: La cámara sigue al jugador en el eje X de forma suave, sin salirse del nivel.
*/
using UnityEngine;

public class CamaraSeguimiento : MonoBehaviour
{
    public Transform objetivo;        // El jugador
    public float suavizado = 5f;      // Más alto = sigue más pegada
    public float adelanto = 2f;       // Muestra un poco más hacia adelante
    public float limiteIzquierdo = 0f;
    public float limiteDerecho = 80f;

    // LateUpdate: se ejecuta después de que el jugador ya se movió en este frame
    void LateUpdate()
    {
        if (objetivo == null) return;

        float xDeseada = Mathf.Clamp(objetivo.position.x + adelanto, limiteIzquierdo, limiteDerecho);
        float x = Mathf.Lerp(transform.position.x, xDeseada, suavizado * Time.deltaTime);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}
