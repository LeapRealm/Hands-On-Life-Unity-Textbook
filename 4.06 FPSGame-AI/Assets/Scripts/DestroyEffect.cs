using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    // 제거될 시간 변수
    public float destroyTime = 1.5f;
    
    // 경과 시간 측정용 변수
    private float _currentTime;

    private void Update()
    {
        // 만일 경과 시간이 제거될 시간을 초과하면 자기 자신을 제거한다.
        if (_currentTime > destroyTime)
        {
            Destroy(gameObject);
        }
        
        // 경과 시간을 누적한다.
        _currentTime += Time.deltaTime;
    }
}