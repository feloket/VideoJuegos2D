/*
Autor:Daniel García
Fecha:07/11/2026
Licencia:Open estudiantil
Funcionalidad: Templo Perdido - juego de plataformas 2D.
Modificación: Herramienta de EDITOR (no es parte del juego). Agrega el menú
              "Templo Perdido > Construir escena" que configura los sprites y arma la escena
              Assets/Scenes/TemploPerdido.unity con todos los GameObjects ordenados.
              Después de usarla, la escena se puede editar a mano normalmente.
*/
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public static class ConstructorEscena
{
    const string RUTA_SPRITES = "Assets/Sprites/TemploPerdido/";
    const string RUTA_AUDIO = "Assets/Audio/";
    const string RUTA_ESCENA = "Assets/Scenes/TemploPerdido.unity";
    const string RUTA_MATERIAL = "Assets/Fisica/SinFriccion.physicsMaterial2D";
    const float PPU = 16f;
    const float SUELO_Y = -4f; // Altura de la superficie del suelo

    [MenuItem("Templo Perdido/Construir escena")]
    public static void Construir()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        ConfigurarSprites();
        AsegurarTag("Suelo_01");
        PhysicsMaterial2D sinFriccion = CrearMaterialSinFriccion();

        var escena = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // ---------- Carpetas (GameObjects vacíos para ordenar la jerarquía) ----------
        Transform gestores = Carpeta("--- Gestores ---");
        Transform fondo = Carpeta("--- Fondo ---");
        Transform escenario = Carpeta("--- Escenario ---");
        Transform objetos = Carpeta("--- Objetos ---");
        Transform enemigos = Carpeta("--- Enemigos ---");
        Transform jugadorCarpeta = Carpeta("--- Jugador ---");
        Transform ui = Carpeta("--- UI ---");

        // ---------- Cámara ----------
        GameObject camGO = new GameObject("Main Camera");
        camGO.tag = "MainCamera";
        Camera cam = camGO.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 6f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.25f, 0.59f, 0.75f);
        camGO.AddComponent<AudioListener>();
        camGO.transform.position = new Vector3(-1.3f, 0f, -10f);
        CamaraSeguimiento seguimiento = camGO.AddComponent<CamaraSeguimiento>();
        seguimiento.limiteIzquierdo = -1.3f;
        seguimiento.limiteDerecho = 89.3f;

        // ---------- Fondo dinámico (parallax) ----------
        CapaFondo(fondo, "Cielo", "fondo_cielo", -20, 1f, 0f);
        CapaFondo(fondo, "Nubes", "fondo_nubes", -19, 0.92f, -0.3f);
        CapaFondo(fondo, "Montanas y templo", "fondo_montanas", -18, 0.85f, 0f);
        CapaFondo(fondo, "Selva", "fondo_selva", -17, 0.55f, 0f);

        // ---------- Escenario: suelo con fosos ----------
        Transform suelos = SubCarpeta(escenario, "Suelos");
        Bloque(suelos, "Suelo_1", "suelo", -13f, 16f, SUELO_Y, 2f);
        Bloque(suelos, "Suelo_2", "suelo", 19f, 40f, SUELO_Y, 2f);
        Bloque(suelos, "Suelo_3", "suelo", 48f, 72f, SUELO_Y, 2f);
        Bloque(suelos, "Suelo_4", "suelo", 75.5f, 101f, SUELO_Y, 2f);

        // Obstáculos de piedra para saltar (también sirven de suelo)
        Transform obstaculos = SubCarpeta(escenario, "Obstaculos");
        Bloque(obstaculos, "Muro_Inicio", "piedra", -13f, -12f, 6f, 10f);
        Bloque(obstaculos, "Piedra_1", "piedra", 6f, 7f, SUELO_Y + 1f, 1f);
        Bloque(obstaculos, "Piedra_2", "piedra", 26f, 27f, SUELO_Y + 2f, 2f);
        Bloque(obstaculos, "Muro_Final", "piedra", 100f, 101f, 6f, 10f);

        // Repisa de madera: se puede saltar desde abajo y quedar encima (one-way)
        GameObject repisa = Bloque(obstaculos, "Repisa", "plataforma", 31f, 35f, -1.75f, 0.5f);
        repisa.GetComponent<BoxCollider2D>().usedByEffector = true;
        PlatformEffector2D efector = repisa.AddComponent<PlatformEffector2D>();
        efector.useOneWay = true;
        efector.surfaceArc = 170f;

        // Zona mortal bajo los fosos
        GameObject zona = new GameObject("ZonaMortal");
        zona.transform.SetParent(escenario);
        zona.transform.position = new Vector3(45f, -8.5f, 0f);
        BoxCollider2D zonaCol = zona.AddComponent<BoxCollider2D>();
        zonaCol.isTrigger = true;
        zonaCol.size = new Vector2(160f, 2f);
        zona.AddComponent<ZonaMortal>();

        // ---------- Objetos con rutina propia ----------
        Transform lianas = SubCarpeta(objetos, "Lianas");
        foreach (float x in new float[] { 17.5f, 44f, 73.7f, 90f })
        {
            GameObject liana = Sprite2D("Liana", lianas, "liana", new Vector3(x, 6.3f, 0f), -5);
            LianaPendulo lp = liana.AddComponent<LianaPendulo>();
            lp.amplitud = 12f;
            lp.velocidad = 1.3f + (x % 3f) * 0.1f;
        }

        GameObject plataforma = Bloque(objetos, "PlataformaMovil", "plataforma", 42.5f, 45.5f, SUELO_Y, 0.5f);
        Rigidbody2D rbPlat = plataforma.AddComponent<Rigidbody2D>();
        rbPlat.bodyType = RigidbodyType2D.Kinematic;
        PlataformaMovil pm = plataforma.AddComponent<PlataformaMovil>();
        pm.direccion = Vector2.right;
        pm.amplitud = 2.5f;
        pm.velocidad = 1.2f;

        Transform tesoros = SubCarpeta(objetos, "Tesoros");
        Vector2[] monedas =
        {
            new Vector2(10f, -2.6f), new Vector2(17.5f, -1.6f), new Vector2(32f, -1.1f),
            new Vector2(34f, -1.1f), new Vector2(44f, -2.2f), new Vector2(58f, -3.3f),
            new Vector2(73.7f, -1.8f), new Vector2(86f, -2.6f)
        };
        int n = 1;
        foreach (Vector2 p in monedas)
        {
            GameObject m = Sprite2D("Moneda_" + n++, tesoros, "moneda", p, 5);
            CircleCollider2D c = m.AddComponent<CircleCollider2D>();
            c.isTrigger = true;
            c.radius = 0.35f;
            Tesoro t = m.AddComponent<Tesoro>();
            t.amplitud = 0.15f;
            t.velocidad = 3f;
        }

        GameObject idolo = Sprite2D("Idolo_Dorado", tesoros, "idolo", new Vector3(94f, -3.2f, 0f), 5);
        BoxCollider2D colIdolo = idolo.AddComponent<BoxCollider2D>();
        colIdolo.isTrigger = true;
        colIdolo.size = new Vector2(0.9f, 1.1f);
        Tesoro ti = idolo.AddComponent<Tesoro>();
        ti.esIdolo = true;
        ti.girar = false;
        ti.amplitud = 0.15f;
        ti.velocidad = 2f;

        // ---------- Enemigos (cada uno con su rutina en loop) ----------
        Sprite[] escorpion = { CargarSprite("escorpion_1"), CargarSprite("escorpion_2") };
        Sprite[] murcielago = { CargarSprite("murcielago_1"), CargarSprite("murcielago_2") };

        EnemigoPatrulla e1 = CrearEnemigo<EnemigoPatrulla>(enemigos, "Escorpion_1", escorpion, new Vector3(29.5f, SUELO_Y + 0.375f, 0f), new Vector2(0.85f, 0.5f));
        e1.velocidad = 2f;
        e1.distancia = 8f;

        EnemigoPatrulla e2 = CrearEnemigo<EnemigoPatrulla>(enemigos, "Escorpion_2", escorpion, new Vector3(63f, SUELO_Y + 0.375f, 0f), new Vector2(0.85f, 0.5f));
        e2.velocidad = 2.6f;
        e2.distancia = 7f;

        EnemigoVolador e3 = CrearEnemigo<EnemigoVolador>(enemigos, "Murcielago", murcielago, new Vector3(51f, -1.1f, 0f), new Vector2(0.8f, 0.4f));
        e3.velocidad = 2f;
        e3.distancia = 9f;
        e3.amplitud = 0.9f;
        e3.frecuencia = 2.5f;
        e3.GetComponent<AnimacionSprite>().cuadrosPorSegundo = 8f;

        EnemigoRodante e4 = CrearEnemigo<EnemigoRodante>(enemigos, "Tronco", new Sprite[] { CargarSprite("tronco") }, new Vector3(98f, SUELO_Y + 0.5f, 0f), Vector2.zero, 0.42f);
        e4.velocidad = 4f;
        e4.distancia = 21f;
        e4.radio = 0.5f;

        // ---------- Jugador ----------
        GameObject jugadorGO = Sprite2D("Explorador", jugadorCarpeta, "explorador_quieto", new Vector3(-6f, -3.2f, 0f), 10);
        jugadorGO.tag = "Player";
        Rigidbody2D rbJ = jugadorGO.AddComponent<Rigidbody2D>();
        rbJ.gravityScale = 3f;
        rbJ.freezeRotation = true;
        rbJ.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        BoxCollider2D colJ = jugadorGO.AddComponent<BoxCollider2D>();
        colJ.size = new Vector2(0.6f, 1.45f);
        colJ.offset = new Vector2(0f, -0.025f);
        colJ.sharedMaterial = sinFriccion;
        PlayerController jugador = jugadorGO.AddComponent<PlayerController>(); // RequireComponent agrega Movement, Jump, Flip...
        jugadorGO.GetComponent<Jump>().jumpForce = 12f;
        AnimadorExplorador anim = jugadorGO.GetComponent<AnimadorExplorador>();
        anim.quieto = CargarSprite("explorador_quieto");
        anim.paso1 = CargarSprite("explorador_paso1");
        anim.paso2 = CargarSprite("explorador_paso2");
        anim.salto = CargarSprite("explorador_salto");
        seguimiento.objetivo = jugadorGO.transform;

        // ---------- Gestores ----------
        GameObject audioGO = new GameObject("AudioManager");
        audioGO.transform.SetParent(gestores);
        AudioManager audio = audioGO.AddComponent<AudioManager>();
        audio.musicaFondo = CargarAudio("musica_selva");
        audio.sonidoSalto = CargarAudio("salto");
        audio.sonidoMuerte = CargarAudio("muerte");
        audio.sonidoTesoro = CargarAudio("tesoro");
        audio.sonidoVictoria = CargarAudio("victoria");

        GameObject gmGO = new GameObject("GameManager");
        gmGO.transform.SetParent(gestores);
        GameManager gm = gmGO.AddComponent<GameManager>();
        gm.jugador = jugador;

        // ---------- Interfaz ----------
        CrearInterfaz(ui, gm);

        EditorSceneManager.SaveScene(escena, RUTA_ESCENA);
        AgregarABuild();
        Debug.Log("Templo Perdido: escena construida en " + RUTA_ESCENA + ". Presiona Play y luego ENTER.");
    }

    // =====================================================================
    // Ayudantes
    // =====================================================================

    static void ConfigurarSprites()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { RUTA_SPRITES.TrimEnd('/') });
        foreach (string guid in guids)
        {
            string ruta = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter ti = (TextureImporter)AssetImporter.GetAtPath(ruta);
            if (ti == null) continue;

            ti.textureType = TextureImporterType.Sprite;
            ti.spriteImportMode = SpriteImportMode.Single;

            TextureImporterSettings ajustes = new TextureImporterSettings();
            ti.ReadTextureSettings(ajustes);
            ajustes.spriteMeshType = SpriteMeshType.FullRect;      // Necesario para el modo Tiled
            ajustes.spriteAlignment = ruta.EndsWith("liana.png")
                ? (int)SpriteAlignment.TopCenter                   // La liana cuelga desde arriba
                : (int)SpriteAlignment.Center;
            ti.SetTextureSettings(ajustes);

            ti.spritePixelsPerUnit = PPU;
            ti.filterMode = FilterMode.Point;                      // Pixel art nítido
            ti.textureCompression = TextureImporterCompression.Uncompressed;
            ti.mipmapEnabled = false;
            ti.alphaIsTransparency = true;
            ti.wrapMode = TextureWrapMode.Clamp;
            ti.SaveAndReimport();
        }
    }

    static void AsegurarTag(string tag)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tags = tagManager.FindProperty("tags");
        for (int i = 0; i < tags.arraySize; i++)
        {
            if (tags.GetArrayElementAtIndex(i).stringValue == tag) return;
        }
        tags.InsertArrayElementAtIndex(tags.arraySize);
        tags.GetArrayElementAtIndex(tags.arraySize - 1).stringValue = tag;
        tagManager.ApplyModifiedProperties();
    }

    static PhysicsMaterial2D CrearMaterialSinFriccion()
    {
        PhysicsMaterial2D mat = AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>(RUTA_MATERIAL);
        if (mat != null) return mat;

        if (!AssetDatabase.IsValidFolder("Assets/Fisica")) AssetDatabase.CreateFolder("Assets", "Fisica");
        mat = new PhysicsMaterial2D("SinFriccion");
        mat.friction = 0f;   // Evita que el jugador se quede pegado a las paredes
        mat.bounciness = 0f;
        AssetDatabase.CreateAsset(mat, RUTA_MATERIAL);
        return mat;
    }

    static void AgregarABuild()
    {
        List<EditorBuildSettingsScene> escenas = new List<EditorBuildSettingsScene>();
        escenas.Add(new EditorBuildSettingsScene(RUTA_ESCENA, true));
        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e.path != RUTA_ESCENA) escenas.Add(e);
        }
        EditorBuildSettings.scenes = escenas.ToArray();
    }

    static Sprite CargarSprite(string nombre)
    {
        Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>(RUTA_SPRITES + nombre + ".png");
        if (s == null) Debug.LogError("No se encontró el sprite " + nombre);
        return s;
    }

    static AudioClip CargarAudio(string nombre)
    {
        AudioClip a = AssetDatabase.LoadAssetAtPath<AudioClip>(RUTA_AUDIO + nombre + ".wav");
        if (a == null) Debug.LogError("No se encontró el audio " + nombre);
        return a;
    }

    static Transform Carpeta(string nombre)
    {
        return new GameObject(nombre).transform;
    }

    static Transform SubCarpeta(Transform padre, string nombre)
    {
        Transform t = new GameObject(nombre).transform;
        t.SetParent(padre);
        return t;
    }

    static GameObject Sprite2D(string nombre, Transform padre, string sprite, Vector3 posicion, int orden)
    {
        GameObject go = new GameObject(nombre);
        go.transform.SetParent(padre);
        go.transform.position = posicion;
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = CargarSprite(sprite);
        sr.sortingOrder = orden;
        return go;
    }

    // Bloque sólido repetido (Tiled) desde x0 a x1, con su borde superior en yArriba
    static GameObject Bloque(Transform padre, string nombre, string sprite, float x0, float x1, float yArriba, float alto)
    {
        float ancho = x1 - x0;
        GameObject go = Sprite2D(nombre, padre, sprite, new Vector3(x0 + ancho / 2f, yArriba - alto / 2f, 0f), 0);
        go.tag = "Suelo_01";
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        sr.drawMode = SpriteDrawMode.Tiled;
        sr.tileMode = SpriteTileMode.Continuous;
        sr.size = new Vector2(ancho, alto);
        BoxCollider2D col = go.AddComponent<BoxCollider2D>();
        col.size = new Vector2(ancho, alto);
        return go;
    }

    static void CapaFondo(Transform padre, string nombre, string sprite, int orden, float factor, float velocidadAuto)
    {
        GameObject go = Sprite2D(nombre, padre, sprite, new Vector3(-1.3f, 0f, 0f), orden);
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        sr.drawMode = SpriteDrawMode.Tiled;
        sr.tileMode = SpriteTileMode.Continuous;
        sr.size = new Vector2(sr.sprite.bounds.size.x * 3f, sr.sprite.bounds.size.y); // 3 copias lado a lado
        FondoParallax p = go.AddComponent<FondoParallax>();
        p.factorParallax = factor;
        p.velocidadAutomatica = velocidadAuto;
    }

    // radioCircular > 0 usa un CircleCollider2D (tronco); si no, un BoxCollider2D
    static T CrearEnemigo<T>(Transform padre, string nombre, Sprite[] cuadros, Vector3 posicion, Vector2 tamanoColision, float radioCircular = 0f) where T : Enemigo
    {
        GameObject go = new GameObject(nombre);
        go.transform.SetParent(padre);
        go.transform.position = posicion;
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = cuadros[0];
        sr.sortingOrder = 8;
        Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        if (radioCircular > 0f)
        {
            CircleCollider2D circulo = go.AddComponent<CircleCollider2D>();
            circulo.isTrigger = true;
            circulo.radius = radioCircular;
        }
        else
        {
            BoxCollider2D col = go.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = tamanoColision;
        }
        if (cuadros.Length > 1) // Solo se anima si tiene más de un cuadro
        {
            AnimacionSprite anim = go.AddComponent<AnimacionSprite>();
            anim.cuadros = cuadros;
        }
        return go.AddComponent<T>();
    }

    // ---------------- UI ----------------

    static void CrearInterfaz(Transform padre, GameManager gm)
    {
        GameObject canvasGO = new GameObject("Canvas");
        canvasGO.transform.SetParent(padre);
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler escalador = canvasGO.AddComponent<CanvasScaler>();
        escalador.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        escalador.referenceResolution = new Vector2(1920f, 1080f);
        escalador.matchWidthOrHeight = 0.5f;
        canvasGO.AddComponent<GraphicRaycaster>();

        // HUD
        Text hud = Texto(canvasGO.transform, "TextoTesoros", "Tesoros: 0 / 0", 48, new Color(1f, 0.85f, 0.3f), new Vector2(0f, 1f), new Vector2(40f, -40f), TextAnchor.UpperLeft);
        gm.textoTesoros = hud;
        Texto(canvasGO.transform, "TextoControles", "A/D o flechas: mover   Shift: correr   Espacio: saltar", 30, Color.white, new Vector2(1f, 1f), new Vector2(-40f, -45f), TextAnchor.UpperRight);

        // Paneles
        GameObject inicio = Panel(canvasGO.transform, "PanelInicio", new Color(0.05f, 0.1f, 0.06f, 0.82f));
        Texto(inicio.transform, "Titulo", "TEMPLO PERDIDO", 130, new Color(1f, 0.82f, 0.25f), Vector2.one * 0.5f, new Vector2(0f, 220f), TextAnchor.MiddleCenter);
        Texto(inicio.transform, "Subtitulo", "Cruza la selva y recupera el ídolo dorado", 50, Color.white, Vector2.one * 0.5f, new Vector2(0f, 80f), TextAnchor.MiddleCenter);
        Texto(inicio.transform, "Controles", "A/D o flechas: mover  ·  Shift: correr  ·  Espacio: saltar\nEvita escorpiones, murciélagos, troncos y fosos", 36, new Color(0.85f, 0.95f, 0.85f), Vector2.one * 0.5f, new Vector2(0f, -60f), TextAnchor.MiddleCenter);
        Texto(inicio.transform, "Comenzar", "Presiona ENTER para comenzar", 56, new Color(1f, 0.82f, 0.25f), Vector2.one * 0.5f, new Vector2(0f, -230f), TextAnchor.MiddleCenter);
        gm.panelInicio = inicio;

        GameObject fin = Panel(canvasGO.transform, "PanelFinDelJuego", new Color(0.25f, 0.02f, 0.02f, 0.8f));
        Texto(fin.transform, "Titulo", "GAME OVER", 150, new Color(1f, 0.35f, 0.3f), Vector2.one * 0.5f, new Vector2(0f, 80f), TextAnchor.MiddleCenter);
        Texto(fin.transform, "Reintentar", "Presiona R para reintentar", 56, Color.white, Vector2.one * 0.5f, new Vector2(0f, -100f), TextAnchor.MiddleCenter);
        gm.panelFinDelJuego = fin;

        GameObject victoria = Panel(canvasGO.transform, "PanelVictoria", new Color(0.12f, 0.1f, 0.0f, 0.8f));
        Texto(victoria.transform, "Titulo", "¡RECUPERASTE EL ÍDOLO!", 110, new Color(1f, 0.85f, 0.25f), Vector2.one * 0.5f, new Vector2(0f, 80f), TextAnchor.MiddleCenter);
        Texto(victoria.transform, "Reintentar", "Presiona R para jugar de nuevo", 56, Color.white, Vector2.one * 0.5f, new Vector2(0f, -100f), TextAnchor.MiddleCenter);
        gm.panelVictoria = victoria;

        fin.SetActive(false);
        victoria.SetActive(false);
    }

    static GameObject Panel(Transform padre, string nombre, Color color)
    {
        GameObject go = new GameObject(nombre);
        go.transform.SetParent(padre, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        Image img = go.AddComponent<Image>();
        img.color = color;
        return go;
    }

    static Text Texto(Transform padre, string nombre, string contenido, int tamano, Color color, Vector2 ancla, Vector2 posicion, TextAnchor alineacion)
    {
        GameObject go = new GameObject(nombre);
        go.transform.SetParent(padre, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = ancla;
        rt.anchorMax = ancla;
        rt.pivot = ancla;
        rt.anchoredPosition = posicion;
        rt.sizeDelta = new Vector2(1700f, tamano * 2.6f);
        Text t = go.AddComponent<Text>();
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.text = contenido;
        t.fontSize = tamano;
        t.color = color;
        t.alignment = alineacion;
        t.horizontalOverflow = HorizontalWrapMode.Overflow;
        t.verticalOverflow = VerticalWrapMode.Overflow;
        Shadow sombra = go.AddComponent<Shadow>();
        sombra.effectColor = new Color(0f, 0f, 0f, 0.8f);
        sombra.effectDistance = new Vector2(3f, -3f);
        return t;
    }
}
