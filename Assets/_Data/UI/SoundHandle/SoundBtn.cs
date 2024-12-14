using UnityEngine;

public class SoundBtn : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    public void PlaySound()
    {
        if(audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }
}
