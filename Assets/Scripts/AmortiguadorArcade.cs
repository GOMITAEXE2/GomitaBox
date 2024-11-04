using System.Collections.Generic;
using UnityEngine;

public class AmortiguadorArcade : MonoBehaviour
{
    // Lista de objetos que simulan los puntos de suspensión
    [SerializeField]
    private List<Transform> suspensionPoints;

    // Fuerza de repulsión que cada punto aplicará hacia arriba en el objeto contenedor
    public float upwardForce = 10f;

    // Fuerza de repulsión que el objeto contenedor aplicará hacia abajo para balancear
    public float downwardForce = 10f;

    private Rigidbody rb; // Rigidbody del objeto contenedor

    void Start()
    {
        // Obtener el Rigidbody del objeto contenedor
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("El objeto contenedor necesita un Rigidbody.");
        }
    }

    void FixedUpdate()
    {
        // Aplicar fuerza hacia arriba en el objeto contenedor desde cada punto de suspensión
        foreach (Transform suspensionPoint in suspensionPoints)
        {
            if (rb != null)
            {
                // Crear una fuerza hacia arriba desde la posición de cada punto de suspensión
                Vector3 upwardRepulsion = Vector3.up * upwardForce;
                rb.AddForceAtPosition(upwardRepulsion, suspensionPoint.position, ForceMode.Force);
            }
            else
            {
                Debug.LogWarning("El objeto contenedor no tiene Rigidbody.");
            }
        }

        // Aplicar fuerza hacia abajo en el objeto contenedor para balancear
        if (rb != null)
        {
            Vector3 downwardRepulsion = Vector3.down * downwardForce;
            rb.AddForce(downwardRepulsion, ForceMode.Force);
        }
    }
}
