namespace TaskManagerUI.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string JwtToken { get; set; }

        public ICollection<UserTaskViewModel>? UserTasks { get; set; }
    }
}
