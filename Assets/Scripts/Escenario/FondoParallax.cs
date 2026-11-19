/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Fondo dinámico con efecto parallax. Cada capa se mueve a distinta velocidad
              respecto a la cámara (las lejanas más lento) y se repite infinitamente.
              Además puede desplazarse sola (por ejemplo, las nubes).
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [ATRIBUTO], [HERENCIA], [OBJETO], [ENCAPSULACIÓN], [MÉTODO DE UNITY], [COMPONENTE].
*/
using UnityEngine;

// [ATRIBUTO] obliga a tener un SpriteRenderer
[RequireComponent(typeof(SpriteRenderer))]
// [HERENCIA] hereda de MonoBehaviour
// [OBJETO] 4 objetos: Cielo, Nubes, Montanas y templo, Selva (cada uno con su factorParallax)
public class FondoParallax : MonoBehaviour
{
    // [ATRIBUTO] Range: deslizador de 0 a 1 en el Inspector
    [Range(0f, 1f)]
    public float factorParallax = 0.5f;   // 1 = queda fija en pantalla (muy lejos), 0 = se mueve con el suelo
    public float velocidadAutomatica = 0f; // Movimiento propio (nubes), unidades por segundo

    // [ENCAPSULACIÓN] private: datos internos del cálculo
    private Transform camara;
    private float anchoImagen;     // Ancho de UNA copia de la imagen
    private float inicioX;
    private float desplazamientoAutomatico;

    // [MÉTODO DE UNITY] Start: una vez, antes del primer cuadro
    void Start()
    {
        // [OBJETO] Camera.main entrega el objeto cámara principal (tag MainCamera)
        camara = Camera.main.transform;
        inicioX = transform.position.x;

        // El SpriteRenderer está en modo "Tiled" con 3 copias; el ancho de una es el del sprite
        // [COMPONENTE] GetComponent entrega el objeto SpriteRenderer
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        anchoImagen = sr.sprite.bounds.size.x * transform.localScale.x;
    }

    // [MÉTODO DE UNITY] LateUpdate: después de que la cámara se movió
    void LateUpdate()
    {
        desplazamientoAutomatico += velocidadAutomatica * Time.deltaTime;

        // Cuánto se mueve la capa junto con la cámara
        float distancia = camara.position.x * factorParallax + desplazamientoAutomatico;
        transform.position = new Vector3(inicioX + distancia, transform.position.y, transform.position.z);

        // Si la cámara se alejó más de una imagen, se reposiciona la capa (repetición infinita)
        float relativo = camara.position.x * (1f - factorParallax) - desplazamientoAutomatico;
        if (relativo > inicioX + anchoImagen)
        {
            inicioX += anchoImagen;
        }
        else if (relativo < inicioX - anchoImagen)
        {
            inicioX -= anchoImagen;
        }
    }
}
