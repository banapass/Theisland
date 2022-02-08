using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    //캐릭터 움직임 관련
    [SerializeField]
    private float walkSpeed;// 걷기 시 이동속도
    [SerializeField]
    private float runSpeed; // 달리기 시 이동속도
    [SerializeField]
    private float crouchSpeed; // 앉을 시 이동속도
    [SerializeField]
    private float applySpeed; // 현재 적용된 이동속도
    [SerializeField]
    private float jumpForce; // 점프 세기
    [SerializeField]
    private float decreaseJump; // 점프시 스태미나 소모 수치

    // 카메라 관련
    [SerializeField]
    private float lookSens; // 마우스 감도
    [SerializeField]
    private float lookLimit; // Y축 제한
    private float currentCameraRotationX;
    [SerializeField]
    private float crouchPosY; // 앉았을 시 시점 설정
    private float defaultPosY; // 기본 상태 시점 
    private float applyCrouchPosY; // 현제 적용된 시점
    
    // 상태 체크
    public static bool isGround; 
    public static bool isRun;
    private bool isCrouch; 

   

    // 컴포넌트
    [SerializeField] private new Camera camera;
    [SerializeField] private Inventory inventory;
    private CapsuleCollider capsuleCol;
    private Rigidbody rigid;

    // Update is called once per frame
    void Start()
    {
        applySpeed = walkSpeed;
        capsuleCol = GetComponent<CapsuleCollider>();
        rigid = GetComponent<Rigidbody>();
        defaultPosY = camera.transform.localPosition.y;
        applyCrouchPosY = defaultPosY;
    }
    void Update()
    {
        Move();
        if (Inventory.inventoryActivated == false && Menu.isMenuOpen == false)
        {
            
            CameraRotation();
            PlayerRotation();
            Running();
            TryCrouch();
            IsGround();
            Jump();
            //Debug.Log(StatusManager.IsUsedStamina);
        }
    }
    // 플레이어 이동
    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirY = Input.GetAxisRaw("Vertical");

        Vector3 moveX = transform.right * moveDirX;
        Vector3 moveY = transform.forward * moveDirY;

        Vector3 velocity = (moveX + moveY).normalized * applySpeed;
        rigid.MovePosition(transform.position + velocity * Time.deltaTime);
    }
    
    // 카메라 Y축 시점 이동
    private void CameraRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * lookSens;

        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -lookLimit, lookLimit);

        camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    // 카메라 좌우 시점 이동에 따른 캐릭터 회전
    private void PlayerRotation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 playerRotationY = new Vector3(0f, yRotation, 0f) * lookSens;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(playerRotationY));
    }
    // 점프
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround == true && StatusManager.currentSp > 0 ) 
        {
            StatusManager.instance.DecreaseStamina(decreaseJump);
            rigid.velocity = transform.up * jumpForce;

        }
    }
    
    // 땅 체크
    private void IsGround()
    {
        Debug.DrawRay(transform.position, Vector3.down * (capsuleCol.bounds.extents.y+ 0.1f), Color.black);
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCol.bounds.extents.y + 0.1f);
    }
    // 달리기 시작 및 종료
    public void Running()
    {
        if(Input.GetKey(KeyCode.LeftShift) && isGround == true&& StatusManager.CurrentSp > 0)
        {
            Run();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) || StatusManager.CurrentSp <= 0)
        {
            RunCancel();
        }
    }
    public void Run()
    {
        if(StatusManager.currentSp > 0)
        {
            if (isCrouch)
                Crouch();
            StatusManager.IsUsedStamina = true;
            isRun = true;
            applySpeed = runSpeed;
        }
        if (StatusManager.currentSp < 0)
            isRun = false;
    }
    public void RunCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }
    // 앉기
    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    private void Crouch()
    {
        isCrouch = !isCrouch; // 반전
        if(isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = defaultPosY;
        }
        StartCoroutine(CrouchCoroutine());
    }
    //  앉기 시 부드러운 카메라 이동
    IEnumerator CrouchCoroutine()
    {
        float posY = camera.transform.localPosition.y;
        int count = 0;
        while(posY != applyCrouchPosY)
        {
            count++;
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.2f);
            camera.transform.localPosition = new Vector3(0, posY, 0);
            if (count > 10)
                break;
            yield return null;
        }
        camera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }
    

 
}
