using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float MoveSpeed = 2f;
    public float FireRate = 0.5f;
    public float Damage = 1f;

    public float BulletSize = .2f;
    public int BulletPierce = 1;

    public int FrontCannonLevel = 1;   // 1–3
    public bool RearCannonEnabled = false;
    public int FirePointLevel = 1;      // 1–3

    public const float MAX_MOVE_SPEED = 5f;
    public const float MIN_FIRE_RATE = 0.1f;
    public const float MAX_DAMAGE = 10f;
    public const float MAX_BULLET_SIZE = .6f;
    public const int   MAX_PIERCE = 5;
}