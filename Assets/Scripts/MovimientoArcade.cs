using UnityEngine;

public class MovimientoArcade : MonoBehaviour
{
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public Transform frontLeftTransform;
    public Transform frontRightTransform;
    public Transform rearLeftTransform;
    public Transform rearRightTransform;

    public float maxMotorTorque = 1500f; // Torque del motor
    public float maxSteeringAngle = 30f; // Ángulo máximo de dirección

    private void Update()
    {
        // Llamamos a la función para actualizar la visualización de las ruedas
        UpdateWheelMeshes();
    }

    private void FixedUpdate()
    {
        // Obtén los inputs del usuario
        float motorInput = Input.GetAxis("Vertical");
        float steeringInput = Input.GetAxis("Horizontal");

        // Asegúrate de que el input sea correcto
        Debug.Log("Motor Input: " + motorInput);
        Debug.Log("Steering Input: " + steeringInput);

        // Aplicar el torque al motor y la dirección
        float motorTorque = maxMotorTorque * motorInput;
        frontLeftWheel.motorTorque = motorTorque;
        frontRightWheel.motorTorque = motorTorque;

        // Aplicar el ángulo de dirección
        float steering = maxSteeringAngle * steeringInput;
        frontLeftWheel.steerAngle = steering;
        frontRightWheel.steerAngle = steering;
    }

    private void UpdateWheelMeshes()
    {
        // Actualizar la posición y rotación de las ruedas visuales con respecto a los Wheel Colliders
        UpdateWheelPosition(frontLeftWheel, frontLeftTransform);
        UpdateWheelPosition(frontRightWheel, frontRightTransform);
        UpdateWheelPosition(rearLeftWheel, rearLeftTransform);
        UpdateWheelPosition(rearRightWheel, rearRightTransform);
    }

    private void UpdateWheelPosition(WheelCollider collider, Transform transform)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        transform.position = position;
        transform.rotation = rotation;
    }
}