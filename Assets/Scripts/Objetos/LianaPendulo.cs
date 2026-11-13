/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Liana que se balancea como péndulo (decorativa). El pivote del sprite está
              arriba, así que rota desde donde cuelga. Aquí "amplitud" son grados.
*/
using UnityEngine;

public class LianaPendulo : ObjetoMovil
{
    protected override void Moverse()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Oscilacion());
    }
}
