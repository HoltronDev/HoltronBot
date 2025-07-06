// Filename:  HttpServer.cs        
// Author:    Benjamin N. Summerton <define-private-public>        
// License:   Unlicense (http://unlicense.org/)

using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using HoltronBot.Twitch.Models.EventSubAPIMessages;
using System.Web;

namespace HoltronBot
{
    public class HttpServer
    {
        public static HttpListener listener;
        public static string url = "http://localhost:3000/";
        public static int pageViews = 0;
        public static int requestCount = 0;
        public static string pageData =
            "<!DOCTYPE>" +
            "<html>" +
            "  <head>" +
            "    <title>HttpListener Example</title>" +
            "  </head>" +
            "  <body>" +
            "    <p>Page Views: {0}</p>" +
            "    <form method=\"post\" action=\"shutdown\">" +
            "      <input type=\"submit\" value=\"Shutdown\" {1}>" +
            "    </form>" +
            "  </body>" +
            "</html>";


        public static async Task<AuthCodeResponse> HandleIncomingConnection(CancellationToken cancellationToken)
        {
            using var listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();

            bool shutdownListener = false;
            AuthCodeResponse authCodeResponse = null;

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (!shutdownListener)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                // Print out some info about the request
                Console.WriteLine("Request #: {0}", ++requestCount);
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();

                if (req.QueryString != null)
                {
                    authCodeResponse = new AuthCodeResponse();
                    var queryDictionary = HttpUtility.ParseQueryString(req.Url.Query);
                    if (queryDictionary.Keys[0] != "error")
                    {
                        authCodeResponse.Code = queryDictionary["code"];
                        authCodeResponse.Scope = queryDictionary["scope"];
                    }
                    else
                    {
                        authCodeResponse.Error = queryDictionary["error"];
                        authCodeResponse.ErrorDescription = queryDictionary["error_description"];
                    }

                    shutdownListener = true;
                }
            }

            Console.WriteLine("Returning Auth Code Response.");
            return authCodeResponse;
        }
    }
}