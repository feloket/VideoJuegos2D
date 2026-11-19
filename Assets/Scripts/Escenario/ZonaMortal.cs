/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Zona invisible bajo los fosos. Si el jugador cae en ella, termina la partida.
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [ATRIBUTO], [HERENCIA], [OBJETO], [COLISIÓN], [INTERACCIÓN].
*/
using UnityEngine;

// [ATRIBUTO] obliga a tener un BoxCollider2D (en modo Trigger)
[RequireComponent(typeof(BoxCollider2D))]
// [HERENCIA] hereda de MonoBehaviour
// [OBJETO] objeto en la escena: ZonaMortal (bajo los fosos)
public class ZonaMortal : MonoBehaviour
{
    // [COLISIÓN] Trigger: Unity entrega el OTRO objeto como parámetro
    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Player"))
        {
            // [INTERACCIÓN] avisa al GameManager (Singleton)
            GameManager.Instancia.FinDelJuego();
        }
    }
}
