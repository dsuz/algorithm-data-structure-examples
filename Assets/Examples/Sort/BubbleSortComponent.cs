using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

/// <summary>
/// �o�u���\�[�g����������R���|�[�l���g
/// </summary>
public class BubbleSortComponent : MonoBehaviour
{
    [SerializeField] HorizontalOrVerticalLayoutGroup _bubbleSortPanelRoot;
    [SerializeField] float _swapDuration = 0.5f;
    [SerializeField] float _intervalBetweenSwap = 0.4f;
    [SerializeField] float _arrowMoveDuration = 0.2f;
    [SerializeField] float _swapWidth = 12f;
    [SerializeField] Transform _arrow;

    public void BubbleSort()
    {
        var elements = _bubbleSortPanelRoot.GetComponentsInChildren<NumberElement>();
        StartCoroutine(BubbleSort(elements, _swapDuration, _intervalBetweenSwap));
    }

    /// <summary>
    /// �o�u���\�[�g�̎���
    /// </summary>
    /// <param name="array"></param>
    /// <param name="duration"></param>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator BubbleSort(NumberElement[] array, float duration, float waitTime)
    {
        int n = array.Length;

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                _arrow.DOMoveY(array[j].transform.position.y, _arrowMoveDuration);  // �����ړ�����
                yield return new WaitForSeconds(waitTime + _arrowMoveDuration);

                if (array[j].Value > array[j + 1].Value)
                {
                    Swap(ref array[j], ref array[j + 1], duration);
                    yield return new WaitForSeconds(duration + waitTime + _arrowMoveDuration);
                }
                else
                {
                    yield return new WaitForSeconds(waitTime + _arrowMoveDuration);
                }

                if (j == n - i - 2)
                {
                    array[j + 1].Color = Color.white; // �ʒu���m�肳����
                }
            }
        }

        yield return new WaitForSeconds(waitTime);
        array[0].Color = Color.white; // �Ō�Ɏc�����v�f���m�肳����
    }

    /// <summary>
    /// �w�肵���I�u�W�F�N�g�̕��я������ւ���
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="duration"></param>
    void Swap(ref NumberElement a, ref NumberElement b, float duration)
    {
        // ��ʏ�̈ʒu�����ւ���
        var tempPos = a.transform.position;
        Vector3[] downPath = new Vector3[] {
                        a.transform.position,
                        _swapWidth * Vector3.right + a.transform.position,
                        _swapWidth * Vector3.right + b.transform.position,
                        b.transform.position };
        Vector3[] upPath = new Vector3[] {
                        b.transform.position,
                        _swapWidth * Vector3.left + b.transform.position,
                        _swapWidth * Vector3.left + tempPos,
                        tempPos };
        a.transform.DOPath(downPath, duration, PathType.CatmullRom);
        b.transform.DOPath(upPath, duration, PathType.CatmullRom);
        // �z���̈ʒu�����ւ���
        var temp = a;
        a = b;
        b = temp;
    }
}
