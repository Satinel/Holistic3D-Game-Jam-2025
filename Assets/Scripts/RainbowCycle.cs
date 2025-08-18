using UnityEngine;

public class RainbowCycle : MonoBehaviour
{
    [SerializeField] Color32[] _colors;
    [SerializeField] float _speed;

    int _currentColorIndex = 0, _nextColorIndex = 1;
    float _lerpProgress;

    public Color LerpColor()
    {
        if(_colors.Length < 1) { return Color.white; }

        Color currentColor = _colors[_currentColorIndex];
        Color nextColor = _colors[_nextColorIndex];

        Color colorNow = Color.Lerp(currentColor, nextColor, _lerpProgress);

        _lerpProgress += Time.deltaTime * _speed;

        if(_lerpProgress >= 1f)
        {
            _lerpProgress = 0;
            _currentColorIndex = _nextColorIndex;
            _nextColorIndex = (_nextColorIndex + 1) % _colors.Length;
        }

        return colorNow;
    }
}
