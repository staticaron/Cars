using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarMovement : MonoBehaviour
{
    [SerializeField] float carMoveForce;
    [SerializeField] float carMaxMoveSpeed;
    [SerializeField] float carTurnTorque;
    [Space]
    [SerializeField] float backwardMovementModifier;
    [SerializeField] float brakeModifier;
    [Space]
    [SerializeField] Transform torquePoint;

    private Rigidbody _carBody;
    private Vector2 _input;

    private void Start()
    {
        _carBody = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        //Apply Directional Force
        if (_input.y < 0 && _carBody.velocity.z > 0)
        {
            //Condition for Brake
            _carBody.AddForce(_input.y * transform.forward * carMoveForce * brakeModifier);
        }
        else if (_input.y < 0 && _carBody.velocity.z < 0)
        {
            //Condition for Rear
            _carBody.AddForce(_input.y * transform.forward * carMoveForce * backwardMovementModifier);
        }
        else
        {
            _carBody.AddForce(_input.y * transform.forward * carMoveForce);
        }

        //Apply Torque
        _carBody.AddForceAtPosition(transform.right * carTurnTorque * _input.x * _input.y, torquePoint.position, ForceMode.Force);

        //Apply Forward Force
    }
}
