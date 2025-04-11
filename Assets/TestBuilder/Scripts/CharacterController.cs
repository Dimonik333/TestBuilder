using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f; // скорость перемещени€ игрока

    [SerializeField] private float _mouseSensitivity = 2f; // чувствительность мыши
    [SerializeField] private float _verticalClamp = 80f;   // ограничение по углу дл€ наклона камеры

    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _bodyTransform;
    [Space]
    [SerializeField] private ItemPlacer _placer;



    private float _verticalRotation = 0f;

    void Start()
    {
        // Ѕлокируем курсор и делаем его невидимым
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // ѕеремещение: используем стандартные оси "Horizontal" (A/D) и "Vertical" (W/S)
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        // –асчитываем направление движени€ относительно направлени€ взгл€да игрока
        var moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;

        _bodyTransform.position += moveDirection * _moveSpeed * Time.deltaTime;

        // ѕоворот персонажа по горизонтали (влево/вправо)
        var mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        _bodyTransform.Rotate(Vector3.up * mouseX);

        // ѕоворот камеры по вертикали (вверх/вниз)
        var mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;
        _verticalRotation -= mouseY;
        // ќграничиваем вертикальный поворот, чтобы избежать переворота камеры
        _verticalRotation = Mathf.Clamp(_verticalRotation, -_verticalClamp, _verticalClamp);

        // ѕредполагаетс€, что основна€ камера €вл€етс€ дочерним объектом игрока
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
