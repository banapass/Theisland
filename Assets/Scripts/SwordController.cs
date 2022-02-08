using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField]
    LayerMask layerMask;
    RaycastHit hit;

    [SerializeField]
    private float maxDistance;

   

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);
        TryAction();
    }
    private void TryAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Attack/" + "TryAction()");
            Attack();
        }
    }
    private bool CheckObj()
    {
        Debug.Log("Attack/" + "CheckObj()");

        
        bool isCheck = false ;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, layerMask)) 
        {
            hit.transform.GetComponent<AnimalController>().nav.ResetPath();
            isCheck = true;

        }
        Debug.Log("Attack/" + StatusManager.currentSp);
        return isCheck;
    }
    private void Attack()
    {
        
        if (CheckObj() && StatusManager.currentSp > 0 && ActionController.currentEquip == "Sword")
        {
            Debug.Log("Attack/" + "Attack()");
            hit.transform.GetComponent<AnimalController>().Hunting(1, transform.position);
            StatusManager.instance.DecreaseStamina(15);
            
        }
    }
}
