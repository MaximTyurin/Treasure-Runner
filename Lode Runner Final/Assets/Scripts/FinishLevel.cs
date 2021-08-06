using UnityEngine.SceneManagement;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    [SerializeField] private SceneController _sceneController;

    private int _lastSceneIndex = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player) && SceneManager.GetActiveScene().buildIndex != _lastSceneIndex)
        {
            _sceneController.LoadNextlevel();
        }
        else if(SceneManager.GetActiveScene().buildIndex == _lastSceneIndex)
        {
            Debug.Log($"Game Over {_lastSceneIndex}");
            _sceneController.StartCoroutine(_sceneController.Win());
        }
    }
}
