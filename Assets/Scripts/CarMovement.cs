using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarMovement : MonoBehaviour
{
    [SerializeField] float carMoveForce;
    [SerializeField] float carMaxMoveSpeed;
    [SerializeField] float carTurnTorque;
    [Space]
    [SerializeField] float backwardMovementModifier;
    [Space]
    [SerializeField] Transform leftRightTorquePosition;

    private Rigidbody _carBody;
    private Vector2 _input;

    private void Start()
    {
        _carBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Mathf.Clamp(Input.GetAxisRaw("Vertical"), -1 * backwardMovementModifier, 1));
    }

    private void FixedUpdate()
    {
        //Apply Directional Force
        _carBody.AddForce(_input.y * transform.forward * carMoveForce);

        //Apply Torque
        _carBody.AddTorque(leftRightTorquePosition.position * carTurnTorque, ForceMode.Force);

        //Apply Forward Force
    }
}
