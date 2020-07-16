using UnityEngine;

public class BombAction : MonoBehaviour
{
    // 폭발 이펙트 프리팹 변수
    public GameObject bombEffect;
    
    // 수류탄 데미지
    public int attackPower = 10;
    
    // 폭발 효과 반경
    public float explosionRadius = 5f;

    private Collider[] _colliderCache;
    private const int ColliderCacheSize = 16;

    private void Awake()
    {
        _colliderCache = new Collider[ColliderCacheSize];
    }

    // 충돌했을 때의 처리
    private void OnCollisionEnter()
    {
        // 폭발 효과 반경 내에서 레이어가 'Enemy'인 모든 게임 오브젝트들의 Collider 컴포넌트를 배열에 저장한다.
        var colliderCount = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, _colliderCache, 1 << 10);
        
        // 저장된 Collider 배열에 있는 모든 에너미에게 수류탄 데미지를 적용한다.
        for (var i = 0; i < colliderCount; i++)
        {
            _colliderCache[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);
        }
        
        // 이펙트 프리팹을 생성한다.
        var effect = Instantiate(bombEffect);
        
        // 이펙트 프리팹의 위치는 수류탄 오브젝트 자신의 위치와 동일하다.
        effect.transform.position = transform.position;
        
        // 자기 자신을 제거한다.
        Destroy(gameObject);
    }
}