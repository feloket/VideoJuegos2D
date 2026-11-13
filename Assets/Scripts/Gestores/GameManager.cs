/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Controla el estado de la partida: pantalla de inicio, jugando, fin del juego
              (GAME OVER) y victoria. Lleva la cuenta de tesoros y muestra los paneles.
              Es un "Singleton": hay uno solo y se accede con GameManager.Instancia.
*/
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum EstadoJuego
{
    Inicio,
    Jugando,
    FinDelJuego,
    Victoria
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia { get; private set; }

    public PlayerController jugador;
    public GameObject panelInicio;
    public GameObject panelFinDelJuego;
    public GameObject panelVictoria;
    public Text textoTesoros;

    private EstadoJuego estado;
    private int tesorosRecogidos = 0;
    private int tesorosTotales = 0;

    public bool EstaJugando
    {
        get { return estado == EstadoJuego.Jugando; }
    }

    void Awake()
    {
        Instancia = this;
    }

    void Start()
    {
        // Cuenta las monedas del nivel (el ídolo no cuenta como moneda)
        foreach (Tesoro t in FindObjectsOfType<Tesoro>())
        {
            if (!t.esIdolo) tesorosTotales++;
        }
        ActualizarTextoTesoros();
        CambiarEstado(EstadoJuego.Inicio);
    }

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
    public void FinDelJuego()
    {
        if (!EstaJugando) return;

        jugador.Morir();
        AudioManager.Instancia.DetenerMusica();
        AudioManager.Instancia.Reproducir(AudioManager.Instancia.sonidoMuerte);
        CambiarEstado(EstadoJuego.FinDelJuego);
    }

    // Lo llama el ídolo dorado al recogerlo
    public void Victoria()
    {
        if (!EstaJugando) return;

        AudioManager.Instancia.DetenerMusica();
        AudioManager.Instancia.Reproducir(AudioManager.Instancia.sonidoVictoria);
        CambiarEstado(EstadoJuego.Victoria);
    }

    // Lo llaman las monedas al recogerlas
    public void RecogerTesoro()
    {
        tesorosRecogidos++;
        AudioManager.Instancia.Reproducir(AudioManager.Instancia.sonidoTesoro);
        ActualizarTextoTesoros();
    }

    private void ActualizarTextoTesoros()
    {
        if (textoTesoros != null)
        {
            textoTesoros.text = "Tesoros: " + tesorosRecogidos + " / " + tesorosTotales;
        }
    }
}
