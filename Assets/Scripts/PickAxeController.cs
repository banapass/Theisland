using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAxeController : MonoBehaviour
{

    [SerializeField]
    LayerMask layerMask;
    RaycastHit hit;

    [SerializeField]
    private float maxDistance;

    private bool isSwing;


    // Update is called once per frame
    void Update()
    {
        TryAction();
    }
    private void TryAction()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartCoroutine(CheckHitCourtine());
        }
    }
    private bool CheckObj()
    {
        if(Physics.Raycast(transform.position,transform.forward,out hit, maxDistance, layerMask))
        {
            return true;
        }
        return false;
    }
    private IEnumerator CheckHitCourtine()
    {
        if(CheckObj() && StatusManager.currentSp > 0)
        {
            isSwing = true;
            while (isSwing) 
            {
                hit.transform.GetComponent<Rock>().Mining();
                StatusManager.instance.DecreaseStamina(15);
                yield return new WaitForSeconds(2);
                isSwing = false;
            }

        }
    }

}
