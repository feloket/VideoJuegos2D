/*
Autor:psotoc
Fecha:25/10/2024
Licencia:Open estudiantil
Funcionalidad: mueve el objeto de iz. a dr. y con la espaciadora salta.
Modificación: Clase que aumenta el salto
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [HERENCIA], [OBJETO].
*/
using UnityEngine;

// [HERENCIA] hereda de MonoBehaviour
public class JumpPowerAdjuster : MonoBehaviour
{
    public float jumpForceIncrement = 1f;
    public float minJumpForce = 5f;
    public float maxJumpForce = 20f;

    // [OBJETO] recibe un OBJETO de la clase Jump y cambia su campo jumpForce
    public void AdjustJumpForce(Jump jump)
    {
        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            jump.jumpForce = Mathf.Min(jump.jumpForce + jumpForceIncrement, maxJumpForce);
        }
        else if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            jump.jumpForce = Mathf.Max(jump.jumpForce - jumpForceIncrement, minJumpForce);
        }
    }
}
