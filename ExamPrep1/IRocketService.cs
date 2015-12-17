using System.Runtime.Serialization;
using System.ServiceModel;

namespace ExamPrep1
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IRocketService
    {
        [FaultContract(typeof(NoAvailableLaunchpadFault))]

        [OperationContract(IsInitiating = true, IsTerminating = false)]
        void NewRocket(Rocket rocket);

        [OperationContract(IsInitiating = true, IsTerminating = false)]
        Rocket AccessExistingRocket(int launchpadId);

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        void AddCargo(Cargo cargo);

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        void RemoveCargo(Cargo cargo);

        [OperationContract(IsInitiating = false, IsTerminating = true)]
        void LaunchActiveRocket(Location target);

        [FaultContract(typeof(NoAvailableHangerFault))]
        [OperationContract(IsInitiating = false, IsTerminating = true)]
        void RemoveActiveRocket();
    }

    [DataContract]
    internal class NoAvailableLaunchpadFault
    {
        [DataMember]
        public string Message = "There are no available launchpads to place your spacecraft.";
    }

    [DataContract]
    internal class NoAvailableHangerFault
    {
        [DataMember]
        public string Message = "There are no available hangers to store your spacecraft.";
    }

    [DataContract]
    public class Rocket
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Location Location { get; set; }
    }

    [DataContract]
    public class Location
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Terestrial { get; set; }
    }

    [DataContract]
    public class Cargo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
    }
}
