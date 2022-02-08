using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp;
    [SerializeField]
    private GameObject rock;

    private int rockNum;
    // Start is called before the first frame update
    
    public void Mining()
    {
        hp--;
        if(hp <=0)
        {
            CreateRock();
        }
        Debug.Log(hp);
    }
    private void CreateRock()
    {
        rockNum = Random.Range(2, 5);
        Destroy(gameObject);
        for (int i = 0; i < rockNum; i++)
        {
            Instantiate(rock, transform.position + (transform.up * Random.Range(0.5f, 1f)), Quaternion.identity);
        }
        
    }
}
