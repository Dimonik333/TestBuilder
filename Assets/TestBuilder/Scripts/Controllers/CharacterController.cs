using UnityEngine;

public sealed class CharacterController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f; 
    [SerializeField] private float _mouseSensitivity = 2f; 
    
    [Space]
    [SerializeField] private Character _character;


    void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        _character.Move(new Vector2(horizontalInput, verticalInput));
        
        var mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        var mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;
        _character.Rotate(new Vector2(mouseX, mouseY));
    }
}
