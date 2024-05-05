using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MusicManager : MonoSingleton<MusicManager>
{
    [SerializeField] AudioSource BgmAudio;
    [SerializeField] AudioSource SfxAudio;

    public AudioClip bgmOpen;
    public AudioClip bgmBattle;
    public AudioClip bgmDiolog;
    public AudioClip bgmDeck;
    public AudioClip attack;
    public AudioClip draw;
    public AudioClip put;
    public AudioClip win;
    public AudioClip lose;
    public AudioClip open;
    public AudioClip click;
    private void Start()
    {
        BgmAudio.clip = SceneManager.GetActiveScene().buildIndex switch
        {
            0 => bgmDiolog,
            1 => bgmOpen,
            2 => bgmDeck,
            3 => bgmBattle,
            _ => null,
        };
        BgmAudio.Play();
    }

    public void Playsfx(AudioClip clip)
    {
        SfxAudio.PlayOneShot(clip);
    }
}   
