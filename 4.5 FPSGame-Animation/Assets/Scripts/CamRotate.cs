using UnityEngine;

public class CamRotate : MonoBehaviour
{
    // 회전 속도 변수
    public float rotSpeed = 200f;
    
    // 회전 값 변수
    private float _mx;
    private float _my;
    
    private void Update()
    {
        // 게임 상태가 '게임 중' 상태일 때만 조작할 수 있게 한다.
        if (GameManager.Instance.currentGameState != GameManager.GameState.Run)
        {
            return;
        }
        
        // 사용자의 마우스 입력을 받아 물체를 회전시키고 싶다.
        
        // 1. 마우스 입력을 받는다.
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");
        
        // 1-1. 회전 값 변수에 마우스 입력 값만큼 미리 누적시킨다.
        _mx += (mouseX * rotSpeed) * Time.deltaTime;
        _my += (mouseY * rotSpeed) * Time.deltaTime;
        
        // 1-2. 마우스 상하 이동 회전 변수(my)의 값을 -90도~90도 사이로 제한한다.
        _my = Mathf.Clamp(_my, -90f, 90f);
        
        // 2. 물체를 회전 방향으로 회전시킨다.
        transform.eulerAngles = new Vector3(-_my, _mx, 0);
    }
}