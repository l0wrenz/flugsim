using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource music;
    // public AudioSource plane_sound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void LowVolume() {
    //     plane_sound.volume = 0.01f;
    // }

    // public void HighVolume() {
    //     plane_sound.volume = 0.4f;
    // }

    public void ChangeMusic(AudioClip newMusic) {
        music.Stop();
        music.clip = newMusic;
        music.Play();
    }
}
