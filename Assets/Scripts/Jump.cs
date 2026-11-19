/*
Autor:psotoc
Fecha:25/10/2024
Licencia:Open estudiantil
Funcionalidad: mueve el objeto de iz. a dr. y con la espaciadora salta.
Modificación: Clase que hace saltar al personaje
Modificación (Daniel García, 07/11/2026): sonido de salto, solo cuenta como suelo si se
              aterriza ENCIMA (no al chocar de lado) y se pierde el suelo al dejarlo.
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [HERENCIA], [ENCAPSULACIÓN], [MÉTODO DE UNITY], [PROPIEDAD], [OBJETO], [INTERACCIÓN], [COLISIÓN].
*/
using UnityEngine;

// [HERENCIA] hereda de MonoBehaviour
public class Jump : MonoBehaviour
{
    // [ENCAPSULACIÓN] public: lo cambia JumpPowerAdjuster
    public float jumpForce = 10f; // Fuerza del salto inicial
    // [ENCAPSULACIÓN] private: solo Jump lo cambia; los demás lo leen con EstaEnSuelo
    private bool isGrounded=true;
    private Rigidbody2D rb;

    // [MÉTODO DE UNITY] Awake: al crearse el objeto
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // [PROPIEDAD] solo get: la usa PlayerController para la animación
    public bool EstaEnSuelo
    {
        get { return isGrounded; }
    }

    // [OBJETO] recibe el OBJETO Rigidbody2D como parámetro
    public void TryJump(Rigidbody2D rb)
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f); // Salto siempre de la misma altura
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;

            if (AudioManager.Instancia != null)
            {
                // [INTERACCIÓN] usa el objeto AudioManager (Singleton) para el sonido
                AudioManager.Instancia.Reproducir(AudioManager.Instancia.sonidoSalto);
            }
        }
    }

    // [COLISIÓN] choque sólido: al tocar algo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        RevisarSuelo(collision);
    }

    // [COLISIÓN] mientras sigue tocando
    private void OnCollisionStay2D(Collision2D collision)
    {
        RevisarSuelo(collision);
    }

    // [COLISIÓN] al dejar de tocar
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo_01"))
        {
            isGrounded = false;
        }
    }

    // [ENCAPSULACIÓN] método private: solo lo usa Jump
    private void RevisarSuelo(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Suelo_01")) return;

        // Si todavía va subiendo (recién saltó), aún no ha aterrizado
        if (rb != null && rb.velocity.y > 0.1f) return;

        // La normal apunta hacia arriba cuando estamos parados SOBRE el objeto
        // [OBJETO] cada contacto es un objeto ContactPoint2D
        foreach (ContactPoint2D contacto in collision.contacts)
        {
            if (contacto.normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }
}
