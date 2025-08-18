using UnityEngine;
using TMPro;

[RequireComponent(typeof(RainbowCycle))]
public class FloatingScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] float _lifeTime = 5f, _risingSpeed = 0.25f;

    RainbowCycle _rainbowCycle;

    void Awake()
    {
        _rainbowCycle = GetComponent<RainbowCycle>();
        Destroy(gameObject, _lifeTime);
    }

    void Update()
    {
        _text.color = _rainbowCycle.LerpColor();
        transform.Translate(_risingSpeed * Time.deltaTime * transform.position * Vector2.up);
    }

    public void SetText(string value)
    {
        _text.text = value;
    }
}
