using System;
namespace MyAPI.Models
{
	public class CodeModel
	{
        public string Value { get; set; } = null!;
        public DateTime ExpiredAt;
        public UserModel User = null!;
    }
}

