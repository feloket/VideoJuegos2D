/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Maneja el audio del juego: música de fondo en loop y efectos de sonido
              (salto, muerte, tesoro, victoria). Usa dos AudioSource: uno para la música
              y otro para los efectos. Singleton: se accede con AudioManager.Instancia.
*/
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instancia { get; private set; }

    [Header("Música")]
    public AudioClip musicaFondo;
    [Range(0f, 1f)] public float volumenMusica = 0.45f;

    [Header("Efectos")]
    public AudioClip sonidoSalto;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoTesoro;
    public AudioClip sonidoVictoria;
    [Range(0f, 1f)] public float volumenEfectos = 0.8f;

    private AudioSource fuenteMusica;
    private AudioSource fuenteEfectos;

    void Awake()
    {
        Instancia = this;

        fuenteMusica = gameObject.AddComponent<AudioSource>();
        fuenteMusica.clip = musicaFondo;
        fuenteMusica.loop = true;
        fuenteMusica.volume = volumenMusica;

        fuenteEfectos = gameObject.AddComponent<AudioSource>();
        fuenteEfectos.volume = volumenEfectos;
    }

    void Start()
    {
        fuenteMusica.Play();
    }

    public void Reproducir(AudioClip clip)
    {
        if (clip != null)
        {
            fuenteEfectos.PlayOneShot(clip);
        }
    }

    public void DetenerMusica()
    {
        fuenteMusica.Stop();
    }
}
