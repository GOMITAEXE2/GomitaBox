using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoDeAutito : MonoBehaviour
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
        //MOVIMIENTO
        FuerzaMovimiento += transform.forward * VelocidadMovimiento * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.position += FuerzaMovimiento * Time.deltaTime;

        //Direccion
        float steerInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerInput * FuerzaMovimiento.magnitude * AnguloDireccion * Time.deltaTime);

        //DRAG
        FuerzaMovimiento *= Drag;
        FuerzaMovimiento = Vector3.ClampMagnitude(FuerzaMovimiento, VelocidadMaxima);

        //TRACCION
        Debug.DrawRay(transform.position, FuerzaMovimiento * 3);
        Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);
        FuerzaMovimiento = Vector3.Lerp(FuerzaMovimiento.normalized, transform.forward, Traccion * Time.deltaTime) * FuerzaMovimiento.magnitude;
    }
}
