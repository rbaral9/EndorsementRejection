using System.ComponentModel.DataAnnotations;

namespace EndorsementRejection.Models.Entities
{
    public class EndoRejection
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Requested By is Mandatory")]
        public string RequestedBy { get; set; }

        public string PolicyNumber { get; set; }
        public string? PolicyHolder { get; set; }
        public string? ProcessedType { get; set; }
        public string? RejectionReason { get; set; }
        public string? ApprovalStatus { get; set; }
        public string? ApprovedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime?  RequestedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovalComments { get; set; }
        public string? completedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? completedDate { get; set; }
        public string? RejectionLetterComments { get; set; }

        public EndoRejection()
        {
            RequestedDate = DateTime.Now;
            //ApprovedDate = DateTime.Now;
        }







    }
}
