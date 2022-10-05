using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviours : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Rigidbody2D _rb;
    public GameObject healthUIObject;
    public float moveSpeed = 5f;
    public float maxSpeed;
    public float acceleration;
    public float currentSpeed;
    public float cameraDamp;
    public float offsetX, offsetY;
    public float health;
    [SerializeField] private bool isDead;
    public SpriteRenderer hpSprite;

    public float colorSpeed;
    public float healthUpdateDelay;
    public float healthUpdateTimer;
    public bool shouldUpdateHealth;
    // Start is called before the first frame update
    void Start()
    {
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        shouldUpdateHealth = true;
    }

    // Update is called once per frame
    void Update()
    {
        //healthUpdateTimer += Time.deltaTime;
        //if(healthUpdateTimer >= healthUpdateDelay)
        //{
        //    shouldUpdateHealth = true;
        //    healthUpdateTimer = 0;
        //}

        //if(shouldUpdateHealth)
        //{
        //    UpdateHealth();
        //}
    }

    void FixedUpdate()
    {
        UpdateCameraPosition();
        if(!isDead)
        {
            Movement();
            //Inputs();
        }
    }

    void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if(horizontal > 0.1f || vertical > 0.1f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
            if(currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
            }
        }
        _rb.velocity = (new Vector3(horizontal, vertical) * (currentSpeed * 100)) * Time.deltaTime;
    }
    void UpdateCameraPosition()
    {
        float newX = this.transform.position.x + offsetX;
        float newY = this.transform.position.y + offsetY;

        if(Mathf.Abs(_camera.transform.position.x - newX) > 0.1f)
            newX = Mathf.Lerp(_camera.transform.position.x, newX, cameraDamp * Time.deltaTime);

        if(Mathf.Abs(_camera.transform.position.y - newY) > 0.1f)
            newY = Mathf.Lerp(_camera.transform.position.y, newY, cameraDamp * Time.deltaTime);

        _camera.transform.position = new Vector3(newX, newY, _camera.transform.position.z);
        
    }


    #region Health
    public void TakeDamage(float amount)
    {
        if (health <= 0)
            isDead = true;
        else
        {
            health -= amount;
        }
    }

    //enum HealthStages
    //{
    //    HEALTHY,
    //    INGURED,
    //    HEAVILYINJURED,
    //    DEAD
    //}
    //void UpdateHealth()
    //{
    //    HealthStages healthStages = HealthStages.HEALTHY;
    //    if (health >= 12) healthStages = HealthStages.HEALTHY;
    //    else if (health >= 6 && health <= 11) healthStages = HealthStages.INGURED;
    //    else if (health <= 5 && health >= 1) healthStages = HealthStages.HEAVILYINJURED;
    //    else if (health <= 0) healthStages = HealthStages.DEAD;
    //    switch(healthStages)
    //    {
    //        case HealthStages.HEALTHY:
    //            StartCoroutine(FadeColor(new Color(168, 255, 100)));
    //            //img.color = Color.Lerp(img.color, new Color(168, 255, 100), Time.deltaTime);
    //            break;
    //        case HealthStages.INGURED:
    //            StartCoroutine(FadeColor(new Color(49, 107, 0)));
    //            //img.color = Color.Lerp(img.color, new Color(49,107,0), Time.deltaTime);
    //            break;
    //        case HealthStages.HEAVILYINJURED:
    //            StartCoroutine(FadeColor(new Color(63, 51, 60)));
    //            //img.color = Color.Lerp(img.color, new Color(63, 51, 60), Time.deltaTime);
    //            break;
    //        case HealthStages.DEAD:
    //            StartCoroutine(FadeColor(new Color(63, 0, 0)));
    //            //img.color = Color.Lerp(img.color, new Color(63, 0, 0), Time.deltaTime);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    #endregion
}
