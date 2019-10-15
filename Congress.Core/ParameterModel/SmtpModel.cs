namespace Congress.Core.ParameterModel
{
    public class SmtpModel
    {
        public string smtp_host { get; set; }
        public int smtp_port { get; set; }
        public string smtp_sender { get; set; }
        public string smtp_password { get; set; }
    }
}
