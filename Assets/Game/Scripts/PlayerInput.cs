using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float HorizontalInput;
    public float VerticalInput;
    
    private string horizontalTag = "Horizontal";
    private string verticalTag = "Vertical";

    void Update()
    {
        HorizontalInput = Input.GetAxis(horizontalTag);
        VerticalInput = Input.GetAxis(verticalTag);
    }

    private void OnDisable()
    {
        HorizontalInput = 0;
        VerticalInput = 0;
    }
}
