using System.Net;
using UnityEngine;

public class IPAddressFinder : MonoBehaviour
{
    void Start()
    {
        string localIP = "";
        foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                localIP = address.ToString();
            }
        }
        Debug.Log("Local IP: " + localIP);
    }
}
