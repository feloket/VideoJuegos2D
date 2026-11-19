/*
Autor:psotoc
Fecha:25/10/2024
Licencia:Open estudiantil
Funcionalidad: mueve el objeto de iz. a dr. y con la espaciadora salta.
Modificación: Clase que rota personaje
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [HERENCIA], [ENCAPSULACIÓN], [INTERACCIÓN].
*/
using UnityEngine;

// [HERENCIA] hereda de MonoBehaviour
public class Flip : MonoBehaviour
{
    // [ENCAPSULACIÓN] private: nadie de afuera cambia hacia dónde mira
    private bool facingRight = true;

    // [INTERACCIÓN] método public: lo llama PlayerController
    public void CheckFlip(float moveInput)
    {
        if (moveInput > 0 && !facingRight)
        {
            FlipSprite();
        }
        else if (moveInput < 0 && facingRight)
        {
            FlipSprite();
        }
    }

    // [ENCAPSULACIÓN] método private: solo lo usa Flip
    private void FlipSprite()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
