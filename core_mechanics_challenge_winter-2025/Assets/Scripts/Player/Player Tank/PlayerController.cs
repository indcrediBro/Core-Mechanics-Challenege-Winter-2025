using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody Rigidbody;
    public Transform TankBase;
    public Transform Cannon;
    public PlayerInputHandler Input;
    public PlayerStatistics Stats;
    public WeaponController WeaponRig;
    public string BulletKey;

    [Header("Modules")]
    [SerializeField] private List<PlayerModule> modules;

    private PlayerContext context;

    private void Awake()
    {
        context = new PlayerContext
        {
            Controller = this,
            Stats = Stats,
            Input = Input,
            Rigidbody = Rigidbody,
            TankBase = TankBase,
            Cannon = Cannon
        };

        foreach (var m in modules)
            m.Initialize(context);
    }

    private void OnEnable() => Input.Enable();
    private void OnDisable() => Input.Disable();

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

        foreach (var m in modules)
            m.Tick(context);
    }

    private void FixedUpdate()
    {
        foreach (var m in modules)
            m.FixedTick(context);
    }
}