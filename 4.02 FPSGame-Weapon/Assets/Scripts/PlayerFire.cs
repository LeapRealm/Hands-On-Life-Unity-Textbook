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

    private void Start()
    {
        _camera = Camera.main;
        
        // 피격 이펙트 오브젝트에서 파티클 시스템 컴포넌트 가져오기
        _bulletEffectParticleSystem = bulletEffectPrefabInstance.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
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
            // 레이를 생성한 후 발사될 위치와 진행 방향을 설정한다.
            var ray = new Ray(_camera.transform.position, _camera.transform.forward);
            
            // 레이가 부딪힌 대상의 정보를 저장할 변수를 생성한다.
            var hitInfo = new RaycastHit();
            
            // 레이를 발사한 후 만일 부딪힌 물체가 있으면 피격 이펙트를 표시한다.
            if (Physics.Raycast(ray, out hitInfo))
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