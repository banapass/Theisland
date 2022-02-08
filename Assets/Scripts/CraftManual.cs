using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Craft
{
    public string craftName;
    public GameObject craftPrefab;
    public GameObject previewPrefab;
}
public class CraftManual : MonoBehaviour
{
    private bool isActivated = false;
    private bool isPreviewActivated = false;

    [SerializeField] private GameObject baseUi;
    [SerializeField] private Craft[] craft_;
    [SerializeField] private Transform tf_Player;

    private GameObject preview;
    private GameObject prefab;

    private RaycastHit hit;
    [SerializeField] LayerMask layerMask;

    [SerializeField] float range;


    // Update is called once per frame
    void Update()
    {
        if (isPreviewActivated)
            PreviewPositionUpdate();
        if (Input.GetKeyDown(KeyCode.Alpha5) && Inventory.inventoryActivated == false)
            Window();
        if (Input.GetMouseButtonDown(0))
            Build();
        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();
    }
    public void SlotClick(int slotNum)
    {
        preview = Instantiate(craft_[slotNum].previewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        prefab = craft_[slotNum].craftPrefab;
        isPreviewActivated = true;
        baseUi.SetActive(false);
    }
    private void PreviewPositionUpdate()
    {
        Debug.Log("Preview");
        if(Physics.Raycast(tf_Player.position,tf_Player.forward,out hit,range,layerMask))
        {
            if (hit.transform != null)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                    preview.transform.Rotate(0f, -90f, 0f);
                else if (Input.GetKeyDown(KeyCode.E))
                    preview.transform.Rotate(0f, 90f, 0f);
                Vector3 location = hit.point;
                //location.Set(Mathf.Round(location.x), Mathf.Round(location.y / 0.1f) * 0.1f, Mathf.Round(location.z));
                preview.transform.position = location;
                MouseController.instance.MouseLock();
            }
        }
    }
    private void Window()
    {
        isActivated = !isActivated;
        if (isActivated)
            OpenWindow();
        else
            CloseWindow();
        
    }
    private void OpenWindow()
    {
        isActivated = true;
        MouseController.instance.MouseOn();
        baseUi.SetActive(true);
    }
    private void CloseWindow()
    {
        isActivated = false;
        baseUi.SetActive(false);
    }
    
    private void Build()
    {
        if(isPreviewActivated)
        {
            Instantiate(prefab, preview.transform.position, preview.transform.rotation);
            Destroy(preview);
            isActivated = false;
            isPreviewActivated = false;
            preview = null;
            prefab = null;
        }
    }
    private void Cancel()
    {
        if (isPreviewActivated)
            Destroy(preview);

        isActivated = false;
        isPreviewActivated = false;
        preview = null;
        prefab = null;
        baseUi.SetActive(false);
    }
    
}
