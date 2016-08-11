using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChieftensLMS.Models
{
	public class Assignment
	{
		[Key]
		public int ID { get; set; }
		public string Name { get; set; }
		public DateTime Date { get; set; }
		public string Description { get; set; }
		public DateTime ExpirationDate { get; set; }

		public ICollection<TurnIn> TurnIns { get; set; }

	}
}