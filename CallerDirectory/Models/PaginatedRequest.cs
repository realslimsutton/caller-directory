namespace CallerDirectory.Models
{
    public class PaginatedRequest
    {
        public int Page { get; set; } = 1;

        public int PerPage { get; set; } = 10;

        public string? SortColumn { get; set; }

        public string SortDirection { get; set; } = "asc";

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public int GetSkip()
        {
            if (this.Page < 1)
            {
                this.Page = 1;
            }

            if (this.PerPage < 1)
            {
                this.PerPage = 10;
            }

            return (this.Page - 1) * this.PerPage;
        }
    }
}
