// using System.Collections.Generic;
// using UnityEngine;
//
// public class SnakeBoss : BossBase
// {
//     [Header("Snake")]
//     [SerializeField] private List<SnakeSegment> segments;
//     [SerializeField] private float dashSpeed = 8f;
//     [SerializeField] private float dashDuration = 1f;
//     [SerializeField] private float dashCooldown = 2f;
//     [SerializeField] private float patrolReachDistance = 0.5f;
//     [SerializeField] private LayerMask damageableLayers;
//     [SerializeField] private float segmentSpacing = 0.4f;
//     [SerializeField] private float segmentDamageRadius = 0.5f;
//
//     private float dashTimer;
//     private float dashDurationTimer;
//     private bool isDashing;
//
//     private Vector3 patrolTarget;
//     private bool hasPatrolTarget;
//
//     private float lockedYPosition;
//     private readonly List<Vector3> positionHistory = new List<Vector3>();
//
//     protected override void OnEnable()
//     {
//         base.OnEnable();
//         dashTimer = 0f;
//         dashDurationTimer = 0f;
//         isDashing = false;
//         committedDir = Vector3.zero;
//         PickNewPatrolTarget();
//         positionHistory.Clear();
//         lockedYPosition = transform.position.y;
//     }
//
//     private void Update()
//     {
//         CleanupSegments();
//
//         if (segments.Count == 0)
//         {
//             health.TakeDamage(health.GetMaxHealthValue());
//             return;
//         }
//
//         dashTimer -= Time.deltaTime;
//
//         if (HasLineOfSight() && dashTimer <= 0f)
//         {
//             StartDash();
//         }
//
//         if (isDashing)
//         {
//             DashStep();
//             CheckSegmentDamage();
//         }
//         else
//         {
//             PatrolOrChase();
//         }
//
//         LockYPosition();
//         RecordHistory();
//         FollowAndRotateSegments();
//     }
//
//     // -------------------------------------------------
//     // BEHAVIOR
//     // -------------------------------------------------
//
//     private void PatrolOrChase()
//     {
//         if (HasLineOfSight())
//         {
//             MoveCardinalWithAvoidance(player.position);
//             return;
//         }
//
//         if (!hasPatrolTarget)
//             PickNewPatrolTarget();
//
//         MoveCardinalWithAvoidance(patrolTarget);
//
//         if (Vector3.Distance(transform.position, patrolTarget) < patrolReachDistance)
//         {
//             PickNewPatrolTarget();
//         }
//     }
//
//     private void StartDash()
//     {
//         isDashing = true;
//         dashTimer = dashCooldown;
//         dashDurationTimer = dashDuration;
//         committedDir = GetCardinalDirection(player.position - transform.position);
//     }
//
//     private void DashStep()
//     {
//         dashDurationTimer -= Time.deltaTime;
//
//         if (dashDurationTimer <= 0f)
//         {
//             StopDash();
//             return;
//         }
//
//         float step = dashSpeed * Time.deltaTime;
//         RaycastHit hit;
//
//         if (Physics.Raycast(transform.position, committedDir, out hit, step, damageableLayers))
//         {
//             if (hit.collider.TryGetComponent(out Health h))
//             {
//                 h.TakeDamage(1);
//             }
//
//             transform.position = hit.point - committedDir * 0.2f;
//             StopDash();
//             return;
//         }
//
//         transform.position += committedDir * step;
//
//         if (!CanMove(committedDir, 0.6f))
//         {
//             StopDash();
//         }
//     }
//
//     private void StopDash()
//     {
//         isDashing = false;
//         committedDir = Vector3.zero;
//         hasPatrolTarget = false;
//         dashDurationTimer = 0f;
//     }
//
//     // -------------------------------------------------
//     // POSITION
//     // -------------------------------------------------
//
//     private void LockYPosition()
//     {
//         Vector3 pos = transform.position;
//         pos.y = lockedYPosition;
//         transform.position = pos;
//     }
//
//     // -------------------------------------------------
//     // DAMAGE
//     // -------------------------------------------------
//
//     private void CheckSegmentDamage()
//     {
//         foreach (var segment in segments)
//         {
//             if (segment == null) continue;
//
//             Collider[] hits = Physics.OverlapSphere(
//                 segment.transform.position,
//                 segmentDamageRadius,
//                 damageableLayers
//             );
//
//             foreach (var hit in hits)
//             {
//                 if (hit.TryGetComponent(out Health h))
//                 {
//                     h.TakeDamage(1);
//                 }
//             }
//         }
//     }
//
//     // -------------------------------------------------
//     // PATROL
//     // -------------------------------------------------
//
//     private void PickNewPatrolTarget()
//     {
//         List<Vector3> spots = LevelManager.Instance.GetFreeSpots();
//         if (spots.Count == 0)
//             return;
//
//         patrolTarget = spots[Random.Range(0, spots.Count)];
//         hasPatrolTarget = true;
//     }
//
//     // -------------------------------------------------
//     // SEGMENTS
//     // -------------------------------------------------
//
//     private void RecordHistory()
//     {
//         if (positionHistory.Count == 0 ||
//             Vector3.Distance(positionHistory[^1], transform.position) > segmentSpacing)
//         {
//             positionHistory.Add(transform.position);
//         }
//
//         int max = segments.Count * 15;
//         if (positionHistory.Count > max)
//             positionHistory.RemoveAt(0);
//     }
//
//     private void FollowAndRotateSegments()
//     {
//         if (positionHistory.Count < 2)
//             return;
//
//         for (int i = 0; i < segments.Count; i++)
//         {
//             int index = Mathf.Clamp(positionHistory.Count - 1 - (i + 1) * 6, 0, positionHistory.Count - 1);
//
//             Vector3 prevPos = segments[i].transform.position;
//             Vector3 nextPos = positionHistory[index];
//             nextPos.y = lockedYPosition;
//
//             segments[i].transform.position = Vector3.Lerp(
//                 prevPos,
//                 nextPos,
//                 Time.deltaTime * 12f
//             );
//
//             // Only segments rotate, Y-axis only, X locked to 90
//             Vector3 moveDir = segments[i].transform.position - prevPos;
//             moveDir.y = 0f;
//
//             if (moveDir.sqrMagnitude > 0.0001f)
//             {
//                 // Calculate yaw ONLY
//                 float yaw = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
//
//                 // HARD LOCK rotation
//                 segments[i].transform.localRotation =
//                     Quaternion.AngleAxis(yaw, Vector3.up) *
//                     Quaternion.AngleAxis(90f, Vector3.right);
//             }
//
//         }
//     }
//
//     private void CleanupSegments()
//     {
//         segments.RemoveAll(s => s == null || s.Health.IsDead());
//     }
// }