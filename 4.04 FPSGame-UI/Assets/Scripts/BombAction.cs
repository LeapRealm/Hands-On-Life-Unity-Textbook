using UnityEngine;

public class BombAction : MonoBehaviour
{
    // 폭발 이펙트 프리팹 변수
    public GameObject bombEffect;
    
    // 충돌했을 때의 처리
    private void OnCollisionEnter()
    {
        // 이펙트 프리팹을 생성한다.
        var effect = Instantiate(bombEffect);
        
        // 이펙트 프리팹의 위치는 수류탄 오브젝트 자신의 위치와 동일하다.
        effect.transform.position = transform.position;
        
        // 자기 자신을 제거한다.
        Destroy(gameObject);
    }
}