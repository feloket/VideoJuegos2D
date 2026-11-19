/*
Autor:psotoc
Fecha:25/10/2024
Licencia:Open estudiantil
Funcionalidad: mueve el objeto de iz. a dr. y con la espaciadora salta.
Modificación: Detecta que tecla se ha presionado.
Marcas POO: busca con Ctrl+F "[HERENCIA]", "[OBJETO]", "[OVERRIDE]", etc.
              Marcas en este archivo: [HERENCIA], [ENUM].
*/
using UnityEngine;

// [NOTA] script de depuración del profesor: no está pegado a ningún objeto de la escena
// [HERENCIA] hereda de MonoBehaviour
public class DetectarTecla : MonoBehaviour
{
    void Update()
    {
        // Detecta si se presiona cualquier tecla
        if (Input.anyKeyDown)
        {
            // Recorre todos los KeyCodes posibles
            // [ENUM] KeyCode es un enum de Unity con todas las teclas
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    Debug.Log("Tecla presionada: " + keyCode);
                }
            }
        }
    }
}
