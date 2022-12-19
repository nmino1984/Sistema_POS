namespace POS.Application.ViewModels.Response
{
    public class CategoryResponseViewModel
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? AuditUpdateDate { get; set; }
        public int State { get; set; }
        public string? StateCategory { get; set; }



    }
}
