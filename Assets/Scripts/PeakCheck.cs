using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeakCheck : MonoBehaviour
{
    [SerializeField]
    SpectrumAnalyzer spectrumAnalyzer;
    void Start()
    {
        // イベントに関数を登録します
        spectrumAnalyzer.OnFrequencyPeak += LogFrequencyPeak;
    }

    void LogFrequencyPeak(int frequencyIndex, float peakValue)
    {
        Debug.Log($"Frequency index: {frequencyIndex}, Peak value: {peakValue}");
    }
}
