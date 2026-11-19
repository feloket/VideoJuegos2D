/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Liana que se balancea como péndulo (decorativa). El pivote del sprite está
              arriba, así que rota desde donde cuelga. Aquí "amplitud" son grados.
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [HERENCIA], [OBJETO], [OVERRIDE].
*/
using UnityEngine;

// [HERENCIA] hereda de ObjetoMovil
// [OBJETO] 4 objetos en la escena (Liana)
public class LianaPendulo : ObjetoMovil
{
    // [OVERRIDE] usa la oscilación como ángulo de rotación
    protected override void Moverse()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Oscilacion());
    }
}
