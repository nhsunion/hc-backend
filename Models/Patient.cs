﻿using System.ComponentModel.DataAnnotations;

namespace hc_backend.Models
{
    public class Patient
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Titel { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Address { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
