using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    // 오브젝트 풀 크기
    public int poolSize = 10;
    // 오브젝트 풀 배열
    public List<GameObject> enemyObjectPool;
    // SpawnPoint들
    public Transform[] spawnPoints;

    // 생성할 최소 시간
    public float minTime = 0.5f;
    // 생성할 최대 시간
    public float maxTime = 1.5f;
    // 현재 시간
    private float _currentTime;
    // 생성 시간
    private float _createTime;
    
    // 적 공장
    public GameObject enemyFactory;
    
    // 싱글톤 변수
    public static EnemyManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // 1. 태어날 때
    private void Start()
    {
        // 적의 생성 시간을 설정한다.
        _createTime = Random.Range(minTime, maxTime);
        
        // 2. 오브젝트 풀을 만들어준다.
        enemyObjectPool = new List<GameObject>();
        // 3. 오브젝트 풀에 넣을 에너미 개수만큼 반복해
        for (var i = 0; i < poolSize; i++)
        {
            // 4. 에너미 공장에서 에너미를 생성한다.
            var enemy = Instantiate(enemyFactory);
            // 5. 에너미를 오브젝트 풀에 넣고 싶다.
            enemyObjectPool.Add(enemy);
            // 비활성화시키자.
            enemy.SetActive(false);
        }
    }

    private void Update()
    {
        // 시간이 흐르다가
        _currentTime += Time.deltaTime;
        
        // 1. 생성 시간이 됐으니까
        if (_currentTime > _createTime)
        {
            // 2. 오브젝트 풀에 에너미가 있다면
            if (enemyObjectPool.Count > 0)
            {
                var enemy = enemyObjectPool[0];
                
                // 랜덤으로 인덱스 선택
                var index = Random.Range(0, spawnPoints.Length);
                // 에너미 위치시키기
                enemy.transform.position = spawnPoints[index].position;
                    
                // 에너미 활성화
                enemy.SetActive(true);
                // 오브젝트풀에서 총알 제거
                enemyObjectPool.Remove(enemy);
            }
            
            // 현재 시간을 0으로 초기화
            _currentTime = 0;
            // 적을 생성한 후 적의 생성 시간을 다시 설정하고 싶다.
            _createTime = Random.Range(minTime, maxTime);
        }
    }
}