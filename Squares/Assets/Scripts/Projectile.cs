using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public float speed;
    private Vector3 targetPosition;
    private Unit target;
    private float maxDistance;
    private Vector3 spawnPos;

    public void Initialize(Unit targetUnit, int dmg, float spd, float range)
    {
        target = targetUnit;
        damage = dmg;
        speed = spd;
        maxDistance = range;

        spawnPos = transform.position;

        // Lock target position at the time of firing
        if (targetUnit != null)
            targetPosition = targetUnit.transform.position;
        else
            targetPosition = spawnPos; // failsafe
    }

    void Update()
    {
        // Direction towards locked position
        Vector3 dir = (targetPosition - transform.position).normalized;

        // Move towards that point
        transform.position += dir * speed * Time.deltaTime;

        // Rotate to face locked position
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Check if hit the target (if still alive and close to locked point)
        if (target != null && Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }

        // Destroy if exceeded range
        if (Vector3.Distance(spawnPos, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
