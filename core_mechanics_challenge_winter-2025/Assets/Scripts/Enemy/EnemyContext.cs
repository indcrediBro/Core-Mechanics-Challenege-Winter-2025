using UnityEngine;
using UnityEngine.AI;

public class EnemyContext
{
    public Transform self;
    public Transform player;
    public Transform playerBase;
    public EnemyHealth health;
    public NavMeshAgent agent;
    public Transform cannon;
    public Transform firePoint;
    public Transform target;
    public SpriteRenderer spriteRenderer;

    public float deltaTime;
    public EnemyController controller;

    // Animation
    public SimpleAnimState animState = new SimpleAnimState();
    public float moveTimer;
    public Vector3 moveDir;
}
