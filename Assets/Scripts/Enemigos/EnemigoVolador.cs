/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Enemigo volador (murciélago). Hereda de EnemigoPatrulla: reutiliza su ida y
              vuelta horizontal y le agrega un vaivén vertical (onda seno).
              Cadena de herencia: Personaje -> Enemigo -> EnemigoPatrulla -> EnemigoVolador.
*/
using UnityEngine;

public class EnemigoVolador : EnemigoPatrulla
{
    public float amplitud = 1f;     // Cuánto sube y baja
    public float frecuencia = 2f;   // Qué tan rápido sube y baja

    protected override void Mover()
    {
        base.Mover(); // Movimiento horizontal heredado de EnemigoPatrulla

        // Se suma el movimiento vertical propio del volador
        float y = posicionInicial.y + Mathf.Sin(Time.time * frecuencia) * amplitud;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
