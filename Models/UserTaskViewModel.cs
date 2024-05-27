namespace TaskManagerUI.Models
{
    public class UserTaskViewModel
    {
        public int UserId { get; set; }
        public UserViewModel? User { get; set; }

        public int TaskId { get; set; }
        public Task? Task { get; set; }

        public DateTime AssignedDate { get; set; }
    }
}
