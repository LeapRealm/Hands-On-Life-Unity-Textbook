using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    // 필요 속성: 이동 속도
    public float speed = 5;
    // 방향을 전역 변수로 만들어 Start와 Update에서 사용
    private Vector3 _dir;

    // 폭발 공장 주소(외부에서 값을 넣어준다)
    public GameObject explosionFactory;

    private void OnEnable()
    {
        var player = GameObject.Find("Player");

        if (player != null)
        {
            var randValue = Random.Range(0, 100);
            
            if (randValue < 50)
            {
                // 방향을 구하고 싶다. target-player
                _dir = player.transform.position - transform.position;
                // 방향의 크기를 1로 하고 싶다.
                _dir.Normalize();
            }
            // 그렇지 않으면 아래 방향으로 정하고 싶다.
            else
            {
                _dir = Vector3.down;
            }
        }
        else
        {
            _dir = Vector3.down;
        }
    }

    private void Update()
    {
        // 이동하고 싶다. 공식 P = P0 + vt
        transform.position += (_dir * speed) * Time.deltaTime;
    }

    // 1. 적이 다른 물체와 충돌했으니까.
    private void OnCollisionEnter(Collision other)
    {
        // 에너미를 잡을 때마다 현재 점수를 표시하고 싶다.
        ScoreManager.Instance.Score++;

        // 2. 폭발 효과 공장에서 폭발 효과를 하나 만들어야 한다.
        var explosion = Instantiate(explosionFactory);
        // 3. 폭발 효과를 발생(위치)시키고 싶다.
        explosion.transform.position = transform.position;
        
        // 만약 부딪힌 객체가 Bullet인 경우에는 비활성화시켜 탄창에 다시 넣어준다.
        // 1. 만약 부딪힌 물체가 Bullet이라면
        if (other.gameObject.name.Contains("Bullet"))
        {
            // 2. 부딪힌 물체를 비활성화
            other.gameObject.SetActive(false);
            
            // PlayerFire 클래스 얻어오기
            var playerFire = GameObject.Find("Player").GetComponent<PlayerFire>();
            // 리스트에 총알 삽입
            playerFire.bulletObjectPool.Add(other.gameObject);
        }
        else
        {
            Destroy(other.gameObject);
        }
        
        // Destroy로 없애는 대신, 비활성화해 풀에 자원을 반납합니다.
        gameObject.SetActive(false);
        
        // 리스트에 에너미 삽입
        EnemyManager.Instance.enemyObjectPool.Add(gameObject);
    }
}