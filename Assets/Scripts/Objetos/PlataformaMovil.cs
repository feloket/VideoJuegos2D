/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Plataforma de madera que va y viene sobre un foso. Si el jugador se para
              encima, lo "lleva" (lo hace hijo de la plataforma mientras está arriba).
*/
using UnityEngine;

public class PlataformaMovil : ObjetoMovil
{
    public Vector2 direccion = Vector2.right; // Eje en que se mueve (derecha = horizontal)

    protected override void Moverse()
    {
        transform.position = posicionInicial + (Vector3)(direccion.normalized * Oscilacion());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Solo lo lleva si el jugador está encima de la plataforma
        if (collision.gameObject.CompareTag("Player") &&
            collision.transform.position.y > transform.position.y)
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!gameObject.activeInHierarchy) return; // Evita errores al reiniciar la escena

        if (collision.gameObject.CompareTag("Player") && collision.transform.parent == transform)
        {
            collision.transform.SetParent(null);
        }
    }
}
