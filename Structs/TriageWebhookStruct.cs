using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Structs
{
    public class TriageWebhookStruct
    {
        public long idHealthUnit { get; set; }
        public string patientName { get; set; }
        public string patientMotherName { get; set; }
        public string patientCpf { get; set; }
        public string patientCns { get; set; }
        public DateTime? patientBirthDate { get; set; }

        public long idEpisode { get; set; }

        public int idGravity { get; set; }

        public float? temperature { get; set; }

        public int? respiratoryFrequency { get; set; }

        public bool? isolation { get; set; }

        public int? heartRate { get; set; }

        public bool? heartRateRegular { get; set; }

        public int? glucose { get; set; }

        public int? bloodPressureDiastole { get; set; }

        public int? bloodPressureSystole { get; set; }
        public long? idPatient { get; set; }
        public int? saturation { get; set; }

        public string userClassificationName { get; set; }

        public string userClassificationCNS { get; set; }

        public string gravityName { get; set; }

        public int? idPain { get; set; }

        public string painColorCode { get; set; }

        public long idUserClassification { get; set; }

        public string gravityColor { get; set; }

        public string gravityColorCode { get; set; }

        public string complaint { get; set; }

        public string flowchart { get; set; }

        public long? idFlowchart { get; set; }

        public string discriminator { get; set; }

        public long? idDiscriminator { get; set; }

        public int? score { get; set; }

        public int? glasgow { get; set; }

        public int? glasgowMotor { get; set; }

        public int? glasgowVerbal { get; set; }

        public int? glasgowEye { get; set; }

        public string justification { get; set; }

        public bool? atmosphericAir { get; set; }

        public bool? dpoc { get; set; }

        public bool? capillaryFillingTime { get; set; }

        public bool? fallRisk { get; set; }

        public int? idSuspicion { get; set; }

        public string diseaseHistory { get; set; }

        public float? height { get; set; }

        public float? weight { get; set; }

        public DateTime dateTimeInclusion { get; set; }

        public DateTime? startClassification { get; set; }

        public DateTime? endClassification { get; set; }

        public long? idRoomAttendance { get; set; }

        public int? idForward { get; set; }

        public string forwardName { get; set; }



        //Ainda não foi implementado
        public string cnesHealthUnit { get; set; }
        public long idTriage { get; set; }
        public string userClassificationCBO { get; set; }
        public string userClassificationCouncil { get; set; }
        public string ticketName { get; set; }
        public string allergy { get; set; }
        public string patientGender { get; set; }
        public string healthUnitUF { get; set; }
        public long waitingTime { get; set; }

        public string observations { get; set; }

        public int? idArrivalReason { get; set; }

        public int? idOrigin { get; set; }
        public int? idArrivalType { get; set; }
        public string userClassificationCPF { get; set; }

        public List<long> listIdPriority { get; set; }

        public bool hasVitalSigns()
        {
            return weight.HasValue ||
                height.HasValue ||
                temperature.HasValue ||
                heartRate.HasValue ||
                bloodPressureSystole.HasValue ||
                bloodPressureDiastole.HasValue ||
                respiratoryFrequency.HasValue ||
                saturation.HasValue ||
                glucose.HasValue ||
                idPain.HasValue;
        }
    }
}
