using UnityEngine;
using UnityEngine.UIElements;

public class CannonMovement : MonoBehaviour
{
    [SerializeField] private Transform m_target;
    [SerializeField] private float m_rotationSpeed;

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        //Todo: Rotate target on Z axis using mouse position
        Vector3 mouseWorldPos = GetMousePosition();
        mouseWorldPos.z = m_target.position.z; // keep rotation in 2D plane

        Vector3 direction = mouseWorldPos - transform.position;

        if (direction.sqrMagnitude < 0.001f)
            return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - 90);

        m_target.rotation = Quaternion.RotateTowards(
            m_target.rotation,
            targetRotation,
            m_rotationSpeed * Time.deltaTime
        );
    }

    private Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
