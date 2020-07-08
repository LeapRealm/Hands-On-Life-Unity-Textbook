using UnityEngine;
// 유니티 UI를 사용하기 위한 네임스페이스
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    // 필요 속성: 점수 UI, 현재 점수, 최고 점수
    // 현재 점수 UI
    public Text currentScoreUI;
    // 현재 점수
    private int _currentScore;
    // 최고 점수 UI
    public Text bestScoreUI;
    // 최고 점수
    private int _bestScore;

    private void Start()
    {
        // 목표: 최고 점수를 불러와 bestScore 변수에 할당하고 화면에 표시한다.
        // 순서: 1. 최고 점수를 불러와 bestScore에 넣어주기
        _bestScore = PlayerPrefs.GetInt("Best Score", 0);
        // 2. 최고 점수를 화면에 표시하기
        bestScoreUI.text = "최고 점수 : " + _bestScore;
    }
    
    // currentScore에 값을 넣고 화면에 표시하기
    public void SetScore(int value)
    {
        // 3. ScoreManager 클래스의 속성에 값을 할당한다.
        _currentScore = value;
        // 4. 화면에 현재 점수 표시하기
        currentScoreUI.text = "현재 점수 : " + _currentScore;

        // 목표: 최고 점수를 표시하고 싶다.
        // 1. 현재 점수가 최고 점수보다 크니까
        // -> 만약 현재 점수가 최고 점수를 초과했다면
        if (_currentScore > _bestScore)
        {
            // 2. 최고 점수를 갱신시킨다.
            _bestScore = _currentScore;
            // 3. 최고 점수 UI에 표시
            bestScoreUI.text = "최고 점수 : " + _bestScore;
            // 목표: 최고 점수를 저장하고 싶다.
            PlayerPrefs.SetInt("Best Score", _bestScore);
        }
    }
    // currentScore 값 가져오기
    public int GetScore()
    {
        return _currentScore;
    }
}