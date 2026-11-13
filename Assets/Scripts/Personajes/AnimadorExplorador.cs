/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Cambia el sprite del explorador según su estado: quieto, caminando/corriendo
              (alterna 2 cuadros) o saltando. Animación simple por código, sin Animator.
*/
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimadorExplorador : MonoBehaviour
{
    public Sprite quieto;
    public Sprite paso1;
    public Sprite paso2;
    public Sprite salto;
    public float cuadrosPorSegundo = 8f;

    private SpriteRenderer spriteRenderer;
    private float temporizador;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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
