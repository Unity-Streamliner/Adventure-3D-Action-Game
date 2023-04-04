using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public bool MouseButtonDown;
    public float HorizontalInput;
    public float VerticalInput;
    public bool SpaceKeyDown;
    
    private string horizontalTag = "Horizontal";
    private string verticalTag = "Vertical";

    private void Update()
    {
        if (!MouseButtonDown && Time.timeScale != 0)
        {
            MouseButtonDown = Input.GetMouseButtonDown(0);
        }
        if (!SpaceKeyDown && Time.deltaTime != 0)
        {
            SpaceKeyDown = Input.GetKeyDown(KeyCode.Space);
        }
        print($"SpaceKey {SpaceKeyDown} ${Time.deltaTime}");
        HorizontalInput = Input.GetAxis(horizontalTag);
        VerticalInput = Input.GetAxis(verticalTag);
    }

    private void OnDisable()
    {
        ClearCache();
    }

    public void ClearCache()
    {
        MouseButtonDown = false;
        SpaceKeyDown = false;
        HorizontalInput = 0;
        VerticalInput = 0;
    }
}
