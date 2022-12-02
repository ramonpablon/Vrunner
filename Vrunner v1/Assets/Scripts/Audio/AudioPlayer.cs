using UnityEngine;

public class AudioPlayer : MonoBehaviour
{

    public static AudioPlayer Instance { get; set; }

    public AudioSource[] ads;
    public AudioClip[] adc;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ads = GetComponents<AudioSource>();
    }
    private void Update()
    {
        if (GameManager.Instance.active == true)
            ads[2].pitch = 1.5f;
        else
            ads[2].pitch = 1;
    }

    public void PowerUpSoundOneShot(int powerUp)
    {
        ads[powerUp].PlayOneShot(adc[powerUp]);
    }
}
