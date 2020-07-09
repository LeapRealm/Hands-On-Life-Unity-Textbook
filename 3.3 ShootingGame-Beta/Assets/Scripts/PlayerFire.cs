using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // 총알을 생산할 공장
    public GameObject bulletFactory;
    // 총구
    public GameObject firePosition;
    
    // 탄창에 넣을 수 있는 총알의 개수
    public int poolSize = 10;
    // 오브젝트 풀 리스트
    public List<GameObject> bulletObjectPool;

    // 태어날 때 오브젝트 풀(탄창)에 총알을 하나씩 생성해 넣고 싶다.
    // 1. 태어날 때
    private void Start()
    {
        // 2. 탄창을 총알 담을 수 있는 크기로 만들어준다.
        bulletObjectPool = new List<GameObject>();
        
        // 3. 탄창에 넣을 총알 개수만큼 반복해
        for (var i = 0; i < poolSize; i++)
        {
            // 4. 총알 공장에서 총알을 생성한다.
            var bullet = Instantiate(bulletFactory);
            
            // 5. 총알을 오브젝트 풀에 넣고 싶다.
            bulletObjectPool.Add(bullet);
            // 비활성화시키자.
            bullet.SetActive(false);
        }
        
        // 실행되는 플랫폼이 안드로이드일 경우 조이스틱을 활성화시킨다.
        #if UNITY_ANDROID
            GameObject.Find("Joystick canvas XYBZ").SetActive(true);
        #elif UNITY_EDITOR || UNITY_STANDALONE
            GameObject.Find("Joystick canvas XYBZ").SetActive(false);
        #endif
    }

    // 목표: 발사 버튼을 누르면 탄창에 있는 총알 중 비활성화된 것을 발사하고 싶다.
    private void Update()
    {
        // 유니티 에디터와 PC(Mac, Windows, Linux) 환경일 때 동작
        #if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetButtonDown("Fire1"))
            {
                Fire();
            }
        #endif
    }

    public void Fire()
    {
        // 2. 탄창 안에 총알이 있다면
        if (bulletObjectPool.Count > 0)
        {
            // 3. 비활성화된 총알을 하나 가져온다.
            var bullet = bulletObjectPool[0];
            // 4. 총알을 발사하고 싶다(활성화시킨다).
            bullet.SetActive(true);
            // 오브젝트 풀에서 총알 제거
            bulletObjectPool.Remove(bullet);
            // 총알을 위치시키기
            bullet.transform.position = transform.position;
        }
    }
}