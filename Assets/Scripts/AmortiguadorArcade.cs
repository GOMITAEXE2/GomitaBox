using UnityEngine;

public class AmortiguadorArcade : MonoBehaviour
{
    public Transform[] wheelTransforms;   // Transform para cada rueda
    public float springStrength = 1500f;  // Fuerza de resorte de la suspensión
    public float damping = 100f;          // Fuerza de amortiguación
    public float restLength = 0.3f;       // Altura de reposo de la suspensión
    public float maxCompressionDistance = 0.2f;  // Máxima compresión
    public float maxSuspensionForce = 3000f;     // Límite de fuerza

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    void FixedUpdate()
    {
        Vector3 totalSuspensionForce = Vector3.zero; // Acumula fuerza total

        foreach (Transform wheel in wheelTransforms)
        {
            totalSuspensionForce += ApplySuspensionForce(wheel);
        }

        // Aplica la fuerza total al chasis en el centro de masa
        rb.AddForce(totalSuspensionForce);
    }

    Vector3 ApplySuspensionForce(Transform wheel)
    {
        Ray ray = new Ray(wheel.position, -wheel.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, restLength + maxCompressionDistance))
        {
            float compressionRatio = Mathf.Clamp01((restLength - hit.distance) / maxCompressionDistance);
            float springForce = springStrength * compressionRatio;

            Vector3 relativeVelocity = rb.GetPointVelocity(wheel.position);
            float damperForce = damping * Vector3.Dot(relativeVelocity, wheel.up);

            Vector3 suspensionForce = Mathf.Clamp(springForce - damperForce, 0, maxSuspensionForce) * wheel.up;

            return suspensionForce; // Devuelve la fuerza calculada para la rueda
        }

        return Vector3.zero; // Si no hay colisión, no se aplica fuerza
    }
}
