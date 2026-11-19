/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Animación simple por cuadros: recorre una lista de sprites en loop.
              La usan los enemigos (escorpión camina, murciélago aletea).
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [ATRIBUTO], [HERENCIA], [OBJETO], [MÉTODO DE UNITY].
*/
using UnityEngine;

// [ATRIBUTO] obliga a tener un SpriteRenderer
[RequireComponent(typeof(SpriteRenderer))]
// [HERENCIA] hereda de MonoBehaviour
// [OBJETO] la llevan Escorpion_1, Escorpion_2 y Murcielago
public class AnimacionSprite : MonoBehaviour
{
    // [OBJETO] arreglo de objetos Sprite
    public Sprite[] cuadros;               // Imágenes de la animación, en orden
    public float cuadrosPorSegundo = 6f;

    private SpriteRenderer spriteRenderer;

    // [MÉTODO DE UNITY] Awake: al crearse el objeto
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // [MÉTODO DE UNITY] Update: cada cuadro
    void Update()
    {
        if (cuadros == null || cuadros.Length == 0) return;

        int indice = (int)(Time.time * cuadrosPorSegundo) % cuadros.Length;
        spriteRenderer.sprite = cuadros[indice];
    }
}
