/*
Autor:psotoc
Fecha:25/10/2024
Licencia:Open estudiantil
Funcionalidad: mueve el objeto de iz. a dr. y con la espaciadora salta.
Modificación: Clase que hace saltar al personaje
Modificación (Daniel García, 07/11/2026): sonido de salto, solo cuenta como suelo si se
              aterriza ENCIMA (no al chocar de lado) y se pierde el suelo al dejarlo.
*/
using UnityEngine;

public class Jump : MonoBehaviour
{
    public float jumpForce = 10f; // Fuerza del salto inicial
    private bool isGrounded=true;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public bool EstaEnSuelo
    {
        get { return isGrounded; }
    }

    public void TryJump(Rigidbody2D rb)
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f); // Salto siempre de la misma altura
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;

            if (AudioManager.Instancia != null)
            {
                AudioManager.Instancia.Reproducir(AudioManager.Instancia.sonidoSalto);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RevisarSuelo(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        RevisarSuelo(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo_01"))
        {
            isGrounded = false;
        }
    }

    private void RevisarSuelo(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Suelo_01")) return;

        // Si todavía va subiendo (recién saltó), aún no ha aterrizado
        if (rb != null && rb.velocity.y > 0.1f) return;

        // La normal apunta hacia arriba cuando estamos parados SOBRE el objeto
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
