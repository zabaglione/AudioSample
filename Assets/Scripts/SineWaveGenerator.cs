using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SineWaveGenerator : MonoBehaviour
{
    public float[] frequencies = new float[] { 440f, 554.37f }; // 任意の周波数を設定
    public float sampleRate = 44100; // サンプルレート
    public float amplitude = 0.5f;   // 振幅

    private float increment;
    private float phase;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0;
        audioSource.loop = true;
        audioSource.volume = amplitude;

        AudioClip audioClip = AudioClip.Create("SineWave", (int)(sampleRate * 2), 1, (int)sampleRate, true, OnAudioRead);
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    void OnAudioRead(float[] data)
    {
        float[] increments = new float[frequencies.Length];
        for (int i = 0; i < frequencies.Length; i++)
        {
            increments[i] = frequencies[i] * 2 * Mathf.PI / sampleRate;
        }

        for (int i = 0; i < data.Length; i++)
        {
            float sum = 0;
            for (int j = 0; j < frequencies.Length; j++)
            {
                sum += Mathf.Sin(phase) * amplitude;
                phase += increments[j];
                if (phase > 2 * Mathf.PI) phase -= 2 * Mathf.PI;
            }
            data[i] = sum / frequencies.Length;
        }
    }
}
