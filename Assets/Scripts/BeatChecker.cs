using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeatChecker: MonoBehaviour
{
    [SerializeField]
    private GameObject beat;

    private Vector3 scale1 = new Vector3(1.5f, 1.5f, 1.5f);

    private Sequence seq1;

    void Start()
    {
        Material mat = beat.GetComponent<MeshRenderer>().material;
        Color org = mat.color;
        seq1 = DOTween.Sequence();
        seq1.Join(beat.transform.DOScale(scale1, 60/140f/4).From())
            .Join(mat.DOColor(Color.red, 60/140f/4).From())
            // 自動的にkillされないようにするための指定
            .Pause()
            .SetAutoKill(false)
            .SetLink(beat);
    }

    // Update is called once per frame
    void Update()
    {
        if (Music.IsJustChangedBeat())
        {
            seq1.Restart();
        }    
    }
}
