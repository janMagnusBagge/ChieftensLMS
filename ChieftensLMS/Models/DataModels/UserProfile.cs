using Microsoft.AspNet.Identity.EntityFramework;
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
	}

}