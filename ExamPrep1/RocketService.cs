using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ExamPrep1
{
    public class RocketService : IRocketService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public Rocket GetDataUsingDataContract(Rocket composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
