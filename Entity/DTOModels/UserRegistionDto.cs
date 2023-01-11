using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOModels
{
    /// <summary>
    /// User Registration model
    /// </summary>
    public class UserRegistionDto
    {
        /// <summary>
        /// Username is Unique for each user
        /// </summary>
        [Required]
        [MaxLength(25, ErrorMessage = "limit Exceeded")]
        public string UserName { get; set; }

        /// <summary>
        /// Firstname or Surname
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "limit Exceeded")]
        public string FirstName { get; set; }
        /// <summary>
        /// User second name
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "limit Exceeded")]
        public string LastName { get; set; }
        /// <summary>
        /// Should include special characters
        /// </summary>
        [Required]
        [MinLength(8, ErrorMessage = "At least 8 characters")]
        public string Password { get; set; }
        /// <summary>
        /// Should include .com 
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "limit Exceeded")]
        public string Email { get; set; }
    }
}
