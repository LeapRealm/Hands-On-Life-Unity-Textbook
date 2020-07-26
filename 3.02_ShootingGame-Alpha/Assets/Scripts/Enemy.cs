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
    
    private void Start()
    {
        // 0부터 9까지 10개의 값 중에 하나를 랜덤으로 가져온다.
        var randValue = Random.Range(0, 10);
        // 만약 3보다 작으면 플레이어 방향
        if (randValue < 3)
        {
            // 플레이어를 찾아 target으로 하고 싶다.
            var target = GameObject.Find("Player");
            // 방향을 구하고 싶다. target-me
            _dir = target.transform.position - transform.position;
            // 방향의 크기를 1로 하고 싶다.
            _dir.Normalize();
        }
        // 그렇지 않으면 아래 방향으로 정하고 싶다.
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
        // 1. 씬에서 ScoreManager 객체를 찾아오자.
        var scoreManagerGameObject = GameObject.Find("ScoreManager");
        // 2. ScoreManager 게임 오브젝트에서 얻어온다.
        var scoreManager = scoreManagerGameObject.GetComponent<ScoreManager>();
        // 3. ScoreManager의 Get/Set 함수로 수정
        scoreManager.SetScore(scoreManager.GetScore() + 1);

        // 2. 폭발 효과 공장에서 폭발 효과를 하나 만들어야 한다.
        var explosion = Instantiate(explosionFactory);
        // 3. 폭발 효과를 발생(위치)시키고 싶다.
        explosion.transform.position = transform.position;
        
        // 너 죽고
        Destroy(other.gameObject);
        // 나 죽자
        Destroy(gameObject);
    }
}