using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class CarMovement : MonoBehaviour
{
    [SerializeField] PhotonView view;

    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";

    private Vector2 _input;
    private bool _breakPressed;
    private Rigidbody _rb;
    public Vector3 _rbVelocity;
    private float breakTorque;

    [SerializeField] float maxTorqueValue;
    [SerializeField] float breakTorqueValue;
    [SerializeField] float maxSteerAngle;
    [SerializeField] float tirerRotationSpeed;
    [SerializeField] float downwardDragValue;

    [Space]

    [SerializeField] Transform groundCheckPos;
    [SerializeField] bool isGrounded;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] WheelCollider _wheelColliderFrontLeft;
    [SerializeField] WheelCollider _wheelColliderFrontRight;
    [SerializeField] WheelCollider _wheelColliderBackLeft;
    [SerializeField] WheelCollider _wheelColliderBackRight;

    [SerializeField] List<Transform> _wheelMeshes;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            GetInput();
            UpdateWheelMeshes();
            _rbVelocity = _rb.velocity;
        }
    }

    private void FixedUpdate()
    {
        if (view.IsMine)
        {
            ApplyTorque();
            ApplySteer();
            GroundCheck();
        }
    }

    #region Input

    private void GetInput()
    {
        _input = new Vector2(Input.GetAxisRaw(HorizontalAxis), Input.GetAxisRaw(VerticalAxis));
        _breakPressed = Input.GetKey(KeyCode.Space);
    }

    #endregion

    #region Movement

    private void ApplyTorque()
    {
        _wheelColliderFrontLeft.motorTorque = _input.y * maxTorqueValue;
        _wheelColliderFrontRight.motorTorque = _input.y * maxTorqueValue;

        breakTorque = _breakPressed == true ? breakTorqueValue : 0;

        ApplyBreakTorque();
    }

    private void ApplyBreakTorque()
    {
        _wheelColliderFrontLeft.brakeTorque = breakTorque;
        _wheelColliderFrontRight.brakeTorque = breakTorque;
        _wheelColliderBackLeft.brakeTorque = breakTorque;
        _wheelColliderBackRight.brakeTorque = breakTorque;
    }

    private void ApplySteer()
    {
        _wheelColliderFrontLeft.steerAngle = Mathf.Lerp(_wheelColliderFrontLeft.steerAngle, _input.x * maxSteerAngle, Time.deltaTime * this.tirerRotationSpeed);
        _wheelColliderFrontRight.steerAngle = Mathf.Lerp(_wheelColliderFrontRight.steerAngle, _input.x * maxSteerAngle, Time.deltaTime * this.tirerRotationSpeed);
    }

    private void GroundCheck()
    {
        isGrounded = Physics2D.Raycast(groundCheckPos.position, -transform.up, groundCheckDistance, groundLayer);
    }

    private void ApplyDownwardDrag()
    {
        if (isGrounded == false) return;

        _rb.AddForce(Vector3.down * Mathf.Abs(_rbVelocity.z) * downwardDragValue, ForceMode.Force);
    }

    #endregion

    #region Visuals
    private void UpdateWheelMeshes()
    {
        Vector3 FL_WheelPos;
        Quaternion FL_WheelRot;
        _wheelColliderFrontLeft.GetWorldPose(out FL_WheelPos, out FL_WheelRot);
        _wheelMeshes[0].transform.position = FL_WheelPos;
        _wheelMeshes[0].transform.rotation = Quaternion.Lerp(_wheelMeshes[0].transform.rotation, FL_WheelRot, Time.deltaTime * tirerRotationSpeed);

        Vector3 FR_WheelPos;
        Quaternion FR_WheelRot;
        _wheelColliderFrontRight.GetWorldPose(out FR_WheelPos, out FR_WheelRot);
        _wheelMeshes[1].transform.position = FR_WheelPos;
        _wheelMeshes[1].transform.rotation = Quaternion.Lerp(_wheelMeshes[1].transform.rotation, FR_WheelRot, Time.deltaTime * tirerRotationSpeed);

        Vector3 BL_WheelPos;
        Quaternion BL_WheelRot;
        _wheelColliderBackLeft.GetWorldPose(out BL_WheelPos, out BL_WheelRot);
        _wheelMeshes[2].transform.position = BL_WheelPos;
        _wheelMeshes[2].transform.rotation = Quaternion.Lerp(_wheelMeshes[2].transform.rotation, BL_WheelRot, Time.deltaTime * tirerRotationSpeed);

        Vector3 BR_WheelPos;
        Quaternion BR_WheelRot;
        _wheelColliderBackRight.GetWorldPose(out BR_WheelPos, out BR_WheelRot);
        _wheelMeshes[3].transform.position = BR_WheelPos;
        _wheelMeshes[3].transform.rotation = Quaternion.Lerp(_wheelMeshes[3].transform.rotation, BR_WheelRot, Time.deltaTime * tirerRotationSpeed);
    }
    #endregion

    #region Debug

    private void OnDrawGizmos()
    {
        Debug.DrawLine(groundCheckPos.position, groundCheckPos.position + new Vector3(0, -groundCheckDistance, 0), Color.green);
    }

    #endregion
}
