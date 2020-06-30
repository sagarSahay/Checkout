namespace PaymentGateway.WriteModel.Application.Configuration
{
    public class QueueSettings
    {
        public string HostName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ReceiveQueueName { get; set; }
    }
}
