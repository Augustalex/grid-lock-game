using System;
using TMPro;
using UnityEngine;

public class BasicBlockController : MonoBehaviour
{
    private TMP_Text _text;

    private int _value = 0;

    public int GetNumber()
    {
        return _value;
    }

    public void DecreaseValue()
    {
        _value = Math.Max(0, _value - 1);
    }

    public void IncreaseValue()
    {
        _value = Math.Min(5, _value + 1);
    }

    void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        var displayValue = Math.Min(3, _value);
        _text.text = _value > 1 ? displayValue.ToString() : "";
    }
}