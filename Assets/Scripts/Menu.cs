using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    [SerializeField] private GameObject menuPanel;
    public static bool isMenuOpen;
    // Start is called before the first frame update


    //private void Start()
    //{
    //    OpenMenu();
    //}
    // Update is called once per frame
    private void Start()
    {
        isMenuOpen = false;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TryOpenMenu();
        }
        Debug.Log(isMenuOpen);
    }
    private void TryOpenMenu()
    {
        isMenuOpen = !isMenuOpen;
        if (isMenuOpen && Inventory.inventoryActivated == false)
            OpenMenu();
        else if(isMenuOpen == false)
            CloseMenu();
    }
    private void OpenMenu()
    {
        MouseController.instance.MouseOn();
        menuPanel.SetActive(true);
    }
    private void CloseMenu()
    {
        menuPanel.SetActive(false);
        MouseController.instance.MouseLock();
    }
}
