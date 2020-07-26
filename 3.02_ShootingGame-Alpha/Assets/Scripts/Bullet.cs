using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 필요 속성: 이동 속도
    public float speed = 5;

    private void Update()
    {
        // 1. 방향을 구한다.
        var dir = Vector3.up;
        
        // 2. 이동하고 싶다. 공식 P = P0 + vt
        transform.position += (dir * speed) * Time.deltaTime;
    }
}