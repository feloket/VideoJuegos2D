/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Clase base (abstracta) de los objetos del escenario que tienen una rutina de
              movimiento propia (plataforma, tesoros, lianas). Entrega a sus hijas la
              posición inicial y una oscilación (seno) que cada una usa a su manera.
*/
using UnityEngine;

public abstract class ObjetoMovil : MonoBehaviour
{
    public float velocidad = 1f;   // Qué tan rápido oscila
    public float amplitud = 1f;    // Cuánto se aleja de su posición inicial
    protected Vector3 posicionInicial;

    protected virtual void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        Moverse();
    }

    // Valor que va de -amplitud a +amplitud y vuelve, en loop
    protected float Oscilacion()
    {
        return Mathf.Sin(Time.time * velocidad) * amplitud;
    }

    // Cada objeto decide cómo usar la oscilación
    protected abstract void Moverse();
}
