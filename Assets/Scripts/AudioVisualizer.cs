using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVisualizer : MonoBehaviour
{
    public int numSamples = 1024;
    public float maxMagnitude = 100.0f;
    public LineRenderer lineRenderer;

    private AudioSource audioSource;
    private float[] spectrumData;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spectrumData = new float[numSamples];
    }

    void Update()
    {
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

        lineRenderer.positionCount = numSamples;
        for (int i = 0; i < numSamples; i++)
        {
            Vector3 pos = new Vector3(i, spectrumData[i] * maxMagnitude, 0.0f);
            lineRenderer.SetPosition(i, pos);
        }
    }
}
