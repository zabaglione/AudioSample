using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VolumeView : MonoBehaviour
{
    [SerializeField]
    private ScaleWithMicVolume scaleWithMicVolume;

    [SerializeField]
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //振幅をdB（デシベル）に変換
        float dB = 20.0f * Mathf.Log10(scaleWithMicVolume.Loundness);
        text.text = dB.ToString("0.00") + " dB";
    }
}
