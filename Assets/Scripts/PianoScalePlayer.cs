using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PianoScalePlayer : MonoBehaviour
{
    public float baseFrequency = 440f; // A4
    public int numberOfKeys = 88; // ピアノの鍵盤数
    public float[] frequencies;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        frequencies = new float[numberOfKeys];
        for (int i = 0; i < numberOfKeys; i++)
        {
            frequencies[i] = baseFrequency * Mathf.Pow(2, (i - 48) / 12f); // 12平均律で各音の周波数を計算
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayNote(40); // C4
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayNote(41); // C#4
        }
        // 他のキー入力と音階の対応を追加
    }

    void PlayNote(int index)
    {
        if (index < 0 || index >= numberOfKeys) return;

        float frequency = frequencies[index];
        AudioClip note = GenerateSineWave(frequency);
        audioSource.PlayOneShot(note);
    }

    AudioClip GenerateSineWave(float frequency)
    {
        int sampleRate = 44100;
        int sampleLength = sampleRate / 5;
        float[] samples = new float[sampleLength];

        for (int i = 0; i < sampleLength; i++)
        {
            samples[i] = Mathf.Sin(2 * Mathf.PI * frequency * i / sampleRate);
        }

        AudioClip clip = AudioClip.Create("SineWave", sampleLength, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}
