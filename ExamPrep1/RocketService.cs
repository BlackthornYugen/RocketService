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
            throw new FaultException<NoAvailableLaunchpadFault>(new NoAvailableLaunchpadFault());
        }

        public Rocket AccessExistingRocket(int launchpadId)
        {
            throw new NotImplementedException();
        }

        public void AddCargo(Cargo cargo)
        {
            throw new NotImplementedException();
        }

        public void LaunchActiveRocket(Location target)
        {
            throw new NotImplementedException();
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
