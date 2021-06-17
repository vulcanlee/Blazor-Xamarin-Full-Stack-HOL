using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AC05.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration configuration;

        public string MyEnglishName { get; set; }
        public string MyChineseName { get; set; }
        public bool IsNETDeveloper { get; set; }
        public int NETDeveloperYear { get; set; }
        public string ProjectName { get; set; }
        public List<string> AllConfigurationProvider { get; set; } = new();
        public IndexModel(ILogger<IndexModel> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }

        public void OnGet()
        {
            RetriveAllConfigurationProvider();
            RetriveConfigurationOneByOne();
        }
        public void RetriveConfigurationOneByOne()
        {
            MyEnglishName = configuration ["MyEnglishName"];
            MyChineseName = configuration ["MyChineseName"];
            IsNETDeveloper = configuration.GetValue<bool>("IsNETDeveloper");
            NETDeveloperYear = configuration.GetValue<int>("NETDeveloperYear");
            ProjectName = configuration["Position:ProjectName"];
        }
        public void RetriveAllConfigurationProvider()
        {
            IConfigurationRoot ConfigRoot = (IConfigurationRoot)configuration;
            foreach (var provider in ConfigRoot.Providers.ToList())
            {
                AllConfigurationProvider.Add(provider.ToString());
            }
        }
    }
}
