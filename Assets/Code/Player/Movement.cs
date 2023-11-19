using UnityEngine;
using Zenject;

public class Movement : MonoBehaviour
{
    [Inject] private InputManager _inputManager;

    private bool _isDragging;
    
    private void Update()
    {
        if (_isDragging)
            MovePlayer();
    }

    private void MovePlayer()
    {
        transform.position = _inputManager.TouchPosition;
    }

    private void OnMouseDown()
    {
        _isDragging = true;
    }

    private void OnMouseUp()
    {
        _isDragging = false;
    }
}
