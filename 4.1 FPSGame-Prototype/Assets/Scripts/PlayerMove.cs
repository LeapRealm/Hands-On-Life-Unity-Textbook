﻿using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 이동 속도 변수
    public float moveSpeed = 7f;
    
    // 캐릭터 컨트롤러 변수
    private CharacterController _characterController;
    
    // 중력 변수
    private float _gravity = -20f;
    
    // 수직 속력 변수
    private float _yVelocity;
    
    // 점프력 변수
    public float jumpPower = 10f;
    
    // 점프 상태 변수
    private bool _isJumping;

    private void Start()
    {
        // 캐릭터 컨트롤러 컴포넌트 받아오기
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // W,A,S,D 키를 누르면 캐릭터를 그 방향으로 이동시키고 싶다.
        // Spacebar 키를 누르면 캐릭터를 수직으로 점프시키고 싶다.
        
        // 1. 사용자의 입력을 받는다.
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        
        // 2. 이동 방향을 설정한다.
        var dir = new Vector3(h, 0, v);
        dir = dir.normalized;
        
        // 2-1. 플레이어 캐릭터를 기준으로 방향을 변환한다.
        dir = transform.TransformDirection(dir);

        // 2-2. 만일, 점프 중이었고, 다시 바닥에 착지했다면
        if (_isJumping && _characterController.collisionFlags == CollisionFlags.Below)
        {
            // 점프 전 상태로 초기화한다.
            _isJumping = false;
            
            // 캐릭터 수직 속도를 0으로 만든다.
            _yVelocity = 0;
        }
        
        // 2-3. 만일, [Spacebar] 키를 눌렀고, 점프하지 않은 상태라면
        if (Input.GetButtonDown("Jump") && !_isJumping)
        {
            // 캐릭터 수직 속도에 점프력을 적용하고 점프 상태로 변경한다.
            _yVelocity = jumpPower;
            _isJumping = true;
        }
        
        // 2-4. 캐릭터 수직 속도에 중력 값을 적용한다.
        _yVelocity += _gravity * Time.deltaTime;
        _yVelocity = Mathf.Clamp(_yVelocity, -3, 10);
        dir.y = _yVelocity;
        
        // 3. 이동 속도에 맞춰 이동한다.
        _characterController.Move((dir * moveSpeed) * Time.deltaTime);
    }
}