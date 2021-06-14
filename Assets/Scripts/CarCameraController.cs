using UnityEngine;

[RequireComponent(typeof(CarMovement))]
public class CarCameraController : MonoBehaviour
{
    private CarMovement _carMovement;

    [SerializeField] GameObject frontCam;
    [SerializeField] GameObject rearCam;

    private void Start()
    {
        _carMovement = GetComponent<CarMovement>();
    }

    private void Update()
    {
        Vector3 velocity = _carMovement._rbVelocity.normalized;

        float rad_angle = Mathf.Acos(Vector3.Dot(transform.forward, velocity));
        float deg_angle = rad_angle * Mathf.Rad2Deg;

        SetCamera(deg_angle);

        Debug.Log(deg_angle);
    }

    private void SetCamera(float angle)
    {
        if (Mathf.Abs(angle) < 90)
        {
            frontCam.SetActive(true);
            rearCam.SetActive(false);
        }
        else if (Mathf.Abs(angle) > 0)
        {
            frontCam.SetActive(false);
            rearCam.SetActive(true);
        }
    }
}
