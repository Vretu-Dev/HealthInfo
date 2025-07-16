using Exiled.API.Interfaces;
using System.Collections.Generic;

namespace HealthInfo
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public List<string> IgnoredTeams { get; set; } = new List<string>
        {
            "Dead"
        };
    }
}