/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Enemigo que patrulla de izquierda a derecha en el suelo (escorpión).
              Va y vuelve entre su punto inicial y "distancia", en loop.
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [HERENCIA], [OBJETO], [ENCAPSULACIÓN], [OVERRIDE].
*/
using UnityEngine;

// [HERENCIA] EnemigoPatrulla hereda de Enemigo, que hereda de Personaje
// [OBJETO] en la escena hay 2 objetos de esta clase: Escorpion_1 y Escorpion_2, cada uno con sus propios valores
public class EnemigoPatrulla : Enemigo
{
    public float distancia = 4f;     // Cuánto avanza a la derecha antes de devolverse
    // [ENCAPSULACIÓN] protected: la hereda EnemigoVolador
    protected int direccion = 1;      // 1 = derecha, -1 = izquierda

    // [OVERRIDE] implementa el Mover() abstracto de Enemigo: va y vuelve
    protected override void Mover()
    {
        // [HERENCIA] velocidad y posicionInicial vienen de Enemigo
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
        // [HERENCIA] spriteRenderer viene de Personaje
        spriteRenderer.flipX = direccion < 0;
    }
}
