using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    
    // 무기 모드 텍스트
    public Text weaponModeText;
    
    // 무기 모드 변수
    private enum WeaponMode
    {
        Normal,
        Sniper
    }

    private WeaponMode _weaponMode;
    
    // 카메라 확대 확인용 변수
    private bool _zoomMode;
    
    // 총 발사 효과 오브젝트 배열
    public GameObject[] muzzleFlashs;

    // 무기 아이콘 스프라이트 변수
    public GameObject weapon01;
    public GameObject weapon02;
    
    // 크로스헤어 스프라이트 변수
    public GameObject crosshair01;
    public GameObject crosshair02;
    
    // 마우스 오른쪽 버튼 클릭 아이콘 스프라이트 변수
    public GameObject weapon01_R;
    public GameObject weapon02_R;
    
    // 마우스 우클릭 줌 모드 스프라이트 변수
    public GameObject crosshair02_zoom;
    
    private void Start()
    {
        _camera = Camera.main;
        
        // 피격 이펙트 오브젝트에서 파티클 시스템 컴포넌트 가져오기
        _bulletEffectParticleSystem = bulletEffectPrefabInstance.GetComponent<ParticleSystem>();
        
        // 애니메이터 컴포넌트 가져오기
        _animator = GetComponentInChildren<Animator>();
        
        // 무기 기본 모드를 노멀 모드로 설정한다.
        _weaponMode = WeaponMode.Normal;
    }

    private void Update()
    {
        // 게임 상태가 '게임 중' 상태일 때만 조작할 수 있게 한다.
        if (GameManager.Instance.currentGameState != GameManager.GameState.Run)
        {
            return;
        }
        
        // 노멀 모드: 마우스 오른쪽 버튼을 누르면 시선 방향으로 수류탄을 던지고 싶다.
        // 스나이퍼 모드: 마우스 오른쪽 버튼을 누르면 화면을 확대하고 싶다.
        
        // 1. 마우스 오른쪽 버튼을 입력받는다.
        if (Input.GetMouseButtonDown(1))
        {
            switch (_weaponMode)
            {
                case WeaponMode.Normal:
                    // 수류탄 오브젝트를 생성한 후 수류탄의 생성 위치를 발사 위치로 한다.
                    var bomb = Instantiate(bombFactory);
                    bomb.transform.position = firePosition.transform.position;
            
                    // 수류탄 오브젝트의 Rigidbody 컴포넌트를 가져온다.
                    var bombRigidbody = bomb.GetComponent<Rigidbody>();
            
                    // 카메라의 정면 방향으로 수류탄에 물리적인 힘을 가한다.
                    bombRigidbody.AddForce(_camera.transform.forward * throwPower, ForceMode.Impulse);
                    break;
                
                case WeaponMode.Sniper:
                    // 만일, 줌 모드 상태가 아니라면 카메라를 확대하고 줌 모드 상태로 변경한다.
                    if (!_zoomMode)
                    {
                        _camera.fieldOfView = 15f;
                        _zoomMode = true;
                        
                        // 줌 모드일 때 크로스헤어를 변경한다.
                        crosshair02_zoom.SetActive(true);
                        crosshair02.SetActive(false);
                    }
                    // 그렇지 않으면 카메라를 원래 상태로 되돌리고 줌 모드 상태를 해제한다.
                    else
                    {
                        _camera.fieldOfView = 60f;
                        _zoomMode = false;
                        
                        // 크로스헤어를 스나이퍼 모드로 돌려놓는다.
                        crosshair02_zoom.SetActive(false);
                        crosshair02.SetActive(true);
                    }

                    break;
            }
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
            // 총 이펙트를 실시한다.
            StartCoroutine(ShootEffectOn(0.05f));
        }

        // 만일 키보드의 숫자 1번 입력을 받으면, 무기 모드를 일반 모드로 변경한다.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _weaponMode = WeaponMode.Normal;
            
            // 스나이퍼 모드에서 일반 모드 키를 눌렀을 때
            // Weapon01_R_zoom은 비활성화되고, 줌 모드는 해제
            crosshair02_zoom.SetActive(false);
            
            // 카메라의 화면을 다시 원래대로 돌려준다.
            _camera.fieldOfView = 60f;
            _zoomMode = false;
            
            // 일반 모드 텍스트 출력
            weaponModeText.text = "Normal Mode";
            
            // 1번 스프라이트는 활성화되고, 2번 스프라이트는 비활성화된다.
            weapon01.SetActive(true);
            weapon02.SetActive(false);
            crosshair01.SetActive(true);
            crosshair02.SetActive(false);
            
            // Weapon01_R은 활성화되고, Weapon02_R은 비활성화된다.
            weapon01_R.SetActive(true);
            weapon02_R.SetActive(false);
        }
        // 만일 키보드의 숫자 2번 입력을 받으면, 무기 모드를 스나이퍼 모드로 변경한다.
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _weaponMode = WeaponMode.Sniper;
            
            // 스나이퍼 모드 텍스트 출력
            weaponModeText.text = "Sniper Mode";
            
            // 1번 스프라이트는 비활성화되고, 2번 스프라이트는 활성화된다.
            weapon01.SetActive(false);
            weapon02.SetActive(true);
            crosshair01.SetActive(false);
            crosshair02.SetActive(true);
            
            // Weapon01_R은 비활성화되고, Weapon02_R은 활성화된다.
            weapon01_R.SetActive(false);
            weapon02_R.SetActive(true);
        }
    }
    
    // 총구 이펙트 코루틴 함수
    private IEnumerator ShootEffectOn(float duration)
    {
        // 숫자를 랜덤하게 뽑는다.
        var index = Random.Range(0, muzzleFlashs.Length);
        
        // 이펙트 오브젝트 배열에서 뽑힌 숫자에 해당하는 이펙트 오브젝트를 활성화한다.
        muzzleFlashs[index].SetActive(true);
        
        // 지정한 시간만큼 기다린다.
        yield return new WaitForSeconds(duration);
        
        // 이펙트 오브젝트를 다시 비활성화한다.
        muzzleFlashs[index].SetActive(false);
    }
}