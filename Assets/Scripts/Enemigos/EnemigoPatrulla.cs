/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Enemigo que patrulla de izquierda a derecha en el suelo (escorpión).
              Va y vuelve entre su punto inicial y "distancia", en loop.
*/
using UnityEngine;

public class EnemigoPatrulla : Enemigo
{
    public float distancia = 4f;     // Cuánto avanza a la derecha antes de devolverse
    protected int direccion = 1;      // 1 = derecha, -1 = izquierda

    protected override void Mover()
    {
        transform.Translate(Vector2.right * direccion * velocidad * Time.deltaTime);

        // Al llegar a un extremo, cambia de dirección
        if (transform.position.x >= posicionInicial.x + distancia)
        {
            direccion = -1;
        }
        else if (transform.position.x <= posicionInicial.x)
        {
            direccion = 1;
        }

        // El sprite mira hacia donde camina (dibujado mirando a la derecha)
        spriteRenderer.flipX = direccion < 0;
    }
}
