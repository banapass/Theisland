using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    private List<Collider> colList = new List<Collider>();

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 2)
            colList.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 2)
            colList.Remove(other);
    }
}
