/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Maneja el audio del juego: música de fondo en loop y efectos de sonido
              (salto, muerte, tesoro, victoria). Usa dos AudioSource: uno para la música
              y otro para los efectos. Singleton: se accede con AudioManager.Instancia.
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [HERENCIA], [STATIC], [SINGLETON], [PROPIEDAD], [ATRIBUTO], [ENCAPSULACIÓN], [COMPOSICIÓN], [MÉTODO DE UNITY], [OBJETO], [INTERACCIÓN].
*/
using UnityEngine;

// [HERENCIA] hereda de MonoBehaviour
public class AudioManager : MonoBehaviour
{
    // [STATIC] pertenece a la clase, no a un objeto: se usa como AudioManager.Instancia
    // [SINGLETON] guarda el único objeto AudioManager para que cualquier clase lo use
    // [PROPIEDAD] get público, set privado: todos leen, solo esta clase asigna
    public static AudioManager Instancia { get; private set; }

    // [ATRIBUTO] Header pone un título en el Inspector; Range pone un deslizador
    [Header("Música")]
    public AudioClip musicaFondo;
    [Range(0f, 1f)] public float volumenMusica = 0.45f;

    [Header("Efectos")]
    public AudioClip sonidoSalto;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoTesoro;
    public AudioClip sonidoVictoria;
    [Range(0f, 1f)] public float volumenEfectos = 0.8f;

    // [ENCAPSULACIÓN] private: nadie de afuera toca los AudioSource
    // [COMPOSICIÓN] AudioManager TIENE dos objetos AudioSource
    private AudioSource fuenteMusica;
    private AudioSource fuenteEfectos;

    // [MÉTODO DE UNITY] Awake: al crearse el objeto
    void Awake()
    {
        // [SINGLETON] this = este objeto; queda guardado en Instancia
        Instancia = this;

        // [OBJETO] AddComponent crea un objeto AudioSource por código (los componentes no se crean con new)
        fuenteMusica = gameObject.AddComponent<AudioSource>();
        fuenteMusica.clip = musicaFondo;
        fuenteMusica.loop = true;
        fuenteMusica.volume = volumenMusica;

        fuenteEfectos = gameObject.AddComponent<AudioSource>();
        fuenteEfectos.volume = volumenEfectos;
    }

    // [MÉTODO DE UNITY] Start: empieza la música
    void Start()
    {
        fuenteMusica.Play();
    }

    // [OBJETO] recibe un objeto AudioClip
    // [INTERACCIÓN] lo llaman Jump y GameManager
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
