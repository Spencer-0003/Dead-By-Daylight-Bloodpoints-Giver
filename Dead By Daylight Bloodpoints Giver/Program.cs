using System;
using System.IO;
using System.Net;

namespace Dead_By_Daylight_Bloodpoints_Giver
{
    internal class Program
    {
        // Functions
        static bool ValidateBloodpoints(string points)
        {
            int normalized;
            try
            {
                normalized = Convert.ToInt32(points);
            }
            catch
            {
                return false;
            }

            return !(normalized <= 0 || normalized > 1000000);
        }

        // Main
        static void Main()
        {
            Console.Title = "Dead By Daylight Bloodpoint Giver";
            Console.WriteLine("Enter your bhvrSession.");

            string bhvrSession = Console.ReadLine();
            Console.WriteLine("How many bloodpoints do you want? (1 - 1,000,000).");
            string bloodpoints = Console.ReadLine();

            // Validate inputs
            if (bhvrSession == "" || bloodpoints == "")
            {
                Console.WriteLine("Missing bhvrSession or bloodpoints.");
                Console.ReadKey();
                return;
            }

            if (!ValidateBloodpoints(bloodpoints))
            {
                Console.WriteLine("Invalid bloodpoints.");
                Console.ReadKey();
                return;
            }

            // Create request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://brill.live.bhvrdbd.com/api/v2/extensions/rewards/grantCurrency/");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.UserAgent = "DeadByDaylight/++DeadByDaylight+Live-CL-509094 EGS/10.0.22000.1.256.64bit";
            request.Headers["x-kraken-client-os"] = "10.0.22000.1.256.64bit";
            request.Headers["x-kraken-client-platform"] = "egs";
            request.Headers["x-kraken-client-provider"] = "egs";
            request.Headers["x-kraken-client-resolution"] = "1920x1080";
            request.Headers["x-kraken-client-timezone-offset"] = "0";
            request.Headers["x-kraken-client-version"] = "5.4.2";
            request.CookieContainer = new CookieContainer();

            // Create cookie
            Cookie session = new Cookie();
            session.Name = "bhvrSession";
            session.Value = bhvrSession;
            session.Domain = "brill.live.bhvrdbd.com";
            request.CookieContainer.Add(session);

            // Add request body & send request
            using (Stream requestStream = request.GetRequestStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(requestStream))
                {
                    streamWriter.Write("{\"data\":{\"rewardType\":\"Story\",\"walletToGrant\":{\"balance\":" + bloodpoints + ",\"currency\":\"Bloodpoints\"}}}");
                }
            }

            try
            {
                request.GetResponse();
                Console.WriteLine("Successfully granted bloodpoints.");
            }
            catch
            {
                Console.WriteLine("Request failed, invalid session?");
            }

            Console.ReadKey();
        }
    }
}
