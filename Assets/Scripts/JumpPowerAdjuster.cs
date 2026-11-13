/*
Autor:psotoc
Fecha:25/10/2024
Licencia:Open estudiantil
Funcionalidad: mueve el objeto de iz. a dr. y con la espaciadora salta.
Modificación: Clase que aumenta el salto
*/
using UnityEngine;

public class JumpPowerAdjuster : MonoBehaviour
{
    public float jumpForceIncrement = 1f;
    public float minJumpForce = 5f;
    public float maxJumpForce = 20f;

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
