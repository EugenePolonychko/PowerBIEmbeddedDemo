using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBIEmbeddedDemo.Models
{
    public class PowerBiModel
    {
     

            public string Id { get; set; }
            public string Name { get; set; }
            public string EmbedUrl { get; set; }
            public string WebUrl { get; set; }

            public IReports Report { get; set; }
            public string AccessToken { get; set; }


        
    }
}