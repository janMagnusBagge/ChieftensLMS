using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChieftensLMS.Models
{
	public class SharedFile
	{
		[Key]
		public int ID { get; set; }
		public string Name { get; set; }
		public DateTime Date { get; set; }
		public string FileName { get; set; }

		[ForeignKey("UserId")]
		public virtual ApplicationUser User { get; set; }

		[StringLength(128)]
		public string UserId { get; set; }

		[ForeignKey("CourseId")]
		public Course Course { get; set;}
		public int CourseId { get; set; }
	}
}