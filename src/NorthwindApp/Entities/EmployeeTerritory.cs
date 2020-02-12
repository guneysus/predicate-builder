namespace Northwind.Domain.Entities
{
    public class EmployeeTerritory
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public string TerritoryId { get; set; }

        public Employee Employee { get; set; }
        public Territory Territory { get; set; }
    }
}
