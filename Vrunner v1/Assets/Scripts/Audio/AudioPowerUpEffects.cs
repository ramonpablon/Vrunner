using UnityEngine;

public class AudioPowerUpEffects : MonoBehaviour {

    public static AudioPowerUpEffects Instance { get; set; }

    public AudioSource[] ads;
    public AudioClip[] adc;
	// Use this for initialization
	void Awake () {
        Instance = this;
	}
    private void Start()
    {
        ads = GetComponents<AudioSource>();
    }

    public void PowerUpSoundOneShot(int powerUp)
    {
        ads[powerUp].PlayOneShot(adc[powerUp]);
    }

    public void PowerUpSoundLoop(int powerUp)
    {
        ads[powerUp].Play();
    }
    public void PowerUpSoundStop(int powerUp)
    {
        ads[powerUp].Stop();
    }
}
