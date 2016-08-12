using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChieftensLMS.Models
{
	public class UserProfile
	{
		[Key, StringLength(128)]
		public string Id { get; set; }
		public string Name { get; set; }
		public string SurName { get; set; }

		public ICollection<Course> Courses { get; set; }

		[ForeignKey("Id")]
		// This should not show in the database, nor should it be used. It's here just so that we can put a foreign constrait
		protected ApplicationUser ApplicationUser { get; set; }
	}

}