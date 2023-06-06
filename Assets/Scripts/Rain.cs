using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Rain : MonoBehaviour
{
    private SEManager _seManager;

    [SerializeField] private float _pitchRange = 0.1f;

    [SerializeField] private Color _hitColor;

    private float _lifeTime = 15f;
    public float lifeTime
    {
        set { _lifeTime = value; }
    }

    private Renderer _rederer;
    private Color _originalColor;

    private Sequence _seq1;

    private void Start()
    {
        DOTween.defaultAutoKill = true;
        _seManager = GameObject.Find("SEManager").GetComponent<SEManager>();
        _rederer = GetComponent<Renderer>();

        _originalColor = _rederer.material.color;

        // 徐々に小さくする
        transform.DOScale(0f, _lifeTime).SetLink(gameObject);
    }

    private void OnDisable() 
    {
        if (_seq1 != null && _seq1.IsActive() && _seq1.IsPlaying()) {
            _seq1.Complete();
            _seq1.Kill();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground") 
        {
            var se = Random.Range(0, _seManager.SECount());
            var volume = Random.Range(0.5f, 1f);
            var pitch = 1 + Random.Range(-_pitchRange, _pitchRange);
            _seManager.PlaySE(se, volume, pitch);
            if (_seq1 != null && _seq1.IsActive() && _seq1.IsPlaying()) {
                _seq1.Complete();
                _seq1.Kill();
            }
            _seq1 = DOTween.Sequence();
            _seq1.OnStart(() => _rederer.material.color = _hitColor)
                .Insert(0f, _rederer.material.DOColor(_originalColor, 0.3f))
                .SetLink(gameObject)
                ;
        }
    }
}
