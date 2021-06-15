using UnityEngine;
using Photon.Pun;

public class SpawnCars : MonoBehaviour
{
    public Transform position;
    public GameObject Car;

    public int spawnIndex = 0;

    private void Start()
    {
        PhotonNetwork.Instantiate(Car.name, position.position, Quaternion.identity);
    }
}
