using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChieftensLMS.Models.DTOModels
{
	public class UserRolesDTO
	{
		public string Name { get; set; }
		public string SurName { get; set; }
		public IEnumerable<string> Roles { get; set; }
	}




}