using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookObj : MonoBehaviour
{
    [SerializeField] private Transform lookTarget;
    [SerializeField] private Transform moveTarget1;
    [SerializeField] private Transform moveTarget2;
    float movePosX;
    public static bool isDestination;
    // Start is called before the first frame update
    void Start()
    {
        isDestination = false;
        StartCoroutine(MoveCo());
    }

    private void Update()
    {
        LookTarget();
    }

    private void LookTarget()
    {
        transform.LookAt(lookTarget);
    }
    IEnumerator MoveCo()
    {
        float posX = transform.localPosition.x;
        float targetX = moveTarget1.localPosition.x;
        float count = 0;
        
        while (posX != targetX)
        {

            count += Time.deltaTime;
            //count++;

            if (count >= 10)
            {
                movePosX = moveTarget1.localPosition.x;
                transform.position = new Vector3(movePosX, 0f, 0f);
                isDestination = true;
            }
            if (transform.position.x != moveTarget1.position.x)
            {
                posX = Mathf.Lerp(posX, targetX, 0.0008f);
                transform.localPosition = new Vector3(posX, 0f, 0f);
            }
            yield return null;

            

        }
    }
}
