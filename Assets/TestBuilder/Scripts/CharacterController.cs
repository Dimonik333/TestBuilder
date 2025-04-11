using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f; // �������� ����������� ������

    [SerializeField] private float _mouseSensitivity = 2f; // ���������������� ����
    [SerializeField] private float _verticalClamp = 80f;   // ����������� �� ���� ��� ������� ������

    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _bodyTransform;
    [Space]
    [SerializeField] private ItemPlacer _placer;



    private float _verticalRotation = 0f;

    void Start()
    {
        // ��������� ������ � ������ ��� ���������
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // �����������: ���������� ����������� ��� "Horizontal" (A/D) � "Vertical" (W/S)
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        // ����������� ����������� �������� ������������ ����������� ������� ������
        var moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;

        _bodyTransform.position += moveDirection * _moveSpeed * Time.deltaTime;

        // ������� ��������� �� ����������� (�����/������)
        var mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        _bodyTransform.Rotate(Vector3.up * mouseX);

        // ������� ������ �� ��������� (�����/����)
        var mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;
        _verticalRotation -= mouseY;
        // ������������ ������������ �������, ����� �������� ���������� ������
        _verticalRotation = Mathf.Clamp(_verticalRotation, -_verticalClamp, _verticalClamp);

        // ��������������, ��� �������� ������ �������� �������� �������� ������
        _cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);

        if (Input.GetKeyDown(KeyCode.Alpha1))
            _placer.enabled = true;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            _placer.enabled = false;
        if (Input.GetMouseButtonDown(0))
            _placer.Build();

        var scroll = Input.mouseScrollDelta;
        _placer.SetScroll(scroll);
    }
}
