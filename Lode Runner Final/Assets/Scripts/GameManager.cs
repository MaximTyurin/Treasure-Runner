using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _ladder;
    [SerializeField] private GameObject _pauseScreen;

    public void CreateFinishLadder()
    {
        _ladder.SetActive(true);
    }

    public void PauseOn()
    {
        Time.timeScale = 0f;
        _pauseScreen.SetActive(true);
    }

    public void PauseOff()
    {
        _pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }
}
