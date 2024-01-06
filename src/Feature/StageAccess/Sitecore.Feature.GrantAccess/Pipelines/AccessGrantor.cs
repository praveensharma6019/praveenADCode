using Sitecore.Diagnostics;
using Sitecore.Feature.GrantAccess.Models;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;

namespace Sitecore.Feature.GrantAccess.Pipelines
{
    public class AccessGrantor : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            if (ShouldRedirect(args))
            {
                return;
            }

            RedirectToNoAccessUrl();
        }

        private bool ShouldRedirect(HttpRequestArgs args)
        {
            if (IsIPLiesInRange(GetIPAddress()))
            {
                return true;
            }
            else
            {
                if (IsHostNameWhitelisted(args.HttpContext.Request.Url.Host))
                    return false;
                else
                    return true;
            }
        }

        protected virtual bool IsIPLiesInRange(string ip)
        {
            Assert.ArgumentNotNullOrEmpty(ip, "ip");
            foreach (var ipRange in _ipRanges)
            {
                if (IsInRange(ipRange.From, ipRange.To, ip))
                    return true;
                continue;
            }
            return false;
        }

        protected virtual bool IsIPWhitelisted(string ip)
        {
            Assert.ArgumentNotNullOrEmpty(ip, "ip");
            return AllowedIPs.Contains(ip);
        }

        protected virtual bool IsHostNameWhitelisted(string hostName)
        {
            Assert.ArgumentNotNullOrEmpty(hostName, "hostName");
            if (AllowedHOSTNAMEs.Count == 0)
                return true;
            return AllowedHOSTNAMEs.Contains(hostName);
        }

        protected virtual void RedirectToNoAccessUrl()
        {
            WebUtil.Redirect("/page-not-found", false);
        }

        private readonly List<IpRange> _ipRanges = new List<IpRange>();

        public IEnumerable<IpRange> IpRanges
        {
            get { return _ipRanges; }
        }

        protected virtual void AddIpRanges(XmlNode node)
        {
            if (node == null)
                return;
            if (node.Attributes == null)
                return;
            if (node.Attributes["from"] == null)
                return;
            if (node.Attributes["from"].Value == null)
                return;
            if (node.Attributes["to"] == null)
                return;
            if (node.Attributes["to"].Value == null)
                return;

            _ipRanges.Add(new IpRange() { From = node.Attributes["from"].Value, To = node.Attributes["to"].Value });
        }


        private IList<string> _AllowedIPs;
        private IList<string> AllowedIPs
        {
            get
            {
                if (_AllowedIPs == null)
                {
                    _AllowedIPs = new List<string>();
                }

                return _AllowedIPs;
            }
        }
        protected virtual void AddAllowedIP(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip) || AllowedIPs.Contains(ip))
            {
                return;
            }

            AllowedIPs.Add(ip);
        }

        private IList<string> _AllowedHOSTNAMEs;
        private IList<string> AllowedHOSTNAMEs
        {
            get
            {
                if (_AllowedHOSTNAMEs == null)
                {
                    _AllowedHOSTNAMEs = new List<string>();
                }

                return _AllowedHOSTNAMEs;
            }
        }

        protected virtual void AddAllowedHOSTNAME(string hostName)
        {
            if (string.IsNullOrWhiteSpace(hostName) || AllowedHOSTNAMEs.Contains(hostName))
            {
                return;
            }

            AllowedHOSTNAMEs.Add(hostName);
        }


        public static bool IsInRange(string startIpAddr, string endIpAddr, string address)
        {
            long ipStart = BitConverter.ToInt32(IPAddress.Parse(startIpAddr).GetAddressBytes().Reverse().ToArray(), 0);

            long ipEnd = BitConverter.ToInt32(IPAddress.Parse(endIpAddr).GetAddressBytes().Reverse().ToArray(), 0);

            long ip = BitConverter.ToInt32(IPAddress.Parse(address).GetAddressBytes().Reverse().ToArray(), 0);

            return ip >= ipStart && ip <= ipEnd;
        }

        public static string GetIPAddress()
        {
            try
            {
                HttpContext context = System.Web.HttpContext.Current;
                string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ipAddress))
                {
                    string[] addresses = ipAddress.Split(',');
                    if (addresses.Length != 0)
                    {
                        string clientIpAddress = addresses.Last().Trim();
                        string[] clientIpAddressWithPortArray = clientIpAddress.Split(':');
                        if (clientIpAddressWithPortArray.Length != 0)
                        {
                            Log.Info("Current User IPs:" + clientIpAddressWithPortArray[0] + "Actual IP:" + clientIpAddress + "Requested URL:" + HttpContext.Current.Request.Url.ToString(), "Client IP");
                            return clientIpAddressWithPortArray[0];
                        }
                    }
                }

                return context.Request.ServerVariables["REMOTE_ADDR"];
            }
            catch (Exception e)
            {
                return e.StackTrace;
            }
        }
    }
}