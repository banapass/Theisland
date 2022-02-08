using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField]
    private float hp;
    public GameObject wood;

    private int woodNum;
    
    public void Tree2()
    {
        hp--;
        if(hp <=0)
        {
            CreateWooden();
        }
    }
    public void CreateWooden()
    {
        woodNum = Random.Range(2, 5);
        Destroy(gameObject);
        for (int i = 0; i < woodNum; i++)
        {
            Instantiate(wood, transform.position + (transform.up * Random.Range(1f,2f)), Quaternion.identity);
        }
        
    }

}
