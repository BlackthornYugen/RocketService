using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;

namespace ExamPrep1
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class RocketService : IRocketService
    {
        int nextRocketId = 1;
        int nextLocationId = 1;
        Object[] launchpads = new Object[10];
        Object[] hangers = new Object[10];
        Dictionary<string, Rocket> activeRocketBySessionId = new Dictionary<string, Rocket>();

        public Rocket NewRocket(Rocket rocket)
        {
            for (int i = 0; i < launchpads.Length; i++)
            {
                if (launchpads[i] == null)
                {
                    launchpads[i] = rocket;
                    activeRocketBySessionId[OperationContext.Current.SessionId] = rocket;
                    if (rocket.Location == null) rocket.Location = new Location { Id = "LOC" + nextLocationId++ };
                    rocket.Location.Name = String.Format("Launch pad {0}", i + 1);
                    rocket.Location.Terestrial = true;
                    if (rocket.Id == null) rocket.Id = "RCKT" + nextRocketId++;
                    return rocket;
                }
            }
            var fault = new NoAvailableLaunchpadFault();
            throw new FaultException<NoAvailableLaunchpadFault>(fault, fault.Message);
        }

        public Rocket AccessExistingRocket(int launchpadId)
        {
            if (launchpads[launchpadId] == null)
            {
                throw new FaultException(String.Format("Hanger {0} did not have anything on it!", launchpadId));
            } 

            Rocket rocket = launchpads[launchpadId] as Rocket;
            if (rocket == null)
            {
                throw new FaultException(String.Format("Hanger {0} did not have a rocket on it.", launchpadId));
            }

            activeRocketBySessionId[OperationContext.Current.SessionId] = rocket;
            return rocket;
        }

        public void AddCargo(Cargo cargo)
        {
            throw new NotImplementedException();
        }

        public void LaunchActiveRocket(Location target)
        {
            using (LogToFile())
            {
                Rocket rocket = activeRocketBySessionId[OperationContext.Current.SessionId];
                if (rocket == null)
                {
                    throw new FaultException(String.Format("No active rocket to send to {0}.", target.Name ?? "place"));
                }
                for (int i = 0; i < launchpads.Length; i++)
                {
                    if (launchpads[i] == rocket)
                    {
                        launchpads[i] = null;
                        activeRocketBySessionId[OperationContext.Current.SessionId] = null;
                        Console.WriteLine(GetTimestamp() + "Rocket is blasting off to {0}.", target.Name ?? "place");
                        return;
                    }
                }
                Console.WriteLine(GetTimestamp() + "Rocket was not found on any launchpad.");
            }
        }

        public void RemoveActiveRocket()
        {
            throw new NotImplementedException();
        }

        public void RemoveCargo(Cargo cargo)
        {
            throw new NotImplementedException();
        }

        IDisposable LogToFile()
        {
            FileStream fs = new FileStream(Path.GetTempPath() + "RocketServ_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);
            return sw;
        }

        string GetTimestamp()
        {
            return DateTime.Now.ToString("hh:mm:ss\t");
        }
    }
}
