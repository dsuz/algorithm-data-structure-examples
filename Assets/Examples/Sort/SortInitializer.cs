using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ソートするための数字の集合を生成する
/// </summary>
public class SortInitializer : MonoBehaviour
{
    [SerializeField] int _arraySize = 10;
    [SerializeField] int _maxValue = 1000;
    [SerializeField] NumberElement _numberElementPrefab;
    [SerializeField] HorizontalOrVerticalLayoutGroup _sortPanelRoot;
    [SerializeField] Button _sortButton;
    [SerializeField] Text _arrow;
    Color[] _colors = { Color.red, Color.green, Color.blue, Color.brown, Color.magenta, Color.violet, Color.coral, Color.gray, Color.cyan, Color.darkKhaki };

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _sortButton.interactable = false;
        StartCoroutine(InitializeRoutine());
    }

    IEnumerator InitializeRoutine()
    {
        foreach (Transform child in _sortPanelRoot.transform)
            Destroy(child.gameObject);

        _sortPanelRoot.enabled = true;

        for (int i = 0; i < _arraySize; i++)
        {
            NumberElement numberElement = Instantiate(_numberElementPrefab, _sortPanelRoot.transform);
            numberElement.Value = Random.Range(1, _maxValue);
            numberElement.Color = _colors[i % _colors.Length];
            yield return new WaitForSecondsRealtime(0.1f);
        }
        
        yield return null;
        var firstChild = _sortPanelRoot.transform.GetChild(0);
        _arrow.transform.position = new Vector3(_arrow.transform.position.x, firstChild.position.y, _arrow.transform.position.z);   // 矢印の位置を最初の要素に合わせる
        _sortPanelRoot.enabled = false;   // 1フレーム待たないとレイアウトが設定されない
        _sortButton.interactable = true;
    }
}
