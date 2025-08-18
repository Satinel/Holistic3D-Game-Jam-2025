using UnityEngine;

public class OrthoCameraResize : MonoBehaviour
{
    [SerializeField] float _maxSize = 7f;
    [SerializeField] SpriteRenderer _leftBorder, _topBorder;
    bool _adjustmentComplete;

    void Update()
    {
        if(_adjustmentComplete) { return; }

        if(!_leftBorder.isVisible || !_topBorder.isVisible)
        {
            Camera.main.orthographicSize += 0.01f;

            if(Camera.main.orthographicSize >= _maxSize)
            {
                _adjustmentComplete = true;
            }
        }
        else
        {
            _adjustmentComplete = true;
        }
    }
}
