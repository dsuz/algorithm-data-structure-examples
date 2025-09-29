using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ”š‚ğ‰æ–Ê‚É•\¦‚·‚é
/// </summary>
public class NumberElement : MonoBehaviour
{
    [SerializeField] private Text _text;

    public int Value {
        get => int.Parse(_text.text);
        set => _text.text = value.ToString("000");
    }

    public Vector2 Position {
        get => GetComponent<RectTransform>().anchoredPosition;
        set => GetComponent<RectTransform>().anchoredPosition = value;
    }

    public Color Color {
        get => _text.color;
        set => _text.color = value;
    }
}
