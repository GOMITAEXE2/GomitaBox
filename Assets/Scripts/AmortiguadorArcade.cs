using System.Collections.Generic;
using UnityEngine;

public class AmortiguadorArcade : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Esto referencia el Rigidboody del auto")]
    [SerializeField] private Rigidbody CarRb;
    [Tooltip("Esto referencia los puntos de amortiguacion que tendra el auto utilizando un transform")]
    [SerializeField] private Transform[] PuntosDeAmortiguacion;
    [Tooltip("Layer donde el auto va a poder manejarse")]
    [SerializeField] private LayerMask Manejable;

    [Header("Suspencion Setings")]
    [SerializeField] private float ResorteRigidez;
    [SerializeField] private float DuracionDescanso;
    [SerializeField] private float ViajeResorte;
    [SerializeField] private float RadioDeRueda;

    private void Start()
    {
        CarRb = GetComponent<Rigidbody>();
    }

    private void Suspencion()
    {
        foreach (Transform rayPoint in PuntosDeAmortiguacion)
        {
            RaycastHit hit;
            float maximoDescanso = DuracionDescanso * ViajeResorte;
            if (Physics.Raycast(rayPoint.position, -rayPoint.up, out hit, maximoDescanso + RadioDeRueda, Manejable))
            {
                float resorteLongitudActual = hit.distance - RadioDeRueda;
                float comprecionResorte = (DuracionDescanso - resorteLongitudActual) / ViajeResorte;

                float fuerzaResorte = ResorteRigidez * comprecionResorte;

                CarRb.AddForceAtPosition(fuerzaResorte * rayPoint.up, rayPoint.position);

                Debug.DrawLine(rayPoint.position, hit.point, Color.blue);
            }
            else
            {

            }
        }
       

    }
}
