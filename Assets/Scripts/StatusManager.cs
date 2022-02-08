using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public static StatusManager instance;
   
    // ���º� �̹��� ���� �迭
    [SerializeField]
    private Image[] images;

    // �ִ� �������ͽ� ����
    [SerializeField] private float hp;

    [SerializeField] private float sp;

    [SerializeField] private float hungry;

    [SerializeField] private float thirsty;


    // ���� �������ͽ�
    public static float currentHp;
    public static float currentSp;
    public static float currentHungry;
    public static float currentThirsty;

    // ���� ġ ����
    [SerializeField] private float increaseHp;
    // ���� ġ ����
    [SerializeField] private float decreaseHp;
    [SerializeField] private float decreaseHungry;
    [SerializeField] private float decreaseThirsty;
    [SerializeField] private float decreaseRun;
    
    

    // ���¹̳� ������ �ð� ����
    [SerializeField]
    private float rechargingTime;
    private float tickTime;

    // ���׹̳� ��� Ȯ��
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
    
    
    // UI �Ҵ�
    private void Gauge()
    {

        images[(int)STATE.HP].fillAmount = currentHp / hp;
        images[(int)STATE.SP].fillAmount = currentSp / sp;
        images[(int)STATE.HUNGRY].fillAmount = currentHungry / hungry;
        images[(int)STATE.THIRSTY].fillAmount = currentThirsty / thirsty;
    }
    //Hp
    // ����� �񸶸��� 0�̵Ǹ� hp ���� ���� �Ѵ� 0�Ͻ� 2��
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
    // ü�� 0 �ɽ� ���ӿ��� ȭ�� ���
    private void Result()
    {
        if (currentHp <= 0)
            gameObject.SetActive(true);
    }

    //SP
    //�޸��� ��� �� ���¹̳� �Ҹ�
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
    // ���¹̳� �Ҹ�
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
    // ���¹̳� 0�Ͻ� �޸��� ���� ��Ȱ��ȭ
    private void CheckStamina()
    {
        if(currentSp <= 0)
        {
            PlayerController.isRun = false;
        }
    }
    // ���¹̳� ��ȸ��
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
    //���� �ð����� ���� �� 0�� �� ü�°���
    private void DecreaseHungry()
    {
        currentHungry -= decreaseHungry * Time.deltaTime;
        if (currentHungry <= 0)
            currentHungry = 0;
    }

    //Thirsty
    // �����ð����� ���� �� 0�� �� ü�°���
    private void DecreaseThirsty()
    {
        currentThirsty -= decreaseThirsty * Time.deltaTime;
        if (currentThirsty <= 0)
            currentThirsty = 0;
    }
}
