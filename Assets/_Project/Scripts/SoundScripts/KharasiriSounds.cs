using UnityEngine;

public class KharasiriSounds : MonoBehaviour
{
    [Header("Growl configuration")]
    public float minTimeBetweenGrowls = 5f;
    public float maxTimeBetweenGrowls = 15f;

    public float minVolumeGrowl = 0.8f;
    public float maxVolumeGrowl = 1.8f;
    
    [Header("Step configuration")]
    public float stepVolume = 1f;
    
    private float _growlTimer = 0f;
    private float _currentInterval = 0f;
    
    private AudioClip[] _growlClips;
    private AudioSource _audioSource;
    private bool bIsUpdating = false;
    

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        
        _growlClips  = Resources.LoadAll<AudioClip>("Sounds/Kharisiri_Growls");
    }

    public void StartGrowling()
    {
        ScheduleNextGrowl();
        bIsUpdating = true;
    }

    void Update()
    {
        if (!bIsUpdating) return;
        _growlTimer += Time.deltaTime;
        if (_growlTimer >= _currentInterval && !_audioSource.isPlaying)
        {
            PlayGrowl();
            ScheduleNextGrowl();
        }
    }

    void PlayGrowl()
    {
        int index = Random.Range(0, _growlClips.Length);
        float volume = Random.Range(minVolumeGrowl, maxVolumeGrowl);
        _audioSource.PlayOneShot(_growlClips[index], volume);
    }

    void ScheduleNextGrowl()
    {
        _growlTimer = 0f;
        _currentInterval = Random.Range(minTimeBetweenGrowls, maxTimeBetweenGrowls);
    }

}
