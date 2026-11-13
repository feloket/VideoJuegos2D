/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Clase base (abstracta) de los enemigos. Hereda de Personaje.
              Todos los enemigos: tienen velocidad, se mueven solos en loop (no persiguen
              al jugador) y si tocan al jugador terminan la partida.
*/
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Enemigo : Personaje
{
    public float velocidad = 2f;          // Velocidad de su rutina de movimiento
    protected Vector3 posicionInicial;    // Punto de partida de la rutina

    protected override void Awake()
    {
        base.Awake();                      // Ejecuta el Awake de Personaje (obtiene rb y sprite)
        posicionInicial = transform.position;
        rb.bodyType = RigidbodyType2D.Kinematic; // Lo movemos por código, no por gravedad
    }

    // El comportamiento de un enemigo es moverse según su rutina
    protected override void Comportamiento()
    {
        Mover();
    }

    // Cada tipo de enemigo define su propia rutina de movimiento
    protected abstract void Mover();

    // Colisión con el jugador = fin del juego (collider del enemigo en modo Trigger)
    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Player"))
        {
            GameManager.Instancia.FinDelJuego();
        }
    }
}
