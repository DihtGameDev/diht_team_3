using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [System.Serializable]
    public class SoundClip {

        public string name;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume;
        [Range(.1f, 3f)]
        public float pitch;

        [HideInInspector]
        public float default_volume;

        public float relevant_volume;

        [HideInInspector]
        public AudioSource source;

        public bool loop;

        public enum Type { 
            effects, music, interFace
        }
        [SerializeField]
        public Type type;


    }

    public bool isOldGameObject = false;
    public int pointer;
    public SoundClip[] sounds;

    public static AudioController instance;
    // Start is called before the first frame update

    private bool isPlayingBackTrack = false;
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            Global.audioController = this;
            isOldGameObject = false;
            pointer = SceneManager.GetActiveScene().buildIndex;
        }
        else
        {
            instance.isOldGameObject = true;
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;

            sound.default_volume = sound.volume;
            sound.relevant_volume = sound.volume;


            sound.source.loop = sound.loop;
            sound.source.playOnAwake = false;
        }

        
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/Room.unity") 
            && SceneManager.GetActiveScene().buildIndex < SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/Hospital5.unity") && !isPlayingBackTrack)
        {
            isPlayingBackTrack = true;
            StartCoroutine(Play("HospitalTrack", 2.5f, 3.5f));
        }

        if ((SceneManager.GetActiveScene().buildIndex <= SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/Room.unity") 
            || SceneManager.GetActiveScene().buildIndex >= SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/Hospital5.unity")) && isPlayingBackTrack)
        {
            isPlayingBackTrack = false;
        }
    }

    public IEnumerator Play(string name, float fading_time = 0f, float offset = 0f)
    {

        SoundClip sound = findSoundClip(name);
        if (sound != null)
        {
            yield return new WaitForSecondsRealtime(offset);
            sound.source.Play();
            if (fading_time != 0f)
            {
                StartCoroutine(startPlaing(sound, fading_time));
            }
        }
        yield return null;
    }

    public IEnumerator PlayWithDuration(string name, float duration, float fading_time = 0f, float offset = 0f)
    {
        SoundClip sound = findSoundClip(name);
        if (sound != null)
        {
            yield return new WaitForSecondsRealtime(offset);
            sound.source.Play();

            if (fading_time != 0f)
            {
                StartCoroutine(startPlaing(sound, fading_time));
            }
            yield return new WaitForSecondsRealtime(duration - fading_time);

            if (fading_time != 0f)
            {
                StartCoroutine(stopPlaing(sound, fading_time));
            }
            sound.source.Stop();
        }
    }

    public IEnumerator PlayWithPeriod(string name, float period, bool scaled, float fading_time = 0f, float offset = 0f)
    {

        SoundClip sound = findSoundClip(name);
        if (sound != null)
        {

            yield return new WaitForSecondsRealtime(offset);
            if (fading_time != 0f)
            {

                StartCoroutine(startPlaing(sound, fading_time));
                yield return new WaitForSecondsRealtime(fading_time);
            }
            while (true) { 
                sound.source.Play();
                if (scaled)
                {
                
                    yield return new WaitForSeconds(period);

                }
                else {
                    if (Time.timeScale == 0)
                    {
                        yield return new WaitForSeconds(period);
                    }
                    else
                    {
                        yield return new WaitForSecondsRealtime(period);
                    }
                
                }
                sound.source.Stop();
            }
        }
      
        yield return null;
    }

    public IEnumerator Stop(string name, float fading_time = 0f, float offset = 0f)
    {

        SoundClip sound = findSoundClip(name);
        if (sound != null)
        {
            yield return new WaitForSecondsRealtime(offset);
            if (fading_time != 0f)
            {
                StartCoroutine(stopPlaing(sound, fading_time));
                yield return new WaitForSecondsRealtime(fading_time);
            }
            sound.source.Stop();

        }
        yield return null;

    }

    
    public IEnumerator startPlaing(SoundClip sound, float fading_time)
    {
     
        float target_volume = sound.relevant_volume;

        sound.volume = 0f;
        sound.source.volume = sound.volume;
        for (int i = 0; i < 50; i++)
        {
            sound.volume += target_volume / 50f;
            sound.source.volume = sound.volume;
            yield return new WaitForSecondsRealtime(fading_time / 50f);
        }

    }
    public IEnumerator stopPlaing(SoundClip sound, float fading_time)
    {

        float start_volume = sound.volume;

        for (int i = 0; i < 50; i++)
        {
            sound.volume -= start_volume / 50f;
            sound.source.volume = sound.volume;
            yield return new WaitForSecondsRealtime(fading_time / 50f);
        }

    }


    public IEnumerator ChangeVolume(string name, float target_volume, float fading_time = 0f)
    {
        SoundClip sound = findSoundClip(name);

        if (fading_time != 0)
        {
            float start_volume = sound.volume;
            for (int i = 0; i < 50; i++)
            {
                sound.volume += (target_volume - start_volume) / 50f;
                sound.source.volume = sound.volume;
                yield return new WaitForSecondsRealtime(fading_time / 50f);
            }
        }
        else {
            sound.volume = target_volume;
            sound.source.volume = sound.volume;
            yield return null;
        }   
    }
    public IEnumerator ChangeVolumeLocal(string name, float local_volume)
    {
        SoundClip sound = findSoundClip(name);

        if (sound != null)
        {
            sound.relevant_volume = local_volume * sound.default_volume;
            sound.volume = sound.relevant_volume;
            sound.source.volume = sound.volume;
            yield return null;
        }
    }
    public IEnumerator ChangeVolumeLocal(SoundClip sound, float local_volume)
    {

        if (sound != null)
        {
            sound.relevant_volume = local_volume * sound.default_volume;
            sound.volume = sound.relevant_volume;
            sound.source.volume = sound.volume;
            yield return null;
        }
    }



    public float GetClipVolume(string name) {
        SoundClip sound = findSoundClip(name);
        if (sound != null)
        {
            return sound.source.volume;
        }
        return 0;
    }

    public float GetClipRelevantVolume(string name)
    {
        SoundClip sound = findSoundClip(name);
        if (sound != null)
        {
            return sound.relevant_volume;
        }
        return 0;
    }


    public SoundClip findSoundClip(string findName)
    {
        foreach (var obj in sounds)
        {
            if (obj.name == findName)
            {
                return obj;
            }
        }
        return null;

        
    }

    
    public IEnumerator turnOffSound(int buildIndex)
    {
        Debug.Log(buildIndex);
        
        if (buildIndex <= SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/Room.unity") ||
            buildIndex >= SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/Hospital5.unity"))
        {
            StartCoroutine(Stop("HospitalTrack"));
            StartCoroutine(Stop("Tension"));

        }
        if (buildIndex != SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/Room.unity"))
        {
            StartCoroutine(Stop("RoomAnxiety"));
            StartCoroutine(Stop("NeonLamp"));
            StartCoroutine(Stop("BackGround"));
        }
        if (buildIndex != SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/MainMenu.unity")) {
            StartCoroutine(Stop("MenuNeon"));
            StartCoroutine(Stop("BackGround"));
        }
        yield return null;
    }

    public void changeVolumeSettings(string soundType, float value) {
        switch (soundType) {
            case "effects":
                foreach (var sound in sounds) {
                    if (sound.type == SoundClip.Type.effects)
                    {
                        StartCoroutine(ChangeVolumeLocal(sound, value));
                    }
                }
                break;
            case "music":
                foreach (var sound in sounds)
                {
                    if (sound.type == SoundClip.Type.music)
                    {
                        StartCoroutine(ChangeVolumeLocal(sound, value));
                    }
                   
                }
                break;
            case "interface":
                foreach (var sound in sounds)
                {
                    if (sound.type == SoundClip.Type.interFace)
                    {
                        StartCoroutine(ChangeVolumeLocal(sound, value));
                    }
             
                }
                break;
        }        
    }

    public void StartChasingSound(float fading = 3f) {
        StartCoroutine(ChangeVolume("HospitalTrack", GetClipRelevantVolume("HospitalTrack") * 0.1f, fading));
        StartCoroutine(Play("Tension", fading));
    }

    public void StopChasingSound(float fading = 4f) {
        StartCoroutine(Stop("Tension", fading));
        StartCoroutine(ChangeVolume("HospitalTrack", GetClipRelevantVolume("HospitalTrack"), fading));

    }
}
