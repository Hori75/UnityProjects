using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip JumpStart;
    public AudioClip JumpEnd;

    AudioSource AudioOutput;

    void Start()
    {
        AudioOutput = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio (string type)
    {
        switch(type)
        {
            case "Jump In":
                AudioOutput.clip = JumpStart;
                break;
            case "Jump Out":
                AudioOutput.clip = JumpEnd;
                break;
            default:
                return;
        }

        AudioOutput.Play();
    }
}
