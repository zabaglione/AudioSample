using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ScaleWithMicVolume : MonoBehaviour
{
    private AudioSource audioSource;
    public float sensitivity = 50;
    public bool isSmooth = true;
    // 音量
    private float _loudness;
    public float Loundness {
        get { return _loudness; }
    }

    // バッファサイズ
    private readonly int BUFFER_SIZE = 256;

    private void Start()
    {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = Microphone.Start(null, true, 1, 44100);
            audioSource.loop = true;
           //マイクデバイスの準備ができるまで待つ
            while (!(Microphone.GetPosition("") > 0)) { }
             audioSource.Play(); // オーディオを再生
    }

    private void Update()
    {
        _loudness = GetAveragedVolume();
        float viewScale = _loudness * sensitivity;
        var targetScale = new Vector3(viewScale, viewScale, viewScale);
        if (isSmooth) {
            // スムーズに拡大縮小
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 3);
        } else {
            transform.localScale = targetScale;
        }
    }

    float GetAveragedVolume()
    {
        float[] data = new float[BUFFER_SIZE];
        float a = 0;
        // マイクの音量を取得
        audioSource.GetOutputData(data, 0);

         //バッファ内の平均振幅を取得（絶対値を平均する）
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a / BUFFER_SIZE;
    }
}
