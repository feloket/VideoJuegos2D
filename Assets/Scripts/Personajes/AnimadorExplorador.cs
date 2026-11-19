/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Cambia el sprite del explorador según su estado: quieto, caminando/corriendo
              (alterna 2 cuadros) o saltando. Animación simple por código, sin Animator.
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [ATRIBUTO], [HERENCIA], [COMPOSICIÓN], [OBJETO], [ENCAPSULACIÓN], [MÉTODO DE UNITY], [INTERACCIÓN].
*/
using UnityEngine;

// [ATRIBUTO] obliga a tener un SpriteRenderer
[RequireComponent(typeof(SpriteRenderer))]
// [HERENCIA] hereda de MonoBehaviour
// [COMPOSICIÓN] es una de las piezas que usa PlayerController
public class AnimadorExplorador : MonoBehaviour
{
    // [OBJETO] campos que guardan objetos Sprite (se asignan en el Inspector)
    public Sprite quieto;
    public Sprite paso1;
    public Sprite paso2;
    public Sprite salto;
    public float cuadrosPorSegundo = 8f;

    // [ENCAPSULACIÓN] private: solo lo usa esta clase
    private SpriteRenderer spriteRenderer;
    private float temporizador;

    // [MÉTODO DE UNITY] Awake: al crearse el objeto
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // [INTERACCIÓN] lo llama PlayerController cada cuadro
    public void Actualizar(float moveInput, bool corriendo, bool enSuelo)
    {
        if (!enSuelo)
        {
            spriteRenderer.sprite = salto;
            return;
        }

        if (Mathf.Abs(moveInput) < 0.01f)
        {
            spriteRenderer.sprite = quieto;
            temporizador = 0f;
            return;
        }

        // Al correr, la animación va más rápido
        float velocidadAnimacion = corriendo ? cuadrosPorSegundo * 1.7f : cuadrosPorSegundo;
        temporizador += Time.deltaTime * velocidadAnimacion;
        spriteRenderer.sprite = ((int)temporizador % 2 == 0) ? paso1 : paso2;
    }
}
