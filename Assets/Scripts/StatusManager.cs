using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public static StatusManager instance;
   
    // 상태별 이미지 넣을 배열
    [SerializeField]
    private Image[] images;

    // 최대 스테이터스 설정
    [SerializeField] private float hp;

    [SerializeField] private float sp;

    [SerializeField] private float hungry;

    [SerializeField] private float thirsty;


    // 현재 스테이터스
    public static float currentHp;
    public static float currentSp;
    public static float currentHungry;
    public static float currentThirsty;

    // 증가 치 설정
    [SerializeField] private float increaseHp;
    // 감소 치 설정
    [SerializeField] private float decreaseHp;
    [SerializeField] private float decreaseHungry;
    [SerializeField] private float decreaseThirsty;
    [SerializeField] private float decreaseRun;
    
    

    // 스태미나 재충전 시간 설정
    [SerializeField]
    private float rechargingTime;
    private float tickTime;

    // 스테미나 사용 확인
    private static bool isUsedStamina = false;

    public static bool IsUsedStamina
    {
        set
        {
            isUsedStamina = value;
        }
        get
        {
            return isUsedStamina;
        }
    }
    public static float CurrentSp
    {
        set
        {
            currentSp = value;
        }
        get
        {
            return currentSp;
        }
    }
    private enum STATE
    {
        HP,
        SP,
        HUNGRY,
        THIRSTY
    }

   
    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
        currentSp = sp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        isUsedStamina = false;
        instance = this;


    }
    
    // Update is called once per frame
    void Update()
    {
        Gauge();
        IncreaseHp();
        DecreaseHp();
        DecreaseHungry();
        DecreaseThirsty();
        RunStamina();
        RechargingStamina();
        
    }
    
    
    // UI 할당
    private void Gauge()
    {

        images[(int)STATE.HP].fillAmount = currentHp / hp;
        images[(int)STATE.SP].fillAmount = currentSp / sp;
        images[(int)STATE.HUNGRY].fillAmount = currentHungry / hungry;
        images[(int)STATE.THIRSTY].fillAmount = currentThirsty / thirsty;
    }
    //Hp
    // 배고픔 목마름이 0이되면 hp 차감 시작 둘다 0일시 2배
    private void IncreaseHp()
    {
        if(currentHungry > 80)
        {
            currentHp += increaseHp * Time.deltaTime;
        }
        else if(currentThirsty > 80)
        {
            currentHp += increaseHp * Time.deltaTime;
        }
        else if (currentHungry > 80 && currentThirsty > 80)
        {
            currentHp += increaseHp * 2 * Time.deltaTime;
        }
        if (currentHp >= 100)
            currentHp = 100;
    }
    private void DecreaseHp()
    {
        if (currentHungry <= 0)
        {
            currentHp -= decreaseHp * Time.deltaTime;
            if (currentHp <= 0)
                currentHp = 0;
        }
        else if (currentHungry <= 0 && currentThirsty <= 0)
        {
            currentHp -= decreaseHp * 2 * Time.deltaTime;
            if (currentHp <= 0)
                currentHp = 0;
        }
        else if (currentThirsty <= 0)
        {
            currentHp -= decreaseHp * Time.deltaTime;
            if (currentHp <= 0)
                currentHp = 0;
        }
        
    }
    // 체력 0 될시 게임오버 화면 출력
    private void Result()
    {
        if (currentHp <= 0)
            gameObject.SetActive(true);
    }

    //SP
    //달리기 사용 시 스태미나 소모
    private void RunStamina()
    {
        if (PlayerController.isRun)
        {
            currentSp -= decreaseRun * Time.deltaTime;
        }
        else
            isUsedStamina = false;
    }

    //private void JumpStamina()
    //{
    //    if (PlayerController.isGround == false && IsUsedStamina == true)
    //    {

    //        Debug.Log("JumpStamina");
    //        if (IsUsedStamina)
    //        {

    //            currentSp -= decreaseJump;
    //            IsUsedStamina = false;
    //        }

    //    } 
    //}
    // 스태미나 소모
    public void DecreaseStamina(float count)
    {
        isUsedStamina = true;
        tickTime = 0;
        if (currentSp - count > 0)
        {
            currentSp -= count;
        }
        else
        {
            currentSp = 0;
        }
    }
    // 스태미나 0일시 달리기 점프 비활성화
    private void CheckStamina()
    {
        if(currentSp <= 0)
        {
            PlayerController.isRun = false;
        }
    }
    // 스태미나 재회복
    private void RechargingStamina()
    {
        if (isUsedStamina == true)
            tickTime = 0;

        if (isUsedStamina == false)
        {
            tickTime += Time.deltaTime;
            if (tickTime >= rechargingTime)
            {
                currentSp += 30 * Time.deltaTime;
                if (currentSp >= 100)
                    currentSp = 100;
            }

        }
    }

    //Hungry
    //일정 시간마다 감소 및 0일 시 체력감소
    private void DecreaseHungry()
    {
        currentHungry -= decreaseHungry * Time.deltaTime;
        if (currentHungry <= 0)
            currentHungry = 0;
    }

    //Thirsty
    // 일정시가마다 감소 및 0일 시 체력감소
    private void DecreaseThirsty()
    {
        currentThirsty -= decreaseThirsty * Time.deltaTime;
        if (currentThirsty <= 0)
            currentThirsty = 0;
    }
}
