using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Text _musicText;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
            _musicSource.PlayDelayed(0);

        if(PlayerPrefs.HasKey("MusicVolume") == false)
        {
            PlayerPrefs.SetInt("MusicVolume", 3);
        }

        _musicSource.volume = (float)PlayerPrefs.GetInt("MusicVolume") / 9;
        _musicSlider.value = PlayerPrefs.GetInt("MusicVolume");
    }

    private void Update()
    {
        PlayerPrefs.SetInt("MusicVolume", (int)_musicSlider.value);
        _musicSource.volume = (float)PlayerPrefs.GetInt("MusicVolume") / 9;
        _musicText.text = _musicSlider.value.ToString();
    }


}
