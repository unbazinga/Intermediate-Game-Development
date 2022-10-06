using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugScripts : MonoBehaviour
{
    public bool ShouldShowSpeed;
    public Rigidbody2D playerBody;

    public Text speedText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ShouldShowSpeed)
        {
            Speedometer();
        }
    }

    string Speedometer()
    {
        if(playerBody != null)
        {
            return (speedText.text = (((int)playerBody.velocity.magnitude) * 3.6).ToString("0.0") + " u/s");
        }
        return null;
    }
}
