/*
Autor:psotoc
Fecha:25/10/2024
Licencia:Open estudiantil
Funcionalidad: mueve el objeto de iz. a dr. y con la espaciadora salta.
Modificación: Clase que se asigna al objeto de juego
Modificación (Daniel García, 07/11/2026): ahora hereda de Personaje. El input se lee en
              Comportamiento() (lo llama Personaje solo si la partida está en curso),
              se agrega correr con Shift y la animación del explorador.
*/
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Jump))]
[RequireComponent(typeof(Flip))]
[RequireComponent(typeof(JumpPowerAdjuster))]
[RequireComponent(typeof(AnimadorExplorador))]

public class PlayerController : Personaje
{
    private Movement movement;
    private Jump jump;
    private Flip flip;
    private JumpPowerAdjuster jumpPowerAdjuster;
    private AnimadorExplorador animador;

    void Start()
    {
        // rb y spriteRenderer los obtiene la clase padre (Personaje) en Awake()
        if (rb == null) Debug.LogError("Rigidbody2D no asignado");
        rb.freezeRotation = true;

        movement = GetComponent<Movement>();
        if (movement == null) Debug.LogError("Movement no asignado");

        jump = GetComponent<Jump>();
        if (jump == null) Debug.LogError("Jump no asignado");

        flip = GetComponent<Flip>();
        if (flip == null) Debug.LogError("Flip no asignado");

        jumpPowerAdjuster = GetComponent<JumpPowerAdjuster>();
        if (jumpPowerAdjuster == null) Debug.LogError("JumpPowerAdjuster no asignado");

        animador = GetComponent<AnimadorExplorador>();
        if (animador == null) Debug.LogError("AnimadorExplorador no asignado");
    }

    // Antes era Update(); ahora es la implementación del método abstracto de Personaje
    protected override void Comportamiento()
    {
        // Movimiento horizontal (GetAxisRaw: -1, 0 o 1, sin aceleración)
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Correr manteniendo Shift
        bool corriendo = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        movement.Move(rb, moveInput, corriendo);

        // Verificar la dirección en la que mira el personaje
        flip.CheckFlip(moveInput);

        // Intentar saltar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump.TryJump(rb);
        }

        // Ajustar la potencia de salto
        jumpPowerAdjuster.AdjustJumpForce(jump);

        // Actualizar el dibujo según lo que está haciendo
        animador.Actualizar(moveInput, corriendo, jump.EstaEnSuelo);
    }

    // Sobrescribe Morir() de Personaje: además de morir, se detiene y se tiñe de rojo
    public override void Morir()
    {
        base.Morir();
        rb.velocity = Vector2.zero;
        spriteRenderer.color = new Color(1f, 0.45f, 0.45f);
    }
}
