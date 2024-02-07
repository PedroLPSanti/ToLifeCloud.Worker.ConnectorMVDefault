namespace ToLifeCloud.Worker.ConnectorMVDefault.Structs
{
    public class EvasionWebhookStruct
    {
        public int idHealthUnit { get; set; }

        public long idEpisode { get; set; }

        public int idUser { get; set; }

        public int idRoom { get; set; }

        public string motive { get; set; }
    }
}
