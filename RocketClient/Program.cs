using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxy = new RocketService.RocketServiceClient();

            var Rocket = proxy.NewRocket(new RocketService.Rocket
            {
                Name = "Enterprize"
            });

            proxy.LaunchActiveRocket(new RocketService.Location { Name = "Moon", Terestrial = false });
        }
    }
}
