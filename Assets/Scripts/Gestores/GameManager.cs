/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Controla el estado de la partida: pantalla de inicio, jugando, fin del juego
              (GAME OVER) y victoria. Lleva la cuenta de tesoros y muestra los paneles.
              Es un "Singleton": hay uno solo y se accede con GameManager.Instancia.
Modificación (Daniel García, 19/11/2026): la cuenta de tesoros pasa a una clase anidada
              (inner class) privada: ContadorTesoros.
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [ENUM], [HERENCIA], [OBJETO], [STATIC], [SINGLETON], [PROPIEDAD], [ENCAPSULACIÓN], [COMPOSICIÓN], [INNER CLASS], [CONSTRUCTOR], [THIS], [OVERRIDE], [POLIMORFISMO], [MÉTODO DE UNITY], [INTERACCIÓN].
*/
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// [ENUM] tipo con 4 valores fijos con nombre
public enum EstadoJuego
{
    Inicio,
    Jugando,
    FinDelJuego,
    Victoria
}

// [HERENCIA] hereda de MonoBehaviour
// [OBJETO] objeto en la escena: GameManager (uno solo)
public class GameManager : MonoBehaviour
{
    // [STATIC] pertenece a la clase: se usa como GameManager.Instancia
    // [SINGLETON] guarda el único objeto GameManager para que cualquier clase lo use
    // [PROPIEDAD] get público, set privado
    public static GameManager Instancia { get; private set; }

    // [OBJETO] referencia al OBJETO PlayerController del Explorador (se asigna en el Inspector)
    public PlayerController jugador;
    public GameObject panelInicio;
    public GameObject panelFinDelJuego;
    public GameObject panelVictoria;
    // [OBJETO] referencia al objeto Text del HUD
    public Text textoTesoros;

    // [ENCAPSULACIÓN] private: el estado solo lo cambia GameManager (se lee con EstaJugando)
    private EstadoJuego estado;
    // [OBJETO] objeto de la inner class ContadorTesoros
    // [COMPOSICIÓN] GameManager TIENE un ContadorTesoros
    private ContadorTesoros contador; // Objeto de la clase anidada (se crea en Start)

    // -----------------------------------------------------------------
    // INNER CLASS (clase anidada): ContadorTesoros está declarada DENTRO
    // de GameManager. Es private, así que solo GameManager la puede usar.
    // No hereda de MonoBehaviour: es una clase C# normal y se crea con "new"
    // (no se pega a ningún GameObject).
    // -----------------------------------------------------------------
    // [INNER CLASS] clase anidada: declarada DENTRO de GameManager, private
    private class ContadorTesoros
    {
        // [ENCAPSULACIÓN] private: solo cambia con Sumar()
        private int recogidos;  // Encapsulado: nadie de afuera lo cambia directo
        private int totales;

        // Constructor: se ejecuta al hacer new ContadorTesoros(...)
        // [CONSTRUCTOR] mismo nombre que la clase, sin tipo de retorno; corre al hacer new
        public ContadorTesoros(int totales)
        {
            // [THIS] this.totales es el campo; totales (solo) es el parámetro
            this.totales = totales;
            this.recogidos = 0;
        }

        public void Sumar()
        {
            recogidos++;
        }

        // Sobrescribe ToString() de System.Object (toda clase C# hereda de object)
        // [OVERRIDE] sobrescribe ToString() de object (toda clase C# hereda de object)
        // [POLIMORFISMO] ToString() devuelve un texto distinto según la clase
        public override string ToString()
        {
            return "Tesoros: " + recogidos + " / " + totales;
        }
    }

    // [PROPIEDAD] solo get: Personaje la consulta cada cuadro
    public bool EstaJugando
    {
        get { return estado == EstadoJuego.Jugando; }
    }

    // [MÉTODO DE UNITY] Awake: al crearse el objeto
    void Awake()
    {
        // [SINGLETON] this = este objeto; queda guardado en Instancia
        Instancia = this;
    }

    // [MÉTODO DE UNITY] Start: una vez, antes del primer cuadro
    void Start()
    {
        // Cuenta las monedas del nivel (el ídolo no cuenta como moneda)
        int totalMonedas = 0;
        // [OBJETO] busca todos los OBJETOS de la clase Tesoro en la escena
        foreach (Tesoro t in FindObjectsOfType<Tesoro>())
        {
            if (!t.esIdolo) totalMonedas++;
        }
        // [CONSTRUCTOR] new crea el OBJETO de la inner class y ejecuta su constructor
        contador = new ContadorTesoros(totalMonedas); // Se crea el objeto de la inner class
        ActualizarTextoTesoros();
        CambiarEstado(EstadoJuego.Inicio);
    }

    // [MÉTODO DE UNITY] Update: lee ENTER y R aunque el tiempo esté en 0
    void Update()
    {
        // Iniciar la partida desde la pantalla de inicio
        if (estado == EstadoJuego.Inicio &&
            (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            CambiarEstado(EstadoJuego.Jugando);
        }

        // Reiniciar cuando terminó (perdió o ganó)
        if ((estado == EstadoJuego.FinDelJuego || estado == EstadoJuego.Victoria) &&
            Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // [ENCAPSULACIÓN] método private: solo GameManager cambia el estado
    private void CambiarEstado(EstadoJuego nuevo)
    {
        estado = nuevo;

        panelInicio.SetActive(estado == EstadoJuego.Inicio);
        panelFinDelJuego.SetActive(estado == EstadoJuego.FinDelJuego);
        panelVictoria.SetActive(estado == EstadoJuego.Victoria);

        // Solo corre el tiempo (física, enemigos) mientras se juega
        Time.timeScale = (estado == EstadoJuego.Jugando) ? 1f : 0f;
    }

    // Lo llaman los enemigos y la zona mortal
    // [INTERACCIÓN] lo llaman Enemigo y ZonaMortal
    public void FinDelJuego()
    {
        if (!EstaJugando) return;

        // [POLIMORFISMO] Morir() está en Personaje, pero se ejecuta la versión override de PlayerController
        jugador.Morir();
        AudioManager.Instancia.DetenerMusica();
        AudioManager.Instancia.Reproducir(AudioManager.Instancia.sonidoMuerte);
        CambiarEstado(EstadoJuego.FinDelJuego);
    }

    // Lo llama el ídolo dorado al recogerlo
    // [INTERACCIÓN] lo llama Tesoro cuando es el ídolo
    public void Victoria()
    {
        if (!EstaJugando) return;

        AudioManager.Instancia.DetenerMusica();
        AudioManager.Instancia.Reproducir(AudioManager.Instancia.sonidoVictoria);
        CambiarEstado(EstadoJuego.Victoria);
    }

    // Lo llaman las monedas al recogerlas
    // [INTERACCIÓN] lo llama Tesoro cuando es una moneda
    public void RecogerTesoro()
    {
        // [ENCAPSULACIÓN] no toca recogidos directo: usa el método Sumar()
        contador.Sumar();
        AudioManager.Instancia.Reproducir(AudioManager.Instancia.sonidoTesoro);
        ActualizarTextoTesoros();
    }

    private void ActualizarTextoTesoros()
    {
        if (textoTesoros != null)
        {
            // [POLIMORFISMO] llama al ToString() sobrescrito
            textoTesoros.text = contador.ToString();
        }
    }
}
