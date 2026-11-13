/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Enemigo que rueda hacia la izquierda (tronco). Cuando recorre "distancia"
              vuelve a su punto inicial y repite (loop). El jugador debe saltarlo.
*/
using UnityEngine;

public class EnemigoRodante : Enemigo
{
    public float distancia = 15f;   // Recorrido antes de reaparecer
    public float radio = 0.5f;      // Radio del tronco (para que el giro calce con el avance)

    protected override void Mover()
    {
        // Avanza a la izquierda en coordenadas del mundo
        transform.Translate(Vector2.left * velocidad * Time.deltaTime, Space.World);

        // Gira como una rueda: grados = distancia recorrida / perímetro * 360
        float grados = velocidad * Time.deltaTime / (2f * Mathf.PI * radio) * 360f;
        transform.Rotate(0f, 0f, grados);

        // Al terminar el recorrido, vuelve al inicio
        if (transform.position.x <= posicionInicial.x - distancia)
        {
            transform.position = posicionInicial;
        }
    }
}
