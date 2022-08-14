using System;
using TMPro;
using UnityEngine;

public class BlockCounter : MonoBehaviour
{
    public FactoryCreator factoryCreator;

    public AnimationCurve updateAnimation;

    private TMP_Text _counter;
    private float _animationStart;
    private const float AnimationLenght = .8f;
    private static readonly Vector3 StartScale = new Vector3(1f, 1f, 1f);
    private static readonly Vector3 MaxScale = new Vector3(3f, 3f, 1f);

    void Awake()
    {
        _counter = GetComponent<TMP_Text>();
    }

    void Start()
    {
        factoryCreator.ConsumedBlock += UpdateCounter;
    }

    private void Update()
    {
        if (Time.time - _animationStart <= AnimationLenght)
        {
            var delta = (Time.time - _animationStart) / AnimationLenght;
            var curvedDelta = updateAnimation.Evaluate(delta) * AnimationLenght;
            transform.localScale = Vector3.Lerp(StartScale, MaxScale, curvedDelta);
            Debug.Log("delta: " + delta + ", cdelta: " + curvedDelta + " SCALE: " + transform.localScale);
        }
        else
        {
            transform.localScale = StartScale;
        }
    }

    private void UpdateCounter(int blocksLeft)
    {
        _counter.text = blocksLeft.ToString();
        _animationStart = Time.time;
    }
}