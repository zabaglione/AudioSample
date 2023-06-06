using UnityEngine;

public class SEManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] seClips;
    [SerializeField] private AudioSource audioSource;

    public int SECount() {
        return seClips.Length;
    }
    
    public void PlaySE(int index) {
        PlaySE(index, 1f);
    }

    public void PlaySE(int index, float volume) {
        PlaySE(index, volume, 1f);
    }

    public void PlaySE(int index, float volume, float pitch) {
        PlaySE(index, volume, pitch, 0f);
    }

    public void PlaySE(int index, float volume, float pitch, float panStereo) {
        audioSource.panStereo = panStereo;
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(seClips[index], volume);
        audioSource.pitch = 1f;
        audioSource.panStereo = 0f;
    }
}
