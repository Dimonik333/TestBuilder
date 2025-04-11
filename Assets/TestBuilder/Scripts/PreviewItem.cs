using UnityEngine;

public sealed class PreviewItem : MonoBehaviour
{
    [SerializeField] private Bounds _bounds;
    [SerializeField] private Material _correctMaterial;
    [SerializeField] private Material _wrongMaterial;
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private LayerMask _layerMask;

    public bool CanPlaced()
    {
        var result = !Check();
        SetMode(result);
        return result;
    }

    public void SetMode(bool value)
    {
        var targetMaterial = value
            ? _correctMaterial
            : _wrongMaterial;
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material = targetMaterial;
    }

    public bool Check()
    {
        var position = transform.position + _bounds.center;
        var rotation = transform.rotation;
        var size = Vector3.Scale(transform.localScale, _bounds.extents / 2);
        return Physics.CheckBox(position, size, rotation, _layerMask);
    }

    private void OnDrawGizmosSelected()
    {
        var originalMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(_bounds.center, _bounds.extents);
        Gizmos.matrix = originalMatrix;
    }
}