using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public UserDto Login(LoginDto login)
        {
            User newUser = _context.Users!.FirstOrDefault(user => user.Email == login.Email && user.Password == login.Password)!;

            if (newUser == null) return null!;
            return new UserDto()
            {
                UserId = newUser.UserId,
                Name = newUser.Name!,
                Email = newUser.Email!,
                UserType = newUser.UserType!,
            };
        }
        public UserDto Add(UserDtoInsert user)
        {
            var email = GetUserByEmail(user.Email!);
            if (email != null) return null;

            var newUser = _context.Users.Add(new User()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = "client",
            });

            _context.SaveChanges();

            return new UserDto()
            {
                UserId = newUser.Entity.UserId,
                Name = newUser.Entity.Name,
                Email = newUser.Entity.Email,
                UserType = newUser.Entity.UserType!,
            };
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            var user = _context.Users.FirstOrDefault(users => users.Email.Equals(userEmail));

            if (user == null) return null!;

            return new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                Name = user.Name,
                UserType = user.UserType
            };
        }

        public IEnumerable<UserDto> GetUsers()
        {
            return _context.Users.Select(u => new UserDto
            {
                UserId = u.UserId,
                Email = u.Email,
                Name = u.Name,
                UserType = u.UserType
            });
        }

    }
}
