using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class MovimientoDeAutito : MonoBehaviourPunCallbacks
{
    public float VelocidadMovimiento;
    public float VelocidadMaxima;
    public float Drag;
    public float AnguloDireccion;
    public float Traccion;

    private Vector3 FuerzaMovimiento;

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            //MOVIMIENTO

            FuerzaMovimiento += transform.forward * VelocidadMovimiento * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += FuerzaMovimiento * Time.deltaTime;

            //TRACCION
            FuerzaMovimiento = Vector3.Lerp(FuerzaMovimiento.normalized, transform.forward, Traccion * Time.deltaTime) * FuerzaMovimiento.magnitude;

            //DRAG
            FuerzaMovimiento *= Drag;
            FuerzaMovimiento = Vector3.ClampMagnitude(FuerzaMovimiento, VelocidadMaxima);

            //Direccion
            float steerInput = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up * steerInput * FuerzaMovimiento.magnitude * AnguloDireccion * Time.deltaTime);

            Debug.DrawRay(transform.position, FuerzaMovimiento * 3);
            Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);
        }
    }
}
