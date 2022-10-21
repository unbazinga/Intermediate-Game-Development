using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBehaviours : MonoBehaviour
{
    [SerializeField] private float hitCooldownMax;
    [SerializeField] private float currentHitCooldown;
    [SerializeField] private float wanderTime;
    [SerializeField] private float explosiveForce;
    [SerializeField] private float defBlowUpTimer;
    [SerializeField] private float blowUpDelayDef;
    private Vector3 playerLastSeen;
    public GameObject player;
    public float targetDistance;
    public float damageDistance;
    public float moveSpeed;
    public float maxSpeed;
    public float currentSpeed;
    public float acceleration;
    public float maxDamage, minDamage;
    private float damage;
    public float hitCooldownDelay;
    public bool wasFollowing;
    private float currentWanderTime;
    private bool canBlowUp;
    private float blowUpTimer;
    private float blowUpDelay;

    // Start is called before the first frame update
    void Start()
    {
        blowUpDelay = 0f;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        currentHitCooldown += (Time.deltaTime / 2f);
        blowUpTimer += Time.deltaTime;
        if(currentHitCooldown > hitCooldownMax)
            currentHitCooldown = hitCooldownMax;
        if (blowUpTimer > defBlowUpTimer)
            blowUpTimer = defBlowUpTimer;
        
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
            wasFollowing = true;
            playerLastSeen = displacement;
        } 

        if (Vector2.Distance(player.transform.position, transform.position) <= damageDistance)
        {
            if (currentHitCooldown >= hitCooldownMax)
            {
                blowUpDelay += Time.deltaTime;
                if (blowUpTimer >= defBlowUpTimer)
                {
                    canBlowUp = true;
                    BlowUp();
                    blowUpTimer = 0f;
                }
            }
        }
        if(Vector2.Distance(player.transform.position, transform.position) >= targetDistance)
        {
            if(wasFollowing)
            {
                currentWanderTime -= Time.deltaTime;
                if(currentWanderTime > 0)
                {
                    transform.position += (playerLastSeen * currentSpeed * Time.deltaTime);
                } else
                {
                    currentWanderTime = wanderTime;
                    wasFollowing = false;
                    
                }
            }
        }
    }
    float RollDamage()
    {
        return damage = Random.Range(minDamage, maxDamage);
    }

    void BlowUp()
    {
        if (!canBlowUp) return;

        if (blowUpDelay >= blowUpDelayDef)
        {
            player.GetComponent<Rigidbody2D>().AddForce(explosiveForce * -player.transform.forward, ForceMode2D.Impulse);
            player.SendMessage("TakeDamage", (RollDamage() / Vector2.Distance(this.transform.position, player.transform.position)));
            canBlowUp = false;
            blowUpDelay = 0;
            Debug.Log("Boom");
            Destroy(this.gameObject);
            
        }
            
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, targetDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageDistance);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, damageDistance * transform.forward);
    }
}
