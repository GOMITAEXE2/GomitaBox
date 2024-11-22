using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using UnityEngine;

public class AmortiguadorArcade : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Esto referencia el Rigidboody del auto")]
    [SerializeField] private Rigidbody CarRb;
    [Tooltip("Esto referencia los puntos de amortiguacion que tendra el auto utilizando un transform")]
    [SerializeField] private Transform[] rayPoints;
    [Tooltip("Layer donde el auto va a poder manejarse")]
    [SerializeField] private LayerMask Manejable;
    [Tooltip("Punto tipo transform que se utiliza para la acelereacion del auto")]
    [SerializeField] private Transform PuntoDeAceleracion;

    [Header("Suspencion Settings")]
    [SerializeField] private float ResorteRigidez;
    [SerializeField] private float PistonRigidez;
    [SerializeField] private float DuracionDescanso;
    [SerializeField] private float ViajeResorte;
    [SerializeField] private float RadioDeRueda;

    private int[] RuedaSobreSuelo;
    private bool SobreElSuelo = false;

    [Header("Input")]
    [SerializeField] private float moveInput = 0;
    [SerializeField] private float streetInput = 0;

    [Header("Moviment Settings")]
    [SerializeField] private float Aceleracion;
    [SerializeField] private float VelocidadMaxima;
    [SerializeField] private float Desaceleracion;
    [SerializeField] private float FuerzaDireccion;
    [SerializeField] private AnimationCurve DoblarAnimation;
    [SerializeField] private float CoefficienteDeAgarre;

    private Vector3 VelocidadActualDelAuto = Vector3.zero;
    private float RatioVelocidadDelAtuo = 0;
   

    private void Start()
    {
        CarRb = GetComponent<Rigidbody>();
        RuedaSobreSuelo = new int[rayPoints.Length];
    }

    private void FixedUpdate()
    {
        Suspencion();
        ChequearSuelo();
        CalcularVelocidadAuto();
        MovimientoDeAuto();
    }

    private void Update()
    {
        GetPlayerInput();
    }
    private void GetPlayerInput()
    {
        moveInput = Input.GetAxis("Vertical");
        streetInput = Input.GetAxis("Horizontal");
    }

    private void ChequearSuelo()
    {
        int tempRuedasSobreSuelo = 0;

        for (int i = 0; i < RuedaSobreSuelo.Length; i++)
        {
            tempRuedasSobreSuelo += RuedaSobreSuelo[i];
        }
        if(tempRuedasSobreSuelo > 1)
        {
            SobreElSuelo = true;
        }
        else
        {
            SobreElSuelo = false;
        }

    }

    private void MovimientoDeAuto()
    {
        if (SobreElSuelo)
        {
            CalcularAceleracionAuto();
            CalcularDesaceleracionAuto();
            Doblar();
            ArrastreLateral();
        }
    }

    private void CalcularAceleracionAuto()
    {
        CarRb.AddForceAtPosition(Aceleracion * moveInput * transform.forward, PuntoDeAceleracion.position, ForceMode.Acceleration);
    }
    private void CalcularDesaceleracionAuto()
    {
        CarRb.AddForceAtPosition(Desaceleracion * moveInput * -transform.forward, PuntoDeAceleracion.position, ForceMode.Acceleration);
    }

    private void Doblar()
    {
        CarRb.AddTorque(FuerzaDireccion * streetInput * DoblarAnimation.Evaluate(RatioVelocidadDelAtuo) * Mathf.Sign(RatioVelocidadDelAtuo) * transform.up, ForceMode.Acceleration);
    }

    private void ArrastreLateral()
    {
        float velocidadLateralActual = VelocidadActualDelAuto.x;
        float magnitudDeArrastre = velocidadLateralActual * CoefficienteDeAgarre;
        Vector3 FuerzaDeArrastre = transform.right * magnitudDeArrastre;
        CarRb.AddForceAtPosition(FuerzaDeArrastre, CarRb.worldCenterOfMass, ForceMode.Acceleration);
    }



    private void CalcularVelocidadAuto()
    {
        VelocidadActualDelAuto = transform.InverseTransformDirection(CarRb.velocity);
        RatioVelocidadDelAtuo = VelocidadActualDelAuto.z / VelocidadMaxima;
    }
    
    private void Suspencion()
    {
        for (int i = 0; i < rayPoints.Length; i++)
        {
            RaycastHit hit;
            float maximoDescanso = DuracionDescanso * ViajeResorte;

            if (Physics.Raycast(rayPoints[i].position, -rayPoints[i].up, out hit, maximoDescanso + RadioDeRueda, Manejable))
            {
                RuedaSobreSuelo[i] = 1;

                float resorteLongitudActual = hit.distance - RadioDeRueda;
                float comprecionResorte = (DuracionDescanso - resorteLongitudActual) / ViajeResorte;

                float velocidadResorte = Vector3.Dot(CarRb.GetPointVelocity(rayPoints[i].position), rayPoints[i].up);
                float fuerzaAmortiguacion = ResorteRigidez * velocidadResorte;

                float fuerzaResorte = ResorteRigidez * comprecionResorte;

                float fuerzaRed = fuerzaResorte - fuerzaAmortiguacion;

                CarRb.AddForceAtPosition(fuerzaRed * rayPoints[i].up, rayPoints[i].position);

                Debug.DrawLine(rayPoints[i].position, hit.point, Color.blue);
            }
            else
            {
                RuedaSobreSuelo[i] = 0;

                Debug.DrawLine(rayPoints[i].position, rayPoints[i].position + (RadioDeRueda + maximoDescanso) * -rayPoints[i].up, Color.green);
            }
        }
    }
}
