using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
    public GameObject[] autos; 
    public Transform spawnPoint;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); 
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conexión completada con el Master");
        PhotonNetwork.JoinRandomOrCreateRoom(); 
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Te has unido a una sala");
        // Obtener el índice del auto seleccionado
        int indiceAuto = PlayerData.AutoSeleccionado;

        // Instanciar el auto correspondiente
        GameObject auto = PhotonNetwork.Instantiate(
            autos[indiceAuto].name,
            spawnPoint.position,
            spawnPoint.rotation
        );

        Debug.Log($"Auto instanciado: {autos[indiceAuto].name}");
    }

}
