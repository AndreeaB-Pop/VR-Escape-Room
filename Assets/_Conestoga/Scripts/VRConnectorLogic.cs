using HPhysic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRConnectorLogic : MonoBehaviour
{
    [SerializeField] Connector connector;
    XRSocketInteractor XRSocket;
    [SerializeField] Connector secondConnector;

    private void Start()
    {
        connector = GetComponentInChildren<Connector>();
    }

    public void CheckSocketConnector()
    {
        if (secondConnector != null)
        {
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
            }
        }
    }

    public void Disconnect()
    {
        connector.Disconnect();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Connector")
        {
            print("this isn't a connector");
            return;
        }
        else
        {
            // Check if the connection has its own connector.
            secondConnector = other.GetComponent<Connector>();
        }
    }
}
