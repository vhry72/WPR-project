namespace WPR_project.DTO_s
{
    public class HuurVerzoekDTO
    {
        public Guid HuurderID { get; set; }
        public DateTime beginDate { get; set; }
        public DateTime endDate { get; set; }

        public bool approved { get; set; }


    }
}
