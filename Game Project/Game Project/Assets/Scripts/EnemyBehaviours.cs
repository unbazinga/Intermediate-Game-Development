using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviours : MonoBehaviour
{
    public float targetDistance;
    public float damageDistance;
    public float moveSpeed;
    public float maxSpeed;
    public float currentSpeed;
    public float acceleration;
    public float maxDamage, minDamage;
    private float damage;
    public GameObject player;
    [SerializeField] private float hitCooldownMax;
    [SerializeField] private float currentHitCooldown;
    public float hitCooldownDelay;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        currentHitCooldown += (Time.deltaTime / 2f);
        if(currentHitCooldown > hitCooldownMax)
            currentHitCooldown = hitCooldownMax;
        
        Vector3 displacement = player.transform.position - transform.position;
        displacement = displacement.normalized;
        if (Vector2.Distance(player.transform.position, transform.position) <= targetDistance)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
            if (currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
            }
            transform.position += (displacement * currentSpeed * Time.deltaTime);

        }

        if (Vector2.Distance(player.transform.position, transform.position) <= damageDistance)
        {
            if (currentHitCooldown >= hitCooldownMax)
            {
                player.SendMessage("TakeDamage", RollDamage());
                currentHitCooldown = 0;
            }
        }
    }
    float RollDamage()
    {
        return damage = Random.Range(minDamage, maxDamage);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, targetDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageDistance);
    }
}
