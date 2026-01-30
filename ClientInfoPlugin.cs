using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace DNWS
{
  class ClientInfoPlugin : IPlugin
  {
    protected static Dictionary<String, int> statDictionary = null;
    public ClientInfoPlugin()
    {
      if (statDictionary == null)
      {
        statDictionary = new Dictionary<String, int>();

      }
    }

    public void PreProcessing(HTTPRequest request)
    {
      if (statDictionary.ContainsKey(request.Url))
      {
        statDictionary[request.Url] = (int)statDictionary[request.Url] + 1;
      }
      else
      {
        statDictionary[request.Url] = 1;
      }
    }

    public HTTPResponse GetResponse(HTTPRequest request)
    {
      StringBuilder sb = new StringBuilder();

      IPEndPoint endpoint = IPEndPoint.Parse(request.getPropertyByKey("remoteendpoint"));

      //Add header
      string userAgent = request.getPropertyByKey("user-agent") ?? "N/A";
      string acceptLang = request.getPropertyByKey("accept-language") ?? "N/A";
      string acceptEnc = request.getPropertyByKey("accept-encoding") ?? "N/A";

      sb.Append("<html><body><pre>");
      sb.AppendFormat("Client IP: {0}<br/>\n", endpoint.Address);
      sb.AppendFormat("Client Port: {0}<br/>\n", endpoint.Port);
      //Remove .Trim
      sb.AppendFormat("Browser Information: {0}<br/>\n", userAgent);
      sb.AppendFormat("Accept Language: {0}<br/>\n", acceptLang);
      sb.AppendFormat("Accept Encoding: {0}<br/>\n", acceptEnc);

      sb.Append("</pre></body></html>");

      HTTPResponse response = new HTTPResponse(200);
      response.body = Encoding.UTF8.GetBytes(sb.ToString());
      return response;
    }

    public HTTPResponse PostProcessing(HTTPResponse response)
    {
      // throw new NotImplementedException(); 
      return response;
    }
  }
}