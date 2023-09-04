using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public AudioSource[] soundEffects;
    public void PlayGemBreak()
    {
        soundEffects[0].Stop();

        soundEffects[0].pitch = Random.Range(.8f, 1.2f);

        soundEffects[0].Play();
    }
    public void PlayExplode()
    {
        soundEffects[1].Stop();

        soundEffects[1].pitch = Random.Range(.8f, 1.2f);

        soundEffects[1].Play();
    }
    public void PlayRoundOver()
    {    
        soundEffects[2].Play();
    }
    public void PlayStoneBreak()
    {
        soundEffects[3].Stop();

        soundEffects[3].pitch = Random.Range(.8f, 1.2f);

        soundEffects[3].Play();
    }
}
