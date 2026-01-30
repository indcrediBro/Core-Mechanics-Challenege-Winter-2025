using System;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform m_firePoint;
    [SerializeField] private GameObject m_bullet;


    
    public void Shoot()
    {
        Instantiate(m_bullet, m_firePoint.position, m_firePoint.rotation);
    }
}
