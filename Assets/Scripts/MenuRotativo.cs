using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuRotativo : MonoBehaviour
{
    public GameObject[] characters;  // Array de personajes
    public float radius = 5f;        // Radio del círculo donde estarán los personajes
    public float rotationSpeed = 30f; // Velocidad de rotación controlada por el usuario
    public KeyCode rotateLeftKey = KeyCode.LeftArrow;  // Tecla para rotar a la izquierda
    public KeyCode rotateRightKey = KeyCode.RightArrow; // Tecla para rotar a la derecha

    private int currentCharacterIndex = 0;  // Índice del personaje seleccionado
    private float angleStep;  // Ángulo entre cada personaje

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
    }

    void PositionCharactersInCircle()
    {
        int characterCount = characters.Length;

        for (int i = 0; i < characterCount; i++)
        {
            // Posicionar los personajes en el círculo
            float angle = i * angleStep;
            Vector3 position = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * radius, 0, Mathf.Sin(Mathf.Deg2Rad * angle) * radius);
            characters[i].transform.localPosition = position;
            characters[i].transform.LookAt(transform.position);  // Asegura que el personaje mire hacia el centro
        }
    }

    void RotateLeft()
    {
        currentCharacterIndex--; // Mover al personaje anterior
        if (currentCharacterIndex < 0)
        {
            currentCharacterIndex = characters.Length - 1; // Volver al último personaje si estamos al principio
        }
        RotateMenu();
    }

    void RotateRight()
    {
        currentCharacterIndex++; // Mover al siguiente personaje
        if (currentCharacterIndex >= characters.Length)
        {
            currentCharacterIndex = 0; // Volver al primer personaje si estamos al final
        }
        RotateMenu();
    }

    void RotateMenu()
    {
        // Calcular el ángulo de rotación necesario para alinear al personaje seleccionado
        float targetAngle = currentCharacterIndex * angleStep;
        float currentAngle = transform.eulerAngles.y;

        // Mover suavemente hacia el ángulo objetivo
        float angle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        // Aplicar la rotación
        transform.eulerAngles = new Vector3(0, angle, 0);
    }
}