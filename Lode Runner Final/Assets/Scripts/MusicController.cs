using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Text _musicText;

    private string _prefsKeyMusicVolume = "MusicVolume";

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
            _musicSource.PlayDelayed(0);

        if(PlayerPrefs.HasKey(_prefsKeyMusicVolume) == false)
        {
            PlayerPrefs.SetInt(_prefsKeyMusicVolume, 3);
        }

        _musicSource.volume = (float)PlayerPrefs.GetInt(_prefsKeyMusicVolume) / 9;
        _musicSlider.value = PlayerPrefs.GetInt(_prefsKeyMusicVolume);
    }

    private void Update()
    {
        PlayerPrefs.SetInt(_prefsKeyMusicVolume, (int)_musicSlider.value);
        _musicSource.volume = (float)PlayerPrefs.GetInt(_prefsKeyMusicVolume) / 9;
        _musicText.text = _musicSlider.value.ToString();
    }


}
