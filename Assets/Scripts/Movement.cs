/*
Autor:psotoc
Fecha:25/10/2024
Licencia:Open estudiantil
Funcionalidad: mueve el objeto de iz. a dr. y con la espaciadora salta.
Modificación: Clase que realiza el movimiento
Modificación (Daniel García, 07/11/2026): se agrega la velocidad de carrera (correr).
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [HERENCIA], [SOBRECARGA], [OBJETO].
*/
using UnityEngine;

// [HERENCIA] hereda de MonoBehaviour
public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Velocidad de movimiento horizontal (caminar)
    public float runSpeed = 9f;   // Velocidad al correr (Shift)

    // Versión original: camina
    // [SOBRECARGA] versión 1: 2 parámetros (camina)
    public void Move(Rigidbody2D rb, float moveInput)
    {
        Move(rb, moveInput, false);
    }

    // Sobrecarga: camina o corre
    // [SOBRECARGA] versión 2: mismo nombre, 3 parámetros
    // [OBJETO] recibe el OBJETO Rigidbody2D del jugador como parámetro
    public void Move(Rigidbody2D rb, float moveInput, bool corriendo)
    {
        float velocidad = corriendo ? runSpeed : moveSpeed;
        // [OBJETO] new Vector2 crea el valor de la nueva velocidad
        rb.velocity = new Vector2(moveInput * velocidad, rb.velocity.y);
    }
}
