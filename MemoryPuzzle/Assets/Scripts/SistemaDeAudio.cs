using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SistemaDeAudio : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private List<AudioSource> _audioSources;

    // Start is called before the first frame update
    void Start()
    {
        // Acessa O Valor Salvo De Volume, Se Não Tiver Usa 1.0f
        float volume = PlayerPrefs.GetFloat("Volume", 1.0f);

        // Seta Volume
        _volumeSlider.value = volume;
        SetVolume(volume);
    }

    public void SetVolume(float volume)
    {
        // Muda Volume De Todos As Fontes De Som
        foreach (AudioSource audioSource in _audioSources)
            audioSource.volume = volume;

        // Sava O Volume
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }
}
