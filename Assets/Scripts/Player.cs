
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [Header("* * * Base Settings * * *")]
    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private float _rotSpeed = 45.0f;

    [Header("* * * ínå`åüèo * * *")]
    [SerializeField] private float _rayDistance = 2.0f;
    [SerializeField] private Vector3 _rayPoint;
    [SerializeField] private Vector3 _rayDirection;

    private Vector3 _inputAxis;  
    private Vector3 _moveVelocity;     
    private Vector3 _slopeDirection;    

    void Start()
    {

    }
    void Update()
    {
        Movement();   
        Rotation();  

        DrawLocalDirectionRay();

        var terrain = GetTerrain(); 
        if (terrain.transform != null)
        {
            Debug.DrawRay(terrain.point, terrain.normal, Color.yellow, 0, false);

            var cross = Vector3.Cross(transform.right, terrain.normal);
            Debug.DrawRay(terrain.point, cross, Color.yellow, 0, false);

            _slopeDirection = cross.normalized;
        }
    }

    public void Movement()
    {
        _moveVelocity = new Vector3(0, 0, _inputAxis.z) * _moveSpeed * Time.deltaTime;

        transform.Translate(_slopeDirection * _moveSpeed * _inputAxis.z * Time.deltaTime);
    }

    public void Rotation()
    {
        transform.Rotate(0, _inputAxis.x * _rotSpeed * Time.deltaTime, 0);
    }

    public void OnMove(InputValue value)
    {
        Vector2 axis = value.Get<Vector2>();

        _inputAxis.x = axis.x;
        _inputAxis.y = 0;
        _inputAxis.z = axis.y;
    }

    public void DrawLocalDirectionRay()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.blue, 0, false);
        Debug.DrawRay(transform.position, transform.up, Color.green, 0, false);
        Debug.DrawRay(transform.position, transform.right, Color.red, 0, false);
    }
    public RaycastHit GetTerrain()
    {
        _rayPoint = transform.position;
        _rayDirection = transform.up * -1;

        bool isHit = Physics.Raycast(
            origin: _rayPoint,
            direction: _rayDirection,
            hitInfo: out var hit,
            maxDistance: _rayDistance,
            layerMask: LayerMask.GetMask("Terrain"));

        Debug.DrawRay(_rayPoint, _rayDirection * _rayDistance, Color.brown, 0, false);

        if (isHit)
        {
            return hit;
        }

        return default;

    }
}