using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 플레이어가 이동할 속력
    public float speed = 5;
    
    private void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        var dir = new Vector3(h, v, 0);
        
        // P = P0 + vt 공식으로 변경
        transform.position += (dir * speed) * Time.deltaTime;
    }
}