using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public float speed;
    private Unit target;

    public void Initialize(Unit targetUnit, int dmg, float spd)
    {
        target = targetUnit;
        damage = dmg;
        speed = spd;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        // Direction towards target
        Vector3 dir = (target.transform.position - transform.position).normalized;

        // Move towards target
        transform.position += dir * speed * Time.deltaTime;

        // Rotate to face target
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // If close enough, apply damage and destroy
        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

// add range and remove target following from the projectile
