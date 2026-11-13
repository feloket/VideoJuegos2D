/*
Autor:psotoc
Fecha:25/10/2024
Licencia:Open estudiantil
Funcionalidad: mueve el objeto de iz. a dr. y con la espaciadora salta.
Modificación: Clase que rota personaje
*/
using UnityEngine;

public class Flip : MonoBehaviour
{
    private bool facingRight = true;

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

    private void FlipSprite()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
