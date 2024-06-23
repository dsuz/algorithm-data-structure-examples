using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// あ
public class MonteCarlo : MonoBehaviour
{
    [SerializeField] float _interval = 1;
    [SerializeField] int _hordeCount = 100;
    [SerializeField] Sprite _dotSprite;
    [SerializeField] Color _inside = Color.white;
    [SerializeField] Color _outside = Color.red;
    [SerializeField] Text _sampleCountText;
    [SerializeField] Text _approximateValueText;
    int _totalCounter = 0;
    int _insideCounter = 0;

    public void StartMonteCarlo()
    {
        StartCoroutine(MonteCarloRoutine());
    }

    public void Stop()
    {
        StopAllCoroutines();
    }

    IEnumerator MonteCarloRoutine()
    {
        while (true)
        {
            for (int i = 0; i < _hordeCount; i++)
            {
                _totalCounter++;
                float x = Random.Range(0, 1f);
                float y = Random.Range(0, 1f);
                var go = new GameObject();
                go.name = "dot";
                go.transform.SetParent(transform);
                var sprite = go.AddComponent<SpriteRenderer>();
                sprite.sprite = _dotSprite;
                go.transform.position = new Vector3(x, y, 0f);

                if (Vector2.Distance(go.transform.position, Vector2.zero) > 1)
                {
                    sprite.color = _outside;
                }
                else
                {
                    sprite.color = _inside;
                    _insideCounter++;
                }
            }
            var pi = 4 * (float) _insideCounter / _totalCounter;
            _sampleCountText.text = _totalCounter.ToString();
            _approximateValueText.text = pi.ToString("0.0000");
            Debug.Log($"{_totalCounter}: {pi}");
            yield return new WaitForSeconds(_interval);
        }
    }
}
