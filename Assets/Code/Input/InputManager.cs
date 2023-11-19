using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Vector2 _touchPosition;

    public Vector2 TouchPosition
    {
        get => _touchPosition;
    }
    
    void Update()
    {
        if (Input.touchSupported)
            TouchMove();
        else
            MouseMove();
    }

    private void TouchMove()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchPosition = touch.position;
                _touchPosition = touchPosition;
            }
        }
    }

    private void MouseMove()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _touchPosition = mousePosition;
        }
    }
}
