using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChieftensLMS.Models
{
	public class Lecture
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime Date { get; set; }
		public string Description { get; set; }
		public int TimeInMin { get; set; }

		[ForeignKey("CourseId")]
		public Course Course { get; set; }
		public int CourseId { get; set; }
	}
}