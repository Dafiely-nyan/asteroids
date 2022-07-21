using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector3 CursorWorldPosition { get; private set; }
    public static Vector2 VisibleArea { get; private set; }

    public const float DeltaDistance = 0.01f;

    private Camera _mainCam;
    private float _distanceToCamera;

    private void Awake()
    {
        _mainCam = DependencyContainer.Instance.MainCamera;
        _distanceToCamera = Mathf.Abs(_mainCam.transform.position.z);
            
        SetVisibleArea();
    }

    private void Update()
    {
        SetCurrentCursorWorldPosition();
        SetVisibleArea();
    }

    void SetCurrentCursorWorldPosition()
    {
        CursorWorldPosition =
            _mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _distanceToCamera));
    }

    void SetVisibleArea()
    {
        float screenAspectRation = (float)_mainCam.pixelWidth / _mainCam.pixelHeight;
        float orthographicSize = _mainCam.orthographicSize;
            
        VisibleArea = new Vector2(orthographicSize * screenAspectRation, orthographicSize);
    }
}