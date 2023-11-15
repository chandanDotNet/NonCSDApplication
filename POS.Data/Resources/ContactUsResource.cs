namespace POS.Data.Resources
{
    public class ContactUsResource : ResourceParameter
    {
        public ContactUsResource() : base("Name")
        {
        }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

    }
}
