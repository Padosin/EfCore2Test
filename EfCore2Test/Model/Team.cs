namespace EfCore2Test.Model
{
    public class Team : ISoftDelete, IName
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsDelete { get; set; }
    }
}
