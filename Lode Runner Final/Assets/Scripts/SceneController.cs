using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Text _nextLvlText;
    [SerializeField] private GameObject _nextScenePanel;
    [SerializeField] private GameObject _deathScreen;
    [SerializeField] private GameObject _winScreen;

    private AudioSource _audioSource;

    public void LoadLastSaveScene()
    {
        if(PlayerPrefs.HasKey("Lvl"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("Lvl") + 1);
        }
    }

    public void Restartlevel()
    {
        StartCoroutine(ReloadAfterDeath());
    }

    public void LoadNextlevel()
    {
        if (PlayerPrefs.HasKey("Lvl") == false || PlayerPrefs.GetInt("Lvl") < SceneManager.GetActiveScene().buildIndex)
        {
            PlayerPrefs.SetInt("Lvl", SceneManager.GetActiveScene().buildIndex);
            Debug.Log(PlayerPrefs.GetInt("Lvl"));
        }

        StartCoroutine(LoadLvl());
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DeleteAllKeys()
    {
        PlayerPrefs.DeleteAll();
    }

    public IEnumerator Win()
    {
        _winScreen.SetActive(true);
        yield return new WaitForSeconds(2f);
        LoadMainMenu();
    }

    private void Start()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    private IEnumerator LoadLvl()
    {
        _audioSource.enabled = false;
        _nextScenePanel.SetActive(true);
        _nextLvlText.text = $"Level {SceneManager.GetActiveScene().buildIndex + 1}";
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return new WaitForSeconds(0.5f);
        _nextScenePanel.SetActive(false);
    }

    private IEnumerator ReloadAfterDeath()
    {
        _audioSource.enabled = false;
        _deathScreen.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return new WaitForSeconds(0.5f);
        _deathScreen.SetActive(false);
    }
}
