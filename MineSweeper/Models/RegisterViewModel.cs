﻿namespace RegisterAndLoginAPP.Models
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public RegisterViewModel()
        {
            Username = "";
            Password = "";
        }
    }
}
