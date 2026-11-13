/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Zona invisible bajo los fosos. Si el jugador cae en ella, termina la partida.
*/
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ZonaMortal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Player"))
        {
            GameManager.Instancia.FinDelJuego();
        }
    }
}
