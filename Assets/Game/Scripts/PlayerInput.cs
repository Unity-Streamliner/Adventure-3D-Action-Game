using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public bool MouseButtonDown;
    public float HorizontalInput;
    public float VerticalInput;
    
    private string horizontalTag = "Horizontal";
    private string verticalTag = "Vertical";

    void Update()
    {
        if (!MouseButtonDown && Time.timeScale != 0)
        {
            MouseButtonDown = Input.GetMouseButtonDown(0);
        }
        HorizontalInput = Input.GetAxis(horizontalTag);
        VerticalInput = Input.GetAxis(verticalTag);
    }

    private void OnDisable()
    {
        MouseButtonDown = false;
        HorizontalInput = 0;
        VerticalInput = 0;
    }
}
