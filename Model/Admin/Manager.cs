namespace ConvenienceStore.Model.Admin
{
    public class Manager
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
