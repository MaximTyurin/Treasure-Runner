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

    private string _prefsKeyLvl = "Lvl";
    private float _delayTimeLoadScene = 2f;
    private float _delayTimeDeactivation = 0.5f;

    public void LoadLastSaveScene()
    {
        if(PlayerPrefs.HasKey(_prefsKeyLvl))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt(_prefsKeyLvl) + 1);
        }
    }

    public void Restartlevel()
    {
        StartCoroutine(ReloadAfterDeath());
    }

    public void LoadNextlevel()
    {
        if (PlayerPrefs.HasKey(_prefsKeyLvl) == false || PlayerPrefs.GetInt(_prefsKeyLvl) < SceneManager.GetActiveScene().buildIndex)
        {
            PlayerPrefs.SetInt(_prefsKeyLvl, SceneManager.GetActiveScene().buildIndex);
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
        yield return new WaitForSeconds(_delayTimeLoadScene);
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
        yield return new WaitForSeconds(_delayTimeLoadScene);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return new WaitForSeconds(_delayTimeDeactivation);
        _nextScenePanel.SetActive(false);
    }

    private IEnumerator ReloadAfterDeath()
    {
        _audioSource.enabled = false;
        _deathScreen.SetActive(true);
        yield return new WaitForSeconds(_delayTimeLoadScene);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return new WaitForSeconds(_delayTimeDeactivation);
        _deathScreen.SetActive(false);
    }
}
