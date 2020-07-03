namespace PaymentGateway.ReadModel.Denormalizer.MongoDb
{
    public class MongoSettings
    {
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }
}