using UnityEngine;

public class OutOfScreenHandler : MonoBehaviour
{
    private Transform _transform;
    void Awake() => _transform = transform;

    private void LateUpdate()
    {
        if (!GameState.NonPausedUpdate) return;
            
        Vector3 pos = _transform.position;
            
        if (Mathf.Abs(pos.x) > Utils.VisibleArea.x)
            _transform.position = new Vector3(-Mathf.Sign(pos.x) * (Utils.VisibleArea.x - Utils.DeltaDistance), pos.y);
        if (Mathf.Abs(pos.y) > Utils.VisibleArea.y)
            _transform.position = new Vector3(pos.x, -Mathf.Sign(pos.y) * (Utils.VisibleArea.y - Utils.DeltaDistance));
    }
}