using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUps : MonoBehaviour
{
    [SerializeField] private PickUpSO PickUpDefinition;

    private void Start()
    {
        if(this.gameObject.TryGetComponent(out SpriteRenderer spriteRenderer)) {
            spriteRenderer.color = PickUpDefinition.PickUpColor;
        }   
    }
    public PickUpSO GetPickUpDefinition()
    {
        return this.PickUpDefinition;
    }
    
    public void SetPickUpDefinition(PickUpSO pickup)
    {
        PickUpDefinition = pickup;
    }

    public void OnPickup()
    {
        Destroy(gameObject);
    }
}
