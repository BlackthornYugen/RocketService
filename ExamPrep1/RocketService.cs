using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ExamPrep1
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class RocketService : IRocketService
    {
        Object[] launchpads = new Object[10];
        Object[] hangers = new Object[10];
        Dictionary<string, Rocket> activeRocketBySessionId = new Dictionary<string, Rocket>();
        
        public void NewRocket(Rocket rocket)
        {
            for(int i = 0; i < launchpads.Length; i++)
            {
                if (launchpads[i] == null)
                {
                    launchpads[i] = rocket;
                    activeRocketBySessionId[OperationContext.Current.SessionId] = rocket;
                    return;
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
                    Console.WriteLine("Rocket is blasting off to {0}.", target.Name ?? "place");
                    return;
                }
            }
            Console.WriteLine("Rocket was not found on any launchpad.");
        }

        public void RemoveActiveRocket()
        {
            throw new NotImplementedException();
        }

        public void RemoveCargo(Cargo cargo)
        {
            throw new NotImplementedException();
        }
    }
}
