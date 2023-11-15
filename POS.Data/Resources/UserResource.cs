namespace POS.Data.Resources
{
    public class UserResource : ResourceParameter
    {
        public UserResource() : base("Email")
        {
        }

        public string Name { get; set; }
    }
}
