using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public static MouseController instance;
    // Update is called once per frame
    private void Start()
    {
        instance = this;
        MouseLock();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            MouseOn();
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            MouseLock();
        }
        
    }
    public void MouseOn()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void MouseLock()
    {
        Cursor.visible = false; // Ä¿¼­ ¼û±â±â 
        Cursor.lockState = CursorLockMode.Locked;
    }
    
}
