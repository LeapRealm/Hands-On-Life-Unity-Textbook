using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // 발사 위치
    public GameObject firePosition;
    
    // 투척 무기 오브젝트
    public GameObject bombFactory;
    
    // 투척 파워
    public float throwPower = 15f;
    
    // 메인 카메라 오브젝트
    private Camera _camera;
    
    // 피격 이펙트 오브젝트
    public GameObject bulletEffectPrefabInstance;
    
    // 피격 이펙트 파티클 시스템
    private ParticleSystem _bulletEffectParticleSystem;
    
    // 발사 무기 공격력
    public int weaponPower = 5;
    
    // 애니메이터 변수
    private Animator _animator;

    private void Start()
    {
        _camera = Camera.main;
        
        // 피격 이펙트 오브젝트에서 파티클 시스템 컴포넌트 가져오기
        _bulletEffectParticleSystem = bulletEffectPrefabInstance.GetComponent<ParticleSystem>();
        
        // 애니메이터 컴포넌트 가져오기
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // 게임 상태가 '게임 중' 상태일 때만 조작할 수 있게 한다.
        if (GameManager.Instance.currentGameState != GameManager.GameState.Run)
        {
            return;
        }
        
        // 마우스 오른쪽 버튼을 누르면 시선이 바라보는 방향으로 수류탄을 던지고 싶다.
        
        // 1. 마우스 오른쪽 버튼을 입력받는다.
        if (Input.GetMouseButtonDown(1))
        {
            // 수류탄 오브젝트를 생성한 후 수류탄의 생성 위치를 발사 위치로 한다.
            var bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;
            
            // 수류탄 오브젝트의 Rigidbody 컴포넌트를 가져온다.
            var bombRigidbody = bomb.GetComponent<Rigidbody>();
            
            // 카메라의 정면 방향으로 수류탄에 물리적인 힘을 가한다.
            bombRigidbody.AddForce(_camera.transform.forward * throwPower, ForceMode.Impulse);
        }
        
        // 마우스 왼쪽 버튼을 누르면 시선이 바라보는 방향으로 총을 발사하고 싶다.
        
        // 마우스 왼쪽 버튼을 입력받는다.
        if (Input.GetMouseButtonDown(0))
        {
            // 만일 이동 블렌드 트리 파라미터의 값이 0이라면, 공격 애니메이션을 실시한다.
            if (_animator.GetFloat("MoveMotion") <= 0)
            {
                _animator.SetTrigger("Attack");
            }
            
            // 레이를 생성한 후 발사될 위치와 진행 방향을 설정한다.
            var ray = new Ray(_camera.transform.position, _camera.transform.forward);
            
            // 레이가 부딪힌 대상의 정보를 저장할 변수를 생성한다.
            var hitInfo = new RaycastHit();
            
            // 레이를 발사하고, 만일 부딪힌 물체가 있으면
            if (Physics.Raycast(ray, out hitInfo))
            {
                // 만일 레이에 부딪힌 대상의 레이어가 'Enemy'라면 데미지 함수를 실행한다.
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    var enemyFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    enemyFSM.HitEnemy(weaponPower);
                }
                // 그렇지 않다면, 레이에 부딪힌 지점에 피격 이펙트를 플레이한다.
                else
                {
                    // 피격 이펙트의 위치를 레이가 부딪힌 지점으로 이동시킨다.
                    bulletEffectPrefabInstance.transform.position = hitInfo.point;
                
                    // 피격 이펙트의 forward 방향을 레이가 부딪힌 지점의 법선 벡터와 일치시킨다.
                    bulletEffectPrefabInstance.transform.forward = hitInfo.normal;
                
                    // 피격 이펙트를 플레이한다.
                    _bulletEffectParticleSystem.Play();
                }
            }
        }
    }
}