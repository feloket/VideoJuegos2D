/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Clase base (abstracta) de los enemigos. Hereda de Personaje.
              Todos los enemigos: tienen velocidad, se mueven solos en loop (no persiguen
              al jugador) y si tocan al jugador terminan la partida.
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [ATRIBUTO], [CLASE ABSTRACTA], [HERENCIA], [ENCAPSULACIÓN], [OVERRIDE], [BASE], [POLIMORFISMO], [MÉTODO ABSTRACTO], [COLISIÓN], [INTERACCIÓN].
*/
using UnityEngine;

// [ATRIBUTO] obliga a que el enemigo tenga un collider
[RequireComponent(typeof(Collider2D))]
// [CLASE ABSTRACTA] no hay "un enemigo genérico": solo sus hijas van en la escena
// [HERENCIA] Enemigo hereda de Personaje (recibe rb, spriteRenderer, Update y Morir)
public abstract class Enemigo : Personaje
{
    // [ENCAPSULACIÓN] public: se ve y se edita en el Inspector
    public float velocidad = 2f;          // Velocidad de su rutina de movimiento
    // [ENCAPSULACIÓN] protected: lo usan las hijas (EnemigoPatrulla, EnemigoVolador, EnemigoRodante)
    protected Vector3 posicionInicial;    // Punto de partida de la rutina

    // [OVERRIDE] sobrescribe el Awake virtual de Personaje
    protected override void Awake()
    {
        // [BASE] primero ejecuta el Awake del padre (Personaje)
        base.Awake();                      // Ejecuta el Awake de Personaje (obtiene rb y sprite)
        posicionInicial = transform.position;
        rb.bodyType = RigidbodyType2D.Kinematic; // Lo movemos por código, no por gravedad
    }

    // El comportamiento de un enemigo es moverse según su rutina
    // [OVERRIDE] implementa el método abstracto de Personaje
    // [POLIMORFISMO] llama a Mover(): cada tipo de enemigo se mueve distinto
    protected override void Comportamiento()
    {
        Mover();
    }

    // Cada tipo de enemigo define su propia rutina de movimiento
    // [MÉTODO ABSTRACTO] cada tipo de enemigo DEBE definir su propia rutina
    protected abstract void Mover();

    // Colisión con el jugador = fin del juego (collider del enemigo en modo Trigger)
    // [COLISIÓN] Trigger: Unity llama este método y entrega el OTRO objeto como parámetro
    // [HERENCIA] los 3 tipos de enemigo heredan esta colisión: se escribe una sola vez
    private void OnTriggerEnter2D(Collider2D otro)
    {
        // [COLISIÓN] CompareTag identifica con qué objeto chocó
        if (otro.CompareTag("Player"))
        {
            // [INTERACCIÓN] avisa al objeto GameManager (Singleton) que se termina el juego
            GameManager.Instancia.FinDelJuego();
        }
    }
}
