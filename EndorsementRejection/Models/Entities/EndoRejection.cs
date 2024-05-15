using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EndorsementRejection.Models.Entities
{
    public class EndoRejection
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Requested By is Mandatory")]
        public string RequestedBy { get; set; }
        [Required(ErrorMessage = "Policy No is Mandatory")]
        public string PolicyNumber { get; set; }
        [Required(ErrorMessage = "Policy Holder is Mandatory")]
        public string PolicyHolder { get; set; }
        [Required(ErrorMessage = "Processed Type is Mandatory")]
        public string ProcessedType { get; set; }

        public string? EndoProcessed { get; set; }
        [Required(ErrorMessage = "Rejection Reason is Mandatory")]
        public string RejectionReason { get; set; }

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

        [NotMapped]
        public string? ReferralUrl { get; set; }




    }
}
