using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    // 최소 시간
    private float _minTime = 1;
    // 최대 시간
    private float _maxTime = 5;
    // 현재 시간
    private float _currentTime;
    // 일정 시간
    private float _createTime;
    
    // 적 공장
    public GameObject enemyFactory;

    private void Start()
    {
        // 태어날 때 적의 생성 시간을 설정하고
        _createTime = Random.Range(_minTime, _maxTime);
    }

    private void Update()
    {
        // 1. 시간이 흐르다가
        _currentTime += Time.deltaTime;
        
        // 2. 만약 현재 시간이 일정 시간이 되면
        if (_currentTime > _createTime)
        {
            // 3. 적 공장에서 적을 생성해
            var enemy = Instantiate(enemyFactory);
            // 내 위치에 갖다 놓고 싶다.
            enemy.transform.position = transform.position;
            // 현재 시간을 0으로 초기화
            _currentTime = 0;
            // 적을 생성한 후 적의 생성 시간을 다시 설정하고 싶다.
            _createTime = Random.Range(_minTime, _maxTime);
        }
    }
}