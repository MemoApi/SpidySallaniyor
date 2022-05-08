using UnityEngine;
using System;


public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip grappleSound, BackSound;
    
    
   
    private static AudioSource backSource, effectSource;

    public Sound[] sounds;

    public static SoundManagerScript instance;

    private void Awake()
    {

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
       

        foreach(Sound s in sounds)
        {

            s.source = gameObject.AddComponent<AudioSource>();
            if (s.name == "Back")
            {
                backSource = s.source;
                s.source.volume = PlayerPrefs.GetFloat("BackVol",.7f);

            }
            if (s.name == "Grapple") 
            {
                effectSource = s.source;
                s.source.volume = PlayerPrefs.GetFloat("EffectVol",1);
                
            }
            s.source.clip = s.clip;
            s.source.loop = s.loop;

        }
    }


    void Start()
    {     
        play("Back");       
    }

    public void backVolume(float vol)
    {       
            backSource.volume = vol;
            PlayerPrefs.SetFloat("BackVol", vol);            
    }
    public void effectVolume(float vol)
    {
        effectSource.volume = vol;
        PlayerPrefs.SetFloat("EffectVol", vol);       

    }

    public void play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }


}
