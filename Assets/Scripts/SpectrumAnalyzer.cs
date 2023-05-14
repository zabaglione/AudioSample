using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpectrumAnalyzer : MonoBehaviour
{
    public enum LayoutType
    {
        Linear,
        Circular
    }

    public LayoutType layoutType = LayoutType.Linear;
    public int resolution = 1024;
    public float[] samples;

    public int numberOfCubes = 64;
    public Vector3 cubeScale = new Vector3(1, 1, 1);
    public float cubeSpacing = 0.5f;

    public float radius = 5f;
    public float lerpSpeed = 5f;

    public float peakHoldTime = 1f;
    private float[] peakHeights;
    private float[] peakHoldTimers;

    private GameObject[] cubes;
    private int[] cubeFrequencyIndices;

    private AudioSource audioSource;

    public delegate void FrequencyPeakCallback(int frequencyIndex, float peakValue);

    public event FrequencyPeakCallback OnFrequencyPeak;

    void Start()
    {
        // サンプリングデータの格納先
        audioSource = GetComponent<AudioSource>();
        samples = new float[resolution];

        // ピークの格納先
        peakHeights = new float[numberOfCubes];
        peakHoldTimers = new float[numberOfCubes];

        // Cubeのセットアップ
        SetupCube();
    }

    void SetupCube()
    {
        cubes = new GameObject[numberOfCubes];
        cubeFrequencyIndices = new int[numberOfCubes];

        for (int i = 0; i < numberOfCubes; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.SetParent(transform);

            if (layoutType == LayoutType.Linear)
            {
                cube.transform.position = new Vector3(i * (cubeScale.x + cubeSpacing), 0, 0);
            }
            else if (layoutType == LayoutType.Circular)
            {
                float angle = i * 360f / numberOfCubes;
                float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                float z = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
                cube.transform.position = new Vector3(x, 0, z);
            }

            cube.transform.localScale = cubeScale;
            cubes[i] = cube;

            // 線形スケールで各Cubeに周波数インデックスを割り当てます
            // 代わりに対数スケールを使いたい場合は、この計算方法を変更します
            cubeFrequencyIndices[i] = LogScaleIndex(i);
        }
    }

    /// <summary>
    /// 対数スケール変換用
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private int LogScaleIndex(int i)
    {
        float logMax = Mathf.Log(resolution, 2);
        float exp = (i * logMax) / numberOfCubes;
        int index = (int)Mathf.Pow(2, exp);
        return Mathf.Clamp(index, 0, resolution - 1);
    }

    void Update()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        //DisplaySpectrumData();

        // スペクトル解析の結果をCubeで表示
        UpdateCubes();
    }

    void UpdateCubes()
    {
        for (int i = 0; i < numberOfCubes; i++)
        {
            float cubeHeight = Mathf.Clamp(samples[cubeFrequencyIndices[i]] * 100, 0, 10);

            if (cubeHeight > peakHeights[i])
            {
                peakHeights[i] = cubeHeight;
                peakHoldTimers[i] = peakHoldTime;
            }
            else if (peakHoldTimers[i] > 0)
            {
                peakHoldTimers[i] -= Time.deltaTime;
            }
            else
            {
                peakHeights[i] = Mathf.Max(peakHeights[i] - Time.deltaTime * lerpSpeed, cubeHeight);
            }

            // スケールの調整
            float currentCubeHeight = cubes[i].transform.localScale.y;
            float targetCubeHeight = peakHeights[i];
            float newCubeHeight = Mathf.Lerp(currentCubeHeight, targetCubeHeight, lerpSpeed * Time.deltaTime);
            cubes[i].transform.localScale = new Vector3(cubeScale.x, newCubeHeight, cubeScale.z);

            // 現在のポジションからX座標とZ座標を取得し、新しいY座標と組み合わせてCubeの位置を更新します
            float x = cubes[i].transform.position.x;
            float z = cubes[i].transform.position.z;
            float y = newCubeHeight / 2;
            cubes[i].transform.position = new Vector3(x, y, z);

            // 色を高さに応じて変更します（低い = 青、高い = 赤）
            cubes[i].GetComponent<Renderer>().material.color = Color.Lerp(Color.blue, Color.red, peakHeights[i] / 10);

            // ピーク値が特定のしきい値を超えた場合、コールバックを実行します
            if (peakHeights[i] > 8)
            {
                OnFrequencyPeak?.Invoke(cubeFrequencyIndices[i], peakHeights[i]);
            }
        }
    }

    /// <summary>
    /// デバッグ用
    /// </summary>
    void DisplaySpectrumData()
    {
        string output = "";
        for (int i = 0; i < samples.Length; i++)
        {
            output += "[" + i + "]: " + samples[i] + "\n";
        }
        Debug.Log(output);
    }
}
