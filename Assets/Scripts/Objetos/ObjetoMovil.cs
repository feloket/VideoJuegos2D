/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Clase base (abstracta) de los objetos del escenario que tienen una rutina de
              movimiento propia (plataforma, tesoros, lianas). Entrega a sus hijas la
              posición inicial y una oscilación (seno) que cada una usa a su manera.
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [CLASE ABSTRACTA], [HERENCIA], [ENCAPSULACIÓN], [MÉTODO DE UNITY], [MÉTODO VIRTUAL], [POLIMORFISMO], [MÉTODO ABSTRACTO].
*/
using UnityEngine;

// [CLASE ABSTRACTA] molde para los objetos que se mueven solos
// [HERENCIA] hereda de MonoBehaviour. Sus hijas: PlataformaMovil, Tesoro y LianaPendulo
public abstract class ObjetoMovil : MonoBehaviour
{
    // [ENCAPSULACIÓN] public: se edita en el Inspector
    public float velocidad = 1f;   // Qué tan rápido oscila
    public float amplitud = 1f;    // Cuánto se aleja de su posición inicial
    // [ENCAPSULACIÓN] protected: lo usan las hijas
    protected Vector3 posicionInicial;

    // [MÉTODO DE UNITY] Start: una vez, antes del primer cuadro
    // [MÉTODO VIRTUAL] las hijas lo podrían ampliar
    protected virtual void Start()
    {
        posicionInicial = transform.position;
    }

    // [MÉTODO DE UNITY] Update: cada cuadro
    // [POLIMORFISMO] llama a Moverse(): cada hija se mueve distinto
    void Update()
    {
        Moverse();
    }

    // Valor que va de -amplitud a +amplitud y vuelve, en loop
    // [HERENCIA] método que heredan y usan las 3 hijas
    protected float Oscilacion()
    {
        return Mathf.Sin(Time.time * velocidad) * amplitud;
    }

    // Cada objeto decide cómo usar la oscilación
    // [MÉTODO ABSTRACTO] cada hija DEBE implementarlo
    protected abstract void Moverse();
}
