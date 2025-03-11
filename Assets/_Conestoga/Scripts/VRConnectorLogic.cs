using HPhysic;
using UnityEngine;

public class VRConnectorLogic : MonoBehaviour
{
    [SerializeField] Connector connector;
    [SerializeField] Connector secondConnector;

    private void Start()
    {
        connector = GetComponentInChildren<Connector>();
    }

    /// <summary>
    /// Check the connection of the socket and force it to the second connector, then check if the connection is correct to the socket logic brain.
    /// </summary>
    public void CheckSocketConnector()
    {
        print("checking socket connection");
        if (secondConnector != null)
        {
            // Crossed out areas are just for when testing.
            connector.ConnectedTo = secondConnector;
            //secondConnector.ConnectedTo = connector;
            connector.Connect(secondConnector);
            //secondConnector.Connect(connector);
            //connector.SetConnectionToTrue();

            if (connector.IsConnectedRight == false)
            {
                print("incorrect combo");
            } else
            {
                print("it's correct");
                SocketLogic.AddSocketInside();
            }
        } else
        {
            print("for some reason the second connection is null");
        }
    }

    public void Disconnect()
    {
        SocketLogic.RemoveSocket();
        connector.Disconnect();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Connector"))
        {
            return;
        }
        else
        {
            // Check if the connection has its own connector.
            secondConnector = other.GetComponent<Connector>();
        }
    }
}
