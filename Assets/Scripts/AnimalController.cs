using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalController : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float applySpeed;
    [SerializeField] private float waitTime;
    [SerializeField] private float animalAngle;
    [SerializeField] private float moveRangeMin;
    [SerializeField] private float moveRangeMax;
    private float hitResetTime;
    public  bool hitCount;
    public float hitTime;
    private float currentTime;
    private int randomNum;
    private int createRandomNum;
    private bool isAction;
    private bool isWalk;
    private bool isRun;

    //Vector3 direction;
    Vector3 destination;
    //private Rect baseRect;

    [SerializeField] private Animator anim;
    //[SerializeField] private Rigidbody rigid;
    //[SerializeField] private BoxCollider col;
    [SerializeField] private GameObject meat;
    public NavMeshAgent nav;
    
    // Start is called before the first frame update
    void Start()
    {
        hitTime = 10;
        currentTime = waitTime;
        isAction = true;
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //Rotation();
        ElapseTime();
    }
    private void Move()
    {
        if (isWalk || isRun)
            nav.SetDestination(transform.position + destination * Random.Range(3f,5f));
            //rigid.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
    }
    //private void Rotation()
    //{
    //    if (isWalk || isRun)
    //    {
    //        Vector3 rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f,direction.y,0f), 0.05f);
    //        rigid.MoveRotation(Quaternion.Euler(rotation));
    //    }
    //}
    public void Hunting(int dmg, Vector3 targetPos)
    {
        Debug.Log("Attack/" + "Hunting()");
        hp -= dmg;
        hitCount = true;
        if (hitCount == true)
        {
            Run(targetPos);
        }
        if (hp <= 0)
        {
            CreateMeat();
            Destroy(gameObject);
        }
    }
    private void CreateMeat()
    {
        createRandomNum = Random.Range(2, 5);
        for (int i = 0; i < createRandomNum; i++)
        {
            Instantiate(meat, transform.position, Quaternion.identity);
        }
    }
    // 현재 시간(WaitTime)에서 deltaTime 만큼 딜레이를 주고 0이 될 시 행동 초기화
    private void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            hitTime -= Time.deltaTime;
            if (hitTime <= 0)
                hitCount = false;
            if (currentTime <= 0)
            {
                ReSet();
            }
        }
    }
    // 걷기시 방향 360도 내에서 랜덤 설정 
    private void ReSet()
    {
        isWalk = false;
        isRun = false;
        isAction = true;
        nav.ResetPath();

        anim.SetBool("Walk", isWalk);
        anim.SetBool("Run", isRun);
        destination.Set(Random.Range(-animalAngle, animalAngle), 0f, Random.Range(3f, 5f));
        //direction.Set(0f, Random.Range(0f, 360f),0f);
        RandomAction();
        Debug.Log("ReSet");
    }
    // 행동 랜덤 설정
    private void RandomAction()
    {
        randomNum = Random.Range(0, 2);
        if (randomNum == 0)
        {
            //Debug.Log("Walk");
            Walk();
        }
        else if (randomNum == 1)
        {
            //Debug.Log("Eat");
            Eat();
        }
        Debug.Log("RandomAction");

    }
    // 행동
    private void Walk()
    {
        currentTime = waitTime;
        isWalk = true;
        isRun = false;
        //applySpeed = walkSpeed;
        nav.speed = walkSpeed;
        anim.SetBool("Walk",isWalk);
        anim.SetBool("Run", isRun);
    }
    public void Run(Vector3 targetPos)
    {
        //direction = Quaternion.LookRotation(transform.position - targetPos).eulerAngles;
        // 플레이어 반대 방향
        destination = new Vector3(transform.position.x - targetPos.x, 0f, transform.position.z - targetPos.z).normalized;
        nav.ResetPath();
        // 대기 시간
        hitTime = 10;
        currentTime = waitTime;

        // 애니메이션
        isWalk = false;
        isRun = true;
        //applySpeed = runSpeed;
        nav.speed = runSpeed;
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Run",isRun);
        
    }
    private void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
    }
    
}
