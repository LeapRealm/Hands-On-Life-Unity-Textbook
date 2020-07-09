using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    // 영역 안에 다른 물체가 감지될 경우
    private void OnTriggerEnter(Collider other)
    {
        // 1. 만약 부딪힌 물체가 Bullet이거나 Enemy이라면
        if (other.gameObject.name.Contains("Bullet") || other.gameObject.name.Contains("Enemy"))
        {
            // 2. 부딪힌 물체를 비활성화
            other.gameObject.SetActive(false);
            
            // 3. 부딫힌 물체가 총알일 경우 총알 리스트에 삽입
            if (other.gameObject.name.Contains("Bullet"))
            {
                // PlayerFire 클래스 얻어오기
                var player = GameObject.Find("Player");
                if (player != null)
                {
                    var playerFire = player.GetComponent<PlayerFire>();
                    // 리스트에 총알 삽입
                    playerFire.bulletObjectPool.Add(other.gameObject);
                }
            }
            else if (other.gameObject.name.Contains("Enemy"))
            {
                // 리스트에 에너미 삽입
                EnemyManager.Instance.enemyObjectPool.Add(other.gameObject);
            }
        }
    }
}