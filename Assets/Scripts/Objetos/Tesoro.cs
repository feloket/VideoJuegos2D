/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Tesoro que flota (sube y baja) y gira. Al tocarlo el jugador lo recoge.
              Si "esIdolo" está marcado, es el ídolo final y recogerlo gana la partida.
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [HERENCIA], [OBJETO], [OVERRIDE], [COLISIÓN], [INTERACCIÓN].
*/
using UnityEngine;

// [HERENCIA] hereda de ObjetoMovil
// [OBJETO] 9 objetos de esta clase: Moneda_1 a Moneda_8 e Idolo_Dorado
public class Tesoro : ObjetoMovil
{
    public bool girar = true;        // Las monedas giran, el ídolo no
    public float velocidadGiro = 4f;
    // [OBJETO] mismo código, distinto valor en cada objeto: solo el ídolo tiene true
    public bool esIdolo = false;     // Ídolo dorado = meta del nivel

    // [OVERRIDE] flota y gira
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

    // [COLISIÓN] Trigger: el jugador lo toca
    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (!otro.CompareTag("Player")) return;

        if (esIdolo)
        {
            // [INTERACCIÓN] el ídolo avisa al GameManager que se ganó
            GameManager.Instancia.Victoria();
        }
        else
        {
            // [INTERACCIÓN] la moneda avisa al GameManager que se recogió
            GameManager.Instancia.RecogerTesoro();
        }

        // [OBJETO] destruye ESTE objeto (solo esta moneda)
        Destroy(gameObject);
    }
}
