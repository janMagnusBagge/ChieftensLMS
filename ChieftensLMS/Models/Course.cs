﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChieftensLMS.Models
{
	public class Course
	{
		[Key]
		public int ID { get; set; }
		public string Name { get; set; }

		public virtual ICollection<Lecture> Lectures { get; set; }
		public virtual ICollection<Assignment> Assignment { get; set; }
		public virtual ICollection<SharedFile> SharedFiles { get; set; }

		public virtual ICollection<ApplicationUser> Users { get; set; }

	}

}