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
    private float currentPickUpTime;
    private bool OnPickUp;
    public float rotationSpeed;
    [SerializeField] private List<PickUpSO> mPickUpSOs = new List<PickUpSO>();
    private PickUps onPickup;
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
        if(!OnPickUp)
        {
            currentPickUpTime = 0;
        } else
        {
            PickUp();
        }
        var rbRot = _rb.rotation;
        rbRot += rotationSpeed * Time.deltaTime;
        _rb.rotation = rbRot;
    }

    void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (horizontal > 0.1f || vertical > 0.1f || horizontal < -0.1f || vertical < -0.1f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
            if (currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
            }
        }
        else
            currentSpeed = Mathf.Lerp(currentSpeed, 0, _rb.drag * Time.deltaTime);
        _rb.velocity = (new Vector3(horizontal, vertical).normalized * (currentSpeed * 100)) * Time.deltaTime;
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

    void PickUp()
    {
        currentPickUpTime += Time.deltaTime;
        if(currentPickUpTime >= onPickup.GetPickUpDefinition().PickUpTime)
        {
            if(!mPickUpSOs.Contains(onPickup.GetPickUpDefinition()))
                mPickUpSOs.Add(onPickup.GetPickUpDefinition());
            else 
            onPickup.OnPickup();
            PrintPickUps(mPickUpSOs.ToArray());
            //Debug.Log(mPickUpSOs.ToArray().ToString());
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entering Trigger");
        if(collision.TryGetComponent(out PickUps pickUps))
        {
            if (mPickUpSOs.Contains(pickUps.GetPickUpDefinition()))
                OnPickUp = false;
            onPickup = pickUps;
            OnPickUp = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PickUps pickUps))
            OnPickUp = false;
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
    void PrintPickUps(PickUpSO[] pIn)
    {
        foreach(PickUpSO s in pIn)
        {
            Debug.Log(s.name); 
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
