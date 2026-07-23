using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioHandler : MonoBehaviour
{
    public static PlayAudioHandler Instance;
    private AudioSource[] audios;
    private Dictionary<string, int> lastRandomAudioPlayed = new Dictionary<string, int>();
    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audios = GetComponents<AudioSource>();
        string audiosFound = "PlayAudioHandler audio loaded:\n";
        foreach (AudioSource audio in audios)
        {
            audiosFound += audio.clip.name + "\n";
        }
        Debug.Log(audiosFound);
    }

    public AudioSource findAudioClip(string name)
    {
        foreach (AudioSource source in audios)
        {
            if (source.clip.name == name) return source;
        }
        return null;
    }

    public AudioSource[] findMultipleAudioClips(string name, int howManyAudios)
    {
        AudioSource[] audioSources = new AudioSource[howManyAudios];
        int audiosFound = 0;
        foreach (AudioSource source in audios)
        {
            if (source.clip.name.Contains(name))
            {
                audioSources[audiosFound++] = source;
            }
        }
        if(audiosFound != howManyAudios)
        {
            Debug.Log("Could not find all audios: " + name + "\nFound these many: " + audiosFound);
        }
        return audioSources;
    }

    public void playAudioSource(string name)
    {
        AudioSource a = findAudioClip(name);
        if (a != null)
        {
            a.Play();
            return;
        }
        Debug.Log("Failure finding audio " + name);
    }

    public void playRandomAudio(string name, int howManyAudios)
    {
        AudioSource[] randomAudioBank = findMultipleAudioClips(name, howManyAudios);
        int soundToPlay = Random.Range(0, randomAudioBank.Length);

        if (lastRandomAudioPlayed.ContainsKey(name))
        {
            if (soundToPlay == lastRandomAudioPlayed[name])
            {
                if (soundToPlay == (howManyAudios - 1)) soundToPlay = 0;
                else soundToPlay++;
            }
        }
        lastRandomAudioPlayed[name] = soundToPlay;

        randomAudioBank[soundToPlay].Play();
    }

    public void stopAudioSource(string name)
    {
        AudioSource a = findAudioClip(name);
        if (a != null)
        {
            a.Stop();
            return;
        }
        Debug.Log("Failure finding audio " + name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
