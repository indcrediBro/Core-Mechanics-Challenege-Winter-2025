using UnityEngine;

[CreateAssetMenu(menuName = "Player/Stats")]
public class PlayerStatistics : ScriptableObject
{
    public float MoveSpeed = 2f;
    public float FireRate = 0.5f;
    public int Damage = 1;

    public float BulletSize = .2f;
    public int BulletPierce = 1;

    public int FrontCannonLevel = 1;
    public bool RearCannonEnabled = false;
    public int FirePointLevel = 1;
}