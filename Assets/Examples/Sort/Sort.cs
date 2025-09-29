using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class Sort : MonoBehaviour
{
    [SerializeField] NumberElement _numberElementPrefab;
    [SerializeField] HorizontalOrVerticalLayoutGroup _bubbleSortPanelRoot;
    Color[] _colors = { Color.black, Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.violet, Color.coral, Color.gray };

    void Start()
    {
        int[] array = { 64, 34, 25, 12, 22, 11, 90, 54, 1, 23, 45 };

        for (int i = 0; i < array.Length; i++)
        {
            NumberElement numberElement = Instantiate(_numberElementPrefab, _bubbleSortPanelRoot.transform);
            numberElement.Value = array[i];
            numberElement.Color = _colors[i % _colors.Length];
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BubbleSort()
    {
        _bubbleSortPanelRoot.enabled = false;
        StartCoroutine(BubbleSort(_bubbleSortPanelRoot.GetComponentsInChildren<NumberElement>()));
    }

    IEnumerator BubbleSort(NumberElement[] array)
    {
        int n = array.Length;

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (array[j].Value > array[j + 1].Value)
                {
                    // Swap array[j] and array[j + 1]
                    //var temp = array[j].Value;
                    //array[j].Value = array[j + 1].Value;
                    //array[j + 1].Value = temp;

                    //var temp = array[j];
                    //var tempPos = array[j].Position;
                    //array[j].Position = array[j + 1].Position;
                    //array[j + 1].Position = tempPos;
                    //array[j] = array[j + 1];
                    //array[j + 1] = temp;

                    //Swap(ref array[j], ref array[j + 1]);

                    var tempPos = array[j].Position;

                    array[j].GetComponent<RectTransform>().DOAnchorPos(array[j + 1].Position, 0.5f);
                    array[j + 1].GetComponent<RectTransform>().DOAnchorPos(tempPos, 0.5f);
                    var temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;

                    yield return new WaitForSeconds(0.8f);
                }
            }
        }
    }

    void Swap(ref NumberElement a, ref NumberElement b)
    {
        var temp = a;
        var tempPos = a.Position;
        a.Position = b.Position;
        b.Position = tempPos;
        a = b;
        b = temp;
    }
}
