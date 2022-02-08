using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField] private float viewAngle;
    [SerializeField] private float viewDistance;
    [SerializeField] private LayerMask targetMask;

    private AnimalController animal;
    // Start is called before the first frame update
    void Start()
    {
        animal = GetComponent<AnimalController>();
    }

    // Update is called once per frame
    void Update()
    {
        View();
    }
    private Vector3 BoundaryAngle(float angle_)
    {
        angle_ += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle_ * Mathf.Deg2Rad), 0f, Mathf.Cos(angle_ * Mathf.Deg2Rad));
    }
    private void View()
    {
        Vector3 leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position, leftBoundary, Color.red);
        Debug.DrawRay(transform.position, rightBoundary, Color.red);

        Collider[] target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);
        for (int i = 0; i < target.Length; i++)
        {
            Transform targetTf = target[i].transform;
            if(targetTf.tag == "Player")
            {

                // 범위 안에 들어온 상대의 방향
                Vector3 direction = (targetTf.position - transform.position).normalized;
                Debug.DrawRay(transform.position, direction, Color.blue);
                float angle = Vector3.Angle(direction, transform.forward);
                if(angle < viewAngle * 0.5f)
                {
                    RaycastHit hit;
                    if(Physics.Raycast(transform.position,direction,out hit, viewDistance))
                    {
                        
                        if (hit.transform.tag == "Player" && animal.hitCount == true)
                        {
                
                            animal.Run(hit.transform.position);

                        }
                    }
                }
            }
        }
    }
    
}
