/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Plataforma de madera que va y viene sobre un foso. Si el jugador se para
              encima, lo "lleva" (lo hace hijo de la plataforma mientras está arriba).
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [HERENCIA], [OBJETO], [OVERRIDE], [COLISIÓN].
*/
using UnityEngine;

// [HERENCIA] hereda de ObjetoMovil
// [OBJETO] objeto en la escena: PlataformaMovil
public class PlataformaMovil : ObjetoMovil
{
    public Vector2 direccion = Vector2.right; // Eje en que se mueve (derecha = horizontal)

    // [OVERRIDE] usa la oscilación como desplazamiento horizontal
    protected override void Moverse()
    {
        transform.position = posicionInicial + (Vector3)(direccion.normalized * Oscilacion());
    }

    // [COLISIÓN] choque sólido: el jugador se para encima
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Solo lo lleva si el jugador está encima de la plataforma
        if (collision.gameObject.CompareTag("Player") &&
            collision.transform.position.y > transform.position.y)
        {
            // [OBJETO] el jugador pasa a ser hijo de este objeto y se mueve con él
            collision.transform.SetParent(transform);
        }
    }

    // [COLISIÓN] el jugador se baja: se suelta
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!gameObject.activeInHierarchy) return; // Evita errores al reiniciar la escena

        if (collision.gameObject.CompareTag("Player") && collision.transform.parent == transform)
        {
            collision.transform.SetParent(null);
        }
    }
}
