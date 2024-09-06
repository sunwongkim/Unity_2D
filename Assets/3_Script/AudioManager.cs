using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioClip jumpClip;
    public AudioClip coinClip;
    public AudioClip stompClip;
    public AudioClip damagedClip;
    public AudioClip deathClip;
    public AudioClip nextStageClip;
    public AudioClip clearClip;
    AudioSource audioSource;

    void Awake()
    {
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Start() => audioSource = GetComponent<AudioSource>();

    // 오디오 클립 재생 함수
    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }

    public void PlayJumpSound() => PlaySound(jumpClip);
    public void PlayCoinSound() => PlaySound(coinClip);
    public void PlayStompSound() => PlaySound(stompClip);
    public void PlayDamagedSound() => PlaySound(damagedClip);
    public void PlayDeathSound() => PlaySound(deathClip);
    public void PlayNextStageSound() => PlaySound(nextStageClip);
    public void PlayClearSound() => PlaySound(clearClip);
}
