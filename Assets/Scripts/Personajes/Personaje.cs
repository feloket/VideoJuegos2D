/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Clase base (abstracta) de todo lo que "vive" en el juego: el jugador y los enemigos.
              Define CUÁNDO actúa un personaje (solo si está vivo y la partida está en curso);
              cada clase hija define CÓMO actúa sobrescribiendo Comportamiento().
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [ATRIBUTO], [CLASE ABSTRACTA], [HERENCIA], [ENCAPSULACIÓN], [OBJETO], [PROPIEDAD], [MÉTODO DE UNITY], [MÉTODO VIRTUAL], [COMPONENTE], [POLIMORFISMO], [INTERACCIÓN], [MÉTODO ABSTRACTO].
*/
using UnityEngine;

// [ATRIBUTO] RequireComponent: obliga a que el GameObject tenga estos componentes
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
// [CLASE ABSTRACTA] Personaje es un molde: no se pega a ningún objeto ni se crea con new
// [HERENCIA] Personaje hereda de MonoBehaviour. Sus hijas: PlayerController y Enemigo
public abstract class Personaje : MonoBehaviour
{
    // [ENCAPSULACIÓN] protected: lo ven esta clase y sus hijas, nadie más
    // [OBJETO] rb y spriteRenderer guardan referencias a OBJETOS (componentes) de este GameObject
    protected Rigidbody2D rb;               // Física del personaje (compartida por todas las hijas)
    protected SpriteRenderer spriteRenderer; // Dibujo del personaje
    protected bool estaVivo = true;

    // [PROPIEDAD] solo get: otros leen estaVivo, pero solo esta clase lo cambia
    public bool EstaVivo
    {
        get { return estaVivo; }
    }

    // virtual: las clases hijas pueden ampliar Awake llamando a base.Awake()
    // [MÉTODO DE UNITY] Awake: se ejecuta al crearse el objeto, antes que todo
    // [MÉTODO VIRTUAL] tiene código y las hijas lo pueden ampliar con override (ver Enemigo)
    protected virtual void Awake()
    {
        // [COMPONENTE] GetComponent busca el OBJETO Rigidbody2D en este mismo GameObject
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update vive SOLO en la clase padre (patrón "método plantilla")
    // [MÉTODO DE UNITY] Update: se ejecuta cada cuadro
    // [POLIMORFISMO] llama a Comportamiento(): el jugador lee el teclado, el enemigo se mueve
    void Update()
    {
        if (!estaVivo) return;
        // [INTERACCIÓN] le pregunta al objeto GameManager (Singleton) si la partida está en curso
        if (GameManager.Instancia != null && !GameManager.Instancia.EstaJugando) return;

        Comportamiento();
    }

    // abstract: obligatorio implementarlo en cada clase hija
    // [MÉTODO ABSTRACTO] sin código: cada hija DEBE implementarlo con override
    protected abstract void Comportamiento();

    // virtual: comportamiento por defecto al morir, las hijas lo pueden cambiar
    // [MÉTODO VIRTUAL] versión por defecto; PlayerController la sobrescribe
    public virtual void Morir()
    {
        estaVivo = false;
    }
}
