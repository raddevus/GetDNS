//GetDNS
// I'm using this code in an attempt to discover what the issue is
// where my network refuses to load lowes.com
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;


class Program
{
    static void Main(string [] args)
    {
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            
            if (ni.OperationalStatus == OperationalStatus.Up)
            {
                IPInterfaceProperties ipProps = ni.GetIPProperties();
                foreach (IPAddress dns in ipProps.DnsAddresses)
                {
                    Console.WriteLine($"interface: {ni.Name} - DNS Server: {dns} : dns suffix: {ipProps.DnsSuffix}");
                }
                
            }
        }
        var targetUrl = args.FirstOrDefault() ?? "lowes.com";
        var routes = GetTraceRoute(targetUrl);
        Console.WriteLine($"routes.Count : {routes.Count()}");
        
    }

    // Got the following function 
    // from : https://stackoverflow.com/questions/142614/traceroute-and-ping-in-c-sharp/45565253#45565253

    public static IEnumerable<IPAddress> GetTraceRoute(string hostname)
    {
        // following are similar to the defaults in the "traceroute" unix command.
        const int timeout = 10000;
        const int maxTTL = 30;
        
        byte[] buffer = {65, 66, 67, 68, 69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86};
        Console.WriteLine($"sending: {System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length)} \nto {hostname}");
        using (var pinger = new Ping())
        {
            for (int ttl = 1; ttl <= maxTTL; ttl++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                PingOptions options = new PingOptions(ttl, true);
                PingReply reply = pinger.Send(hostname, timeout, buffer, options);
                sw.Stop();
                Console.WriteLine($"reply: {reply.Address} - {System.Text.Encoding.UTF8.GetString(reply.Buffer, 0,reply.Buffer.Length)} took: {(float)sw.ElapsedMilliseconds/1000}s ");
                // we've found a route at this ttl
                if (reply.Status == IPStatus.Success || reply.Status == IPStatus.TtlExpired)
                    yield return reply.Address;

                // if we reach a status other than expired or timed out, we're done searching or there has been an error
                if (reply.Status != IPStatus.TtlExpired && reply.Status != IPStatus.TimedOut)
                    break;
            }
        }
    }
}

