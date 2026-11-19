/*
Autor:psotoc
Fecha:25/10/2024
Licencia:Open estudiantil
Funcionalidad: mueve el objeto de iz. a dr. y con la espaciadora salta.
Modificación: Clase que se asigna al objeto de juego
Modificación (Daniel García, 07/11/2026): ahora hereda de Personaje. El input se lee en
              Comportamiento() (lo llama Personaje solo si la partida está en curso),
              se agrega correr con Shift y la animación del explorador.
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [ATRIBUTO], [COMPOSICIÓN], [HERENCIA], [OBJETO], [ENCAPSULACIÓN], [MÉTODO DE UNITY], [COMPONENTE], [OVERRIDE], [POLIMORFISMO], [SOBRECARGA], [INTERACCIÓN], [BASE].
*/
using UnityEngine;

// [ATRIBUTO] RequireComponent: obliga a tener estos componentes
// [COMPOSICIÓN] el jugador se arma con varias piezas, cada una con una tarea
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Jump))]
[RequireComponent(typeof(Flip))]
[RequireComponent(typeof(JumpPowerAdjuster))]
[RequireComponent(typeof(AnimadorExplorador))]

// [HERENCIA] PlayerController hereda de Personaje (recibe rb, spriteRenderer, Update y Morir)
// [OBJETO] objeto en la escena: Explorador
public class PlayerController : Personaje
{
    // [COMPOSICIÓN] PlayerController TIENE un Movement, un Jump, un Flip...: cada uno es un OBJETO
    // [ENCAPSULACIÓN] private: solo esta clase los usa
    private Movement movement;
    private Jump jump;
    private Flip flip;
    private JumpPowerAdjuster jumpPowerAdjuster;
    private AnimadorExplorador animador;

    // [MÉTODO DE UNITY] Start: una vez, antes del primer cuadro
    void Start()
    {
        // rb y spriteRenderer los obtiene la clase padre (Personaje) en Awake()
        if (rb == null) Debug.LogError("Rigidbody2D no asignado");
        // [HERENCIA] rb viene de Personaje
        rb.freezeRotation = true;

        // [COMPONENTE] GetComponent entrega el OBJETO Movement del mismo GameObject
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
    // [OVERRIDE] implementa el método abstracto de Personaje
    // [POLIMORFISMO] Personaje.Update() llama a Comportamiento() y se ejecuta esta versión
    protected override void Comportamiento()
    {
        // Movimiento horizontal (GetAxisRaw: -1, 0 o 1, sin aceleración)
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Correr manteniendo Shift
        bool corriendo = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        // [SOBRECARGA] usa la versión de Move con 3 parámetros
        // [INTERACCIÓN] le pide al objeto movement que mueva al jugador
        movement.Move(rb, moveInput, corriendo);

        // Verificar la dirección en la que mira el personaje
        flip.CheckFlip(moveInput);

        // Intentar saltar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // [INTERACCIÓN] le pide al objeto jump que salte
            jump.TryJump(rb);
        }

        // Ajustar la potencia de salto
        // [OBJETO] le pasa el OBJETO jump como parámetro
        jumpPowerAdjuster.AdjustJumpForce(jump);

        // Actualizar el dibujo según lo que está haciendo
        animador.Actualizar(moveInput, corriendo, jump.EstaEnSuelo);
    }

    // Sobrescribe Morir() de Personaje: además de morir, se detiene y se tiñe de rojo
    // [OVERRIDE] sobrescribe el Morir() virtual de Personaje
    // [POLIMORFISMO] GameManager llama jugador.Morir() y se ejecuta esta versión
    public override void Morir()
    {
        // [BASE] primero ejecuta el Morir() del padre (estaVivo = false)
        base.Morir();
        rb.velocity = Vector2.zero;
        // [OBJETO] new Color crea un objeto color
        spriteRenderer.color = new Color(1f, 0.45f, 0.45f);
    }
}
