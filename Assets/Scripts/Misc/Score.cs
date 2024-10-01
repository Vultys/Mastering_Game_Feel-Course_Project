using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _currentScore = 0;

    private void OnEnable() 
    {
        Health.OnDeath += ScoreUp;
    }

    private void OnDisable()
    {
        Health.OnDeath -= ScoreUp;
    }

    private void ScoreUp(Health sender)
    {
        if(sender.GetComponent<PlayerController>() != null) return;
        _currentScore++;
        _scoreText.text = _currentScore.ToString("D3");
    }
}
