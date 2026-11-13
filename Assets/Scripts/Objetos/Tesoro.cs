/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Tesoro que flota (sube y baja) y gira. Al tocarlo el jugador lo recoge.
              Si "esIdolo" está marcado, es el ídolo final y recogerlo gana la partida.
*/
using UnityEngine;

public class Tesoro : ObjetoMovil
{
    public bool girar = true;        // Las monedas giran, el ídolo no
    public float velocidadGiro = 4f;
    public bool esIdolo = false;     // Ídolo dorado = meta del nivel

    protected override void Moverse()
    {
        // Flota arriba y abajo
        transform.position = posicionInicial + Vector3.up * Oscilacion();

        // Efecto de moneda girando: achica y agranda su ancho
        if (girar)
        {
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Cos(Time.time * velocidadGiro);
            transform.localScale = escala;
        }
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (!otro.CompareTag("Player")) return;

        if (esIdolo)
        {
            GameManager.Instancia.Victoria();
        }
        else
        {
            GameManager.Instancia.RecogerTesoro();
        }

        Destroy(gameObject);
    }
}
