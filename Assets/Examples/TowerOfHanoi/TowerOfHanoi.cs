using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

public class TowerOfHanoi : MonoBehaviour
{
    [SerializeField] GameObject _diskPrefab;
    [SerializeField] Transform _peg0t;
    [SerializeField] Transform _peg1t;
    [SerializeField] Transform _peg2t;
    Stack<int> _peg0 = new Stack<int>();  // 杭 A
    Stack<int> _peg1 = new Stack<int>();  // 杭 B
    Stack<int> _peg2 = new Stack<int>();  // 杭 C
    // int _limitOfMoves;   // 円盤を動かせる回数
    int _countOfMoves;   // 円盤を動かした回数
    float _height;
    [SerializeField, Range(1, 10)]
    int _countOfDisks = 10;
    [SerializeField] float _duration = 0.2f;
    [SerializeField] float _interval = 0.8f;
    Sequence _seq;

    void Start()
    {
        
    }

    public void Restart()
    {
        DOTween.KillAll(true);
        _peg0.Clear();
        _peg1.Clear();
        _peg2.Clear();
        var disks = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (var disk in disks)
            disk.SetActive(false);
        Initialize();
    }

    void Initialize()
    {
        _seq = DOTween.Sequence();
        _height = Mathf.Max(2.5f, _countOfDisks * 0.1f + 1);

        for (int i = _countOfDisks - 1; i > -1; i--)
        {
            var disk = Instantiate(_diskPrefab);
            disk.name = "Disk" + i;
            disk.transform.localScale = new Vector3(1 + 0.4f * i, 1, 1 + 0.4f * i);
            disk.transform.position = new Vector3(0, _height + 0.2f * (_countOfDisks - i), 0);
            var r = disk.GetComponent<Renderer>();
            var m = r.material;
            m.color = new Color(Random.value, Random.value, Random.value, 1.0f);
            r.material = m;
            var rb = disk.GetComponent<Rigidbody>();
            _seq.Append(rb.DOMoveY((_countOfDisks - i - 1) * 0.1f, _duration));
            _peg0.Push(i);
        }   // 杭 A のデータを作る

        _seq.AppendInterval(_interval);
        string msg = $"{_countOfMoves}: {string.Join(" ", _peg0)}, {string.Join(" ", _peg1)}, {string.Join(" ", _peg2)}";
        // Console.WriteLine(msg);
        Debug.Log(GetPegsStatusString());
        Hanoi(_countOfDisks, new Peg(_peg0t, _peg0), new Peg(_peg2t, _peg2), new Peg(_peg1t, _peg1), _seq);
    }

    string GetPegsStatusString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(_peg0.Count == 0 ? "-" : string.Join(" ", _peg0.Reverse()));
        sb.AppendLine(_peg1.Count == 0 ? "-" : string.Join(" ", _peg1.Reverse()));
        sb.AppendLine(_peg2.Count == 0 ? "-" : string.Join(" ", _peg2.Reverse()));
        return sb.ToString();
    }

    //void Hanoi(int n, Stack<int> x, Stack<int> y, Stack<int> z, Sequence seq)
    void Hanoi(int n, Peg x, Peg y, Peg z, Sequence seq)
    {
        if (n == 1)
        {
            _countOfMoves++;
            var diskNum = x.stack.Pop();
            y.stack.Push(diskNum);
            var disk = GameObject.Find("Disk" + diskNum.ToString());
            seq.Append(disk.transform.DOMoveY(_height, _duration));
            seq.Append(disk.transform.DOMove(new Vector3(y.transform.position.x , _height, y.transform.position.z), _duration));
            seq.Append(disk.transform.DOMoveY((y.stack.Count - 1) * 0.1f, _duration));
            Debug.Log(GetPegsStatusString());
            string msg = $"{_countOfMoves}: {string.Join(" ", _peg0)}, {string.Join(" ", _peg1)}, {string.Join(" ", _peg2)}";
            // Console.WriteLine(msg);
            //if (_countOfMoves >= _limitOfMoves)
            //    return;
        }
        else
        {
            Hanoi(n - 1, x, z, y, seq);
            //if (_countOfMoves >= _limitOfMoves)
            //    return;
            _countOfMoves++;
            var diskNum = x.stack.Pop();
            y.stack.Push(diskNum);
            var disk = GameObject.Find("Disk" + diskNum.ToString());
            seq.Append(disk.transform.DOMoveY(_height, _duration));
            seq.Append(disk.transform.DOMove(new Vector3(y.transform.position.x, _height, y.transform.position.z), _duration));
            seq.Append(disk.transform.DOMoveY((y.stack.Count - 1) * 0.1f, _duration));
            Debug.Log(GetPegsStatusString());
            string msg = $"{_countOfMoves}: {string.Join(" ", _peg0)}, {string.Join(" ", _peg1)}, {string.Join(" ", _peg2)}";
            // Console.WriteLine(msg);
            //if (_countOfMoves >= _limitOfMoves)
            //    return;
            Hanoi(n - 1, z, y, x, seq);
        }
    }
}

class Peg
{
    public Transform transform;
    public Stack<int> stack;

    public Peg(Transform peg, Stack<int> data)
    {
        transform = peg;
        stack = data;
    }
}