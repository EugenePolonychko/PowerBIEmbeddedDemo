using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.PowerBI;
using Microsoft.PowerBI.AspNet.WebForms;
using Microsoft.PowerBI.Security;
using System.Configuration;
using Microsoft.PowerBI.Api.V1;
using Microsoft.Rest;
using PowerBIEmbeddedDemo.Models;

namespace PowerBIEmbeddedDemo
{
    public partial class _Default : Page
    {
        private static string workspaceCollection = ConfigurationManager.AppSettings["powerbi:WorkspaceCollection"];
        private static string workspaceId = ConfigurationManager.AppSettings["powerbi:WorkspaceId"];
        private static string accessKey = ConfigurationManager.AppSettings["powerbi:AccessKey"];
        private static string apiUrl = ConfigurationManager.AppSettings["powerbi:ApiUrl"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                List<PowerBiModel> reportsList = new List<PowerBiModel>();
                var devToken = PowerBIToken.CreateReportEmbedTokenWithScopes(workspaceCollection, workspaceId);
                using (var client = this.CreatePowerBIClient(devToken))
                {
                    var reportsResponse = client.Reports.GetReports(workspaceCollection, workspaceId);

                    for (int i = 0; i < reportsResponse.Value.ToList().Count; i++)
                    {
                        reportsList.Add(new PowerBiModel
                        {
                            Id = reportsResponse.Value[i].Id,
                            Name = reportsResponse.Value[i].Name,
                            EmbedUrl = reportsResponse.Value[i].EmbedUrl,
                            WebUrl = reportsResponse.Value[i].WebUrl,
                            //Report = reportsResponse.Value[i].
                        }
                        );
                    }
                }

                var reportId = reportsList[0].Id;
                using (var client = this.CreatePowerBIClient(devToken))
                {
                    var embedToken = PowerBIToken.CreateReportEmbedToken(workspaceCollection, workspaceId, reportId);

                    var viewModel = new PowerBiModel
                    {
                        Report = reportsList[0].Report,
                        AccessToken = embedToken.Generate(accessKey)
                    };

                    accessTokenText.Value = viewModel.AccessToken;
                    embedUrlText.Value = viewModel.EmbedUrl;
                    
                }

            }

        }

        private IPowerBIClient CreatePowerBIClient(PowerBIToken token)
        {
            var jasonWebToken = token.Generate(accessKey);
            var credentials = new TokenCredentials(jasonWebToken, "AppToken");
            var client = new PowerBIClient(credentials)
            {
                BaseUri = new Uri(apiUrl)
            };

            return client;
        }
    }
}