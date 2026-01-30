using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float m_lifeTime;
    [SerializeField] private Rigidbody2D m_rb;
    
    private void OnEnable()
    {
        Launch();
        Destroy(gameObject, m_lifeTime);
    }


    private void Launch()
    {
        m_rb.AddForce(transform.up  * m_moveSpeed, ForceMode2D.Impulse);
    }
}
