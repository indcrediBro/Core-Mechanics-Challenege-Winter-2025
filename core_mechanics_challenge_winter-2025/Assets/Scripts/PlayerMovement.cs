using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float        m_speed;
    private                  Vector2      m_input;
    [SerializeField] private InputHandler inputHandler;
    
    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        m_input = inputHandler.GetMovement();
        
        if (Mathf.Abs(m_input.x) > Mathf.Abs(m_input.y))
        {
            m_input.y = 0;
        }
        else
        {
            m_input.x = 0;
        }
        
        m_input.Normalize();
    }

    private void Move()
    {
        //Todo: Move the player in 2D using input (up,down,left,right, ** NOT diagonally according to Jam Rules)
        Vector2 velocity = m_input.normalized * m_speed * Time.deltaTime;
        transform.Translate(velocity, Space.World);
    }

    private void Rotate()
    {
        //TODO: rotate to the moving direction
        if (m_input.sqrMagnitude < 0.01f)
            return;

        float angle = Mathf.Atan2(m_input.y, m_input.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + 90);
        
        
        // Override Above Code
        transform.rotation = Quaternion.LookRotation(m_input, Vector3.forward );
    }
}
