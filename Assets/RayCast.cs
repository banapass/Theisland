using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{

    Collider[] col;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        col = Physics.OverlapSphere(transform.position, 10.0f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,transform.localPosition = new Vector3(5,2.5f,5));
    }
}
