using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChieftensLMS.Models
{
	public class TurnIn
	{
		[Key]
		public int ID { get; set; }
		public string Name { get; set; }
		public DateTime Date { get; set; }
		public string FileName { get; set; }

		[ForeignKey("UserId")]
		public ApplicationUser User { get; set; }

		[StringLength(128), Required]
		public string UserId { get; set; }

		[ForeignKey("AssignmentId")]
		public Assignment Assignment { get; set; }

		public int AssignmentId { get; set; }
	}
}