using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

// 씨네머신을 제어하고 싶다.
public class DirectorAction : MonoBehaviour
{
    // 감독 오브젝트
    private PlayableDirector _playableDirector;
    
    // 카메라 오브젝트
    public Camera targetCam;

    private void Start()
    {
        // Director 오브젝트가 갖고 있는 PlayableDirector 컴포넌트를 가져온다.
        _playableDirector = GetComponent<PlayableDirector>();
        
        // 타임라인을 실행한다.
        _playableDirector.Play();
    }

    private void Update()
    {
        // 현재 진행중인 시간이 전체 시간보다 크거나 같으면 (재생시간이 다 되면)
        if (_playableDirector.time >= _playableDirector.duration)
        {
            // 만약 메인 카메라가 타깃 카메라(씨네머신에 활용하는 카메라)라면
            // 제어를 하기 위해 씨네머신 브레인을 비활성화하라.
            if (Camera.main == targetCam)
            {
                targetCam.GetComponent<CinemachineBrain>().enabled = false;
            }
            //씨네머신에 사용한 카메라도 비활성화하라.
            targetCam.gameObject.SetActive(false);
            
            // Director 자신을 비활성화하라.
            gameObject.SetActive(false);
        }
    }
}