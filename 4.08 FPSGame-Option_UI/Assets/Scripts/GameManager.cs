using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 싱글턴 변수
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // 게임 상태 상수
    public enum GameState
    {
        Ready,
        Run,
        Pause,
        GameOver
    }
    
    // 현재의 게임 상태 변수
    public GameState currentGameState;
    
    // 게임 상태 UI 오브젝트 변수
    public GameObject gameLabel;
    
    // 게임 상태 UI 텍스트 컴포넌트 변수
    private Text _gameText;
    
    // PlayerMove 클래스 변수
    private PlayerMove _player;

    // 옵션 화면 UI 오브젝트 변수
    public GameObject gameOption;
    
    private void Start()
    {
        // 초기 게임 상태는 준비 상태로 설정한다.
        currentGameState = GameState.Ready;
        
        // 게임 상태 UI 오브젝트에서 Text 컴포넌트를 가져온다.
        _gameText = gameLabel.GetComponent<Text>();
        
        // 상태 텍스트의 내용을 'Ready...'로 한다.
        _gameText.text = "Ready...";
        
        // 상태 텍스트의 색상을 주황색으로 한다.
        _gameText.color = new Color32(255, 185, 0, 255);
        
        // 게임 준비 -> 게임 중 상태로 전환하기
        StartCoroutine(ReadyToStart());
        
        // 플레이어 오브젝트를 찾은 후 플레이어의 PlayerMove 컴포넌트 받아오기
        _player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    private IEnumerator ReadyToStart()
    {
        // 2초간 대기한다.
        yield return new WaitForSeconds(2f);
        
        // 상태 텍스트의 내용을 'Go!'로 한다.
        _gameText.text = "Go!";
        
        // 0.5초간 대기한다.
        yield return new WaitForSeconds(0.5f);
        
        // 상태 텍스트를 비활성화한다.
        gameLabel.SetActive(false);
        
        // 상태를 '게임 중' 상태로 변경한다.
        currentGameState = GameState.Run;
    }

    private void Update()
    {
        // 만일, 플레이어의 hp가 0 이하라면
        if (_player.Hp <= 0)
        {
            // 플레이어의 애니메이션을 멈춘다.
            _player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);
            
            // 상태 텍스트를 활성화한다.
            gameLabel.SetActive(true);
            
            // 상태 텍스트의 내용을 'Game Over'로 한다.
            _gameText.text = "Game Over";
            
            // 상태 텍스트의 색상을 붉은색으로 한다.
            _gameText.color = new Color32(255, 0, 0, 255);
            
            // 상태 텍스트의 자식 오브젝트의 트랜스폼 컴포넌트를 가져온다.
            var buttons = _gameText.transform.GetChild(0);
            
            // 버튼 오브젝트를 활성화한다.
            buttons.gameObject.SetActive(true);
            
            // 상태를 '게임 오버' 상태로 변경한다.
            currentGameState = GameState.GameOver;
        }
    }
    
    // 옵션 화면 켜기
    public void OpenOptionWindow()
    {
        // 옵션 창을 활성화한다.
        gameOption.SetActive(true);
        // 게임 속도를 0배속으로 전환한다.
        Time.timeScale = 0f;
        // 게임 상태를 일시 정지 상태로 변경한다.
        currentGameState = GameState.Pause;
    }
    
    // 계속하기 옵션
    public void CloseOptionWindow()
    {
        // 옵션 창을 비활성화한다.
        gameOption.SetActive(false);
        // 게임 속도를 1배속으로 전환한다.
        Time.timeScale = 1f;
        // 게임 상태를 게임 중 상태로 변경한다.
        currentGameState = GameState.Run;
    }
    
    // 다시하기 옵션
    public void RestartGame()
    {
        // 게임 속도를 1배속으로 전환한다.
        Time.timeScale = 1f;
        // 현재 씬 번호를 다시 로드한다.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    // 게임 종료 옵션
    public void QuitGame()
    {
        // 애플리케이션을 종료한다.
        Application.Quit();
    }
}