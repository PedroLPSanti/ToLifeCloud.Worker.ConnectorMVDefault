using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Requests
{
    public class PostEpisodeRequest
    {
        public int idFlow { get; set; }

        public int? idOrigin { get; set; }

        public int? idArrivalType { get; set; }

        public string observation { get; set; }

        public string companionName { get; set; }

        public string companionCpf { get; set; }

        public string companionPhone { get; set; }

        public EpisodeTicket episodeTicket { get; set; }

        public Patient patient { get; set; }

        public UnidentifiedPatient unidentifiedPatient { get; set; }

        public List<int> listPriority { get; set; }
    }

    public class EpisodeTicket
    {
        public string ticketInitials { get; set; }

        public int ticketSequence { get; set; }
    }

    public class UnidentifiedPatient
    {
        public string unidentifiedPatientDescription { get; set; }
        
    }

    public class Patient
    {
        public int? idPatient { get; set; }

        public string patientName { get; set; }

        public string socialName { get; set; }

        public string cpf { get; set; }

        public DateTime? birthDate { get; set; }

        public string cns { get; set; }

        public string rg { get; set; }

        public string expeditionIssuer { get; set; }

        public DateTime? expeditionDate { get; set; }

        public string motherName { get; set; }

        public string fatherName { get; set; }

        public int? idRace { get; set; }

        public int? idEducation { get; set; }

        public string profession { get; set; }

        public int? idMaritalStatus { get; set; }

        public int? idGender { get; set; }

        public string zipCode { get; set; }

        public string street { get; set; }

        public string neighborhood { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string country { get; set; }

        public int? houseNumber { get; set; }

        public string apartmentNumber { get; set; }

        public string healthInsuranceRegistration { get; set; }

        public DateTime? healthInsuranceExpirationDate { get; set; }

        public string healthInsuranceSecurityCode { get; set; }

        public string healthInsuranceHolderName { get; set; }

        public DateTime? healthInsuranceGracePeriod { get; set; }

        public string healthInsuranceProcedure { get; set; }

        public string healthInsuranceAuthorization { get; set; }

        public string birthCountry { get; set; }

        public string birthState { get; set; }

        public string birthCity { get; set; }

        public string phone1 { get; set; }

        public string phone2 { get; set; }
    }
}
