using System;
using UnityEngine;

[Serializable]
public sealed class PreviewItemView
{
    [SerializeField] private Material _correctMaterial;
    [SerializeField] private Material _wrongMaterial;
    [SerializeField] private Renderer[] renderers;

    private bool _mode = true;

    public void SetMode(bool value)
    {
        if (_mode == value)
            return;
        _mode = value;
        ApplyMode(_mode);
    }

    private void ApplyMode(bool value)
    {
        var targetMaterial = value
          ? _correctMaterial
          : _wrongMaterial;
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material = targetMaterial;
    }
}

[Serializable]
public sealed class PreviewItemCollisionDetector
{
    [SerializeField] private Bounds _bounds;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _transform;

    public bool CheckCollision()
    {
        var position = _transform.position + _transform.rotation * Vector3.Scale(_transform.localScale, _bounds.center); var rotation = _transform.rotation;
        var size = Vector3.Scale(_transform.localScale, _bounds.extents / 2);
        return Physics.CheckBox(position, size, rotation, _layerMask);
    }

    public void OnDrawGizmosSelected()
    {
        if (_transform == null)
            return;
        var originalMatrix = Gizmos.matrix;
        Gizmos.matrix = _transform.localToWorldMatrix;
        Gizmos.DrawWireCube(_bounds.center, _bounds.extents);
        Gizmos.matrix = originalMatrix;
    }
}

public sealed class PreviewItem : MonoBehaviour
{
    [SerializeField] private PreviewItemView _view;
    [SerializeField] private PreviewItemCollisionDetector _collisionDetector;

    public void SetViewMode(bool value)
        => _view.SetMode(value);

    public bool CheckCollision()
        => _collisionDetector.CheckCollision();

    private void OnDrawGizmosSelected()
        => _collisionDetector.OnDrawGizmosSelected();
}