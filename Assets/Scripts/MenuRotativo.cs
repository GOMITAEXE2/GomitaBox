using UnityEngine;

public class MenuRotativo : MonoBehaviour
{
    public GameObject[] characters;  // Array de personajes
    public float radius = 5f;        // Radio del círculo donde estarán los personajes
    public KeyCode rotateLeftKey = KeyCode.LeftArrow;  // Tecla para rotar a la izquierda
    public KeyCode rotateRightKey = KeyCode.RightArrow; // Tecla para rotar a la derecha
    public float smoothTime = 0.5f;  // Tiempo que tarda en completar la rotación
    public Camera mainCamera;        // Cámara para verificar el foco
    public float focusedRotationSpeed = 30f; // Velocidad de rotación del objeto enfocado

    private int currentCharacterIndex = 0;  // Índice del personaje seleccionado
    private float angleStep;  // Ángulo entre cada personaje
    private float targetRotation = 0f;  // Ángulo objetivo hacia el que se rota
    private float currentRotation = 0f; // Rotación actual del objeto
    private float rotationVelocity = 0f; // Velocidad utilizada para el suavizado

    void Start()
    {
        angleStep = 360f / characters.Length; // Calcular el ángulo de separación entre personajes
        PositionCharactersInCircle();
    }

    void Update()
    {
        // Comprobar si se presionan las teclas para rotar
        if (Input.GetKeyDown(rotateLeftKey))
        {
            RotateLeft();
        }
        if (Input.GetKeyDown(rotateRightKey))
        {
            RotateRight();
        }

        // Suavizar la rotación del menú hacia el ángulo objetivo
        currentRotation = Mathf.SmoothDampAngle(
            currentRotation,
            targetRotation,
            ref rotationVelocity,
            smoothTime
        );

        transform.eulerAngles = new Vector3(0, currentRotation, 0); // Aplicar la rotación del menú

        // Verificar si algún personaje está en el foco de la cámara y hacerlo rotar
        CheckFocusAndRotate();
    }

    void PositionCharactersInCircle()
    {
        int characterCount = characters.Length;

        for (int i = 0; i < characterCount; i++)
        {
            // Posicionar los personajes en el círculo
            float angle = i * angleStep;
            Vector3 position = new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
                0,
                Mathf.Sin(Mathf.Deg2Rad * angle) * radius
            );
            characters[i].transform.localPosition = position;
            characters[i].transform.LookAt(transform.position);  
        }
    }

    void RotateLeft()
    {
        currentCharacterIndex--; 
        if (currentCharacterIndex < 0)
        {
            currentCharacterIndex = characters.Length - 1; 
        }

        UpdateTargetRotation();
    }

    void RotateRight()
    {
        currentCharacterIndex++; 
        if (currentCharacterIndex >= characters.Length)
        {
            currentCharacterIndex = 0; 
        }

        UpdateTargetRotation();
    }

    void UpdateTargetRotation()
    {
        targetRotation = currentCharacterIndex * angleStep; 
    }

    void CheckFocusAndRotate()
    {
        foreach (GameObject character in characters)
        {
            if (IsObjectInFocus(character))
            {
                character.transform.Rotate(Vector3.up, focusedRotationSpeed * Time.deltaTime);
            }
        }
    }

    bool IsObjectInFocus(GameObject obj)
    {
        // Obtener las coordenadas del objeto en el espacio de la cámara
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(obj.transform.position);

        // Verificar si está dentro de los límites del viewport
        bool isInViewport = viewportPos.x >= 0 && viewportPos.x <= 1 &&
                            viewportPos.y >= 0 && viewportPos.y <= 1 &&
                            viewportPos.z > 0;

        return isInViewport;
    }
}
