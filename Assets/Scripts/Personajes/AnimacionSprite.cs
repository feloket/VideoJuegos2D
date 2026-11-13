/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Animación simple por cuadros: recorre una lista de sprites en loop.
              La usan los enemigos (escorpión camina, murciélago aletea).
*/
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimacionSprite : MonoBehaviour
{
    public Sprite[] cuadros;               // Imágenes de la animación, en orden
    public float cuadrosPorSegundo = 6f;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (cuadros == null || cuadros.Length == 0) return;

        int indice = (int)(Time.time * cuadrosPorSegundo) % cuadros.Length;
        spriteRenderer.sprite = cuadros[indice];
    }
}
