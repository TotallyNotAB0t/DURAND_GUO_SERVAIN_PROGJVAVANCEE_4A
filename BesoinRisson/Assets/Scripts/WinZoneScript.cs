using TMPro;
using UnityEngine;

public class WinZoneScript : MonoBehaviour
{
    [SerializeField] private GameObject _playerConcerned;
    [SerializeField] private GameObject _uiWinScreen;
    [SerializeField] private TextMeshProUGUI _winnerText;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.Equals(_playerConcerned.GetComponent<Collider2D>())) 
        {
            _winnerText.text = $"{_playerConcerned.name} won!";
            _uiWinScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
