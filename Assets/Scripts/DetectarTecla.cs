/*
Autor:psotoc
Fecha:25/10/2024
Licencia:Open estudiantil
Funcionalidad: mueve el objeto de iz. a dr. y con la espaciadora salta.
Modificación: Detecta que tecla se ha presionado.
*/
using UnityEngine;

public class DetectarTecla : MonoBehaviour
{
    void Update()
    {
        // Detecta si se presiona cualquier tecla
        if (Input.anyKeyDown)
        {
            // Recorre todos los KeyCodes posibles
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
