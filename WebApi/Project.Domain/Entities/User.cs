using Project.Domain.Enums;

namespace Project.Domain.Entities
{
    public sealed class User
    {
        public int Id { get; private set; }

        public string UserName { get; private set; } = string.Empty;
        public string FullName { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;

        public UserRole Role { get; private set; }
        public bool IsActive { get; private set; }

        private User() { } // EF Core / Dapper

        public User(
            string userName,
            string fullName,
            string passwordHash,
            UserRole role)
        {
            UserName = userName;
            FullName = fullName;
            PasswordHash = passwordHash;
            Role = role;
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void ChangeRole(UserRole role)
        {
            Role = role;
        }
    }
}
