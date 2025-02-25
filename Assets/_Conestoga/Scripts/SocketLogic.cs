using UnityEngine;

public class SocketLogic : MonoBehaviour
{
    // This is a messy way to go about it, but just checks the number of sockets filled + which are triggered to be true.
    int socketsFilled = 0;

    int rightSocketsFilled = 0;
    [SerializeField] int rightSocketsMax = 2;

    [SerializeField] bool correctSocket = false;

    public void CorrectSocketTrigger()
    {
        correctSocket = true;
    }

    public void DisableCorrectSocketTrigger()
    {
        correctSocket = false;
    }

    public void SetSocketIn()
    {
        socketsFilled++;
        if (correctSocket) rightSocketsFilled++;
    }

    public void RemoveSocketOut()
    {
        socketsFilled--;
        if (correctSocket) rightSocketsFilled--;
    }

    public void CheckSocketsCorrect()
    {
        if (rightSocketsFilled == rightSocketsMax)
        {
            Debug.Log("yay correct password");
        }
        else
        {
            Debug.Log("aww wrong match");
        }
    }

}
