//GetDNS
// I'm using this code in an attempt to discover what the issue is
// where my network refuses to load lowes.com
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main()
    {
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            
            if (ni.OperationalStatus == OperationalStatus.Up)
            {
                IPInterfaceProperties ipProps = ni.GetIPProperties();
                foreach (IPAddress dns in ipProps.DnsAddresses)
                {
                    Console.WriteLine($"network interface: {ni.Name} - DNS Server: {dns}");
                }
                
            }
            IPInterfaceProperties properties = ni.GetIPProperties();
            Console.WriteLine(ni.Description);
            Console.WriteLine($"DNS suffix .............................. : {properties.DnsSuffix}");
            if (!OperatingSystem.IsMacOS() ){
                Console.WriteLine($"DNS enabled ............................. : {properties.IsDnsEnabled}");
                Console.WriteLine($"Dynamically configured DNS .............. : {properties.IsDynamicDnsEnabled}");
            }
        }
    }
}

