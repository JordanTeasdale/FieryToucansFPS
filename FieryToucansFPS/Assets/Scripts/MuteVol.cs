using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MuteVol : MonoBehaviour
{
    [SerializeField] AudioMixer mainMixer;

    void Update()
    {
        mainMixer.GetFloat("MasterVol", out float masterVol);
        mainMixer.GetFloat("MusicVol", out float musicVol); 
        mainMixer.GetFloat("SFXVol", out float SFXVol);

        if (masterVol == -41f)
            mainMixer.SetFloat("MasterVol", -80f);
        if (musicVol == -41f)
            mainMixer.SetFloat("MusicVol", -80f);
        if (SFXVol == -41f)
            mainMixer.SetFloat("SFXVol", -80f);

    }
}
