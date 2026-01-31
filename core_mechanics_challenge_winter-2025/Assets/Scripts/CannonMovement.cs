using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class CannonMovement : MonoBehaviour
{
    [SerializeField] private Transform    target;
    [SerializeField] private float        rotationSpeed;
    [SerializeField] private InputHandler inputHandler;

    private void Update()
    {
        // Rotate();
    }

    private void Rotate()
    {
        //Todo: Rotate target on Z axis using mouse position
        Vector3 mouseWorldPos = GetMousePosition();
        mouseWorldPos.z = target.position.z; // keep rotation in 2D plane

        Vector3 direction = mouseWorldPos - transform.position;

        direction = inputHandler.GetDirection();

        if (direction.sqrMagnitude < 0.001f)
            return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - 90);

        target.rotation = Quaternion.RotateTowards(
            target.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(inputHandler.GetMouse());
    }
}
