using UnityEngine;

public class OrthoCameraResize : MonoBehaviour
{
    [SerializeField] SpriteRenderer _leftBorder, _topBorder;

    void Update()
    {
        if(!_leftBorder.isVisible || !_topBorder.isVisible)
        {
            Camera.main.orthographicSize += 0.01f;
        }
    }
}
