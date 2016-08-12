using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChieftensLMS.Models
{
	public class Course
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }

		public ICollection<Lecture> Lectures { get; set; }
		public ICollection<Assignment> Assignments { get; set; }
		public ICollection<SharedFile> SharedFiles { get; set; }

		public ICollection<ApplicationUser> Users { get; set; }

		
	}

}