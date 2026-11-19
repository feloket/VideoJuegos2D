/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Enemigo volador (murciélago). Hereda de EnemigoPatrulla: reutiliza su ida y
              vuelta horizontal y le agrega un vaivén vertical (onda seno).
              Cadena de herencia: Personaje -> Enemigo -> EnemigoPatrulla -> EnemigoVolador.
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [HERENCIA], [OBJETO], [OVERRIDE], [POLIMORFISMO], [BASE].
*/
using UnityEngine;

// [HERENCIA] cadena de 3 niveles: EnemigoVolador -> EnemigoPatrulla -> Enemigo -> Personaje
// [OBJETO] objeto en la escena: Murcielago
public class EnemigoVolador : EnemigoPatrulla
{
    public float amplitud = 1f;     // Cuánto sube y baja
    public float frecuencia = 2f;   // Qué tan rápido sube y baja

    // [OVERRIDE] vuelve a sobrescribir Mover()
    // [POLIMORFISMO] mismo método Mover(), comportamiento distinto
    protected override void Mover()
    {
        // [BASE] reutiliza el Mover() del padre (la patrulla horizontal)
        base.Mover(); // Movimiento horizontal heredado de EnemigoPatrulla

        // Se suma el movimiento vertical propio del volador
        // [HERENCIA] posicionInicial viene de Enemigo
        float y = posicionInicial.y + Mathf.Sin(Time.time * frecuencia) * amplitud;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
