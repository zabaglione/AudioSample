using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDrop : MonoBehaviour
{
    [SerializeField] private GameObject _dropPrefab;

    [SerializeField] private float _radius;

    [SerializeField] float _lifeTime = 15f;

    [SerializeField] float _intervalTime = 1f;

    IEnumerator Start()
    {
        WaitForSeconds intervalTime = new WaitForSeconds(1f);
        while(true) {
            // 指定した半径の中にプレハブを生成して落とす
            Vector3 position = transform.position + Random.insideUnitSphere * _radius;
            var go = Instantiate(_dropPrefab, position, Quaternion.identity);
            var rain = go.GetComponent<Rain>();
            rain.lifeTime = _lifeTime;

            // 生成したプレハブを親に設定
            go.transform.SetParent(transform);
            // 生成したプレハブを1秒後に削除
            Destroy(go, _lifeTime);
            // 1秒待つ
            yield return intervalTime;
        }
    }
}
