/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Clase base (abstracta) de todo lo que "vive" en el juego: el jugador y los enemigos.
              Define CUÁNDO actúa un personaje (solo si está vivo y la partida está en curso);
              cada clase hija define CÓMO actúa sobrescribiendo Comportamiento().
*/
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Personaje : MonoBehaviour
{
    protected Rigidbody2D rb;               // Física del personaje (compartida por todas las hijas)
    protected SpriteRenderer spriteRenderer; // Dibujo del personaje
    protected bool estaVivo = true;

    public bool EstaVivo
    {
        get { return estaVivo; }
    }

    // virtual: las clases hijas pueden ampliar Awake llamando a base.Awake()
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update vive SOLO en la clase padre (patrón "método plantilla")
    void Update()
    {
        if (!estaVivo) return;
        if (GameManager.Instancia != null && !GameManager.Instancia.EstaJugando) return;

        Comportamiento();
    }

    // abstract: obligatorio implementarlo en cada clase hija
    protected abstract void Comportamiento();

    // virtual: comportamiento por defecto al morir, las hijas lo pueden cambiar
    public virtual void Morir()
    {
        estaVivo = false;
    }
}
