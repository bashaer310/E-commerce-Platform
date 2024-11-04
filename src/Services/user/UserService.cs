using AutoMapper;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Repository;
using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.UserDTO;
using static Backend_Teamwork.src.Entities.User;

namespace Backend_Teamwork.src.Services.user
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(
            UserRepository UserRepository,
            IMapper mapper,
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            _userRepository = UserRepository;
            _mapper = mapper;
        }

        public async Task<List<UserReadDto>> GetAllAsync(PaginationOptions paginationOptions)
        {
            // Validate pagination options
            if (paginationOptions.PageSize <= 0)
            {
                throw CustomException.BadRequest("PageSize should be greater than 0.");
            }

            if (paginationOptions.PageNumber <= 0)
            {
                throw CustomException.BadRequest("PageNumber should be greater than 0.");
            }

            var UserList = await _userRepository.GetAllAsync(paginationOptions);
            if (UserList == null)
            {
                throw CustomException.NotFound($"Users not found");
            }
            return _mapper.Map<List<User>, List<UserReadDto>>(UserList);
        }

        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _userRepository.GetCountAsync();
        }

        public async Task<UserReadDto> CreateOneAsync(UserCreateDto createDto, UserRole userRole)
        {
            var isExisted = await _userRepository.GetByEmailAsync(createDto.Email);
            if (isExisted != null)
            {
                throw CustomException.BadRequest("Email is already in use");
            }
            PasswordUtils.HashPassword(
                createDto.Password,
                out string hashedPassword,
                out byte[] salt
            );
            var user = _mapper.Map<UserCreateDto, User>(createDto);
            user.Password = hashedPassword;
            user.Salt = salt;
            user.Role = userRole;

            var createdUser = await _userRepository.CreateOneAsync(user);
            return _mapper.Map<User, UserReadDto>(createdUser);
        }

        public async Task<UserReadDto> GetByIdAsync(Guid userId)
        {
            var foundUser = await _userRepository.GetByIdAsync(userId);
            if (foundUser == null)
            {
                throw CustomException.NotFound($"User with id: {userId} not found");
            }
            return _mapper.Map<User, UserReadDto>(foundUser);
        }

        public async Task DeleteOneAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw CustomException.BadRequest("Invalid user ID");
            }
            var foundUser = await _userRepository.GetByIdAsync(id);
            if (foundUser == null)
            {
                throw CustomException.NotFound($"User with ID {id} not found.");
            }
            await _userRepository.DeleteOneAsync(foundUser);
        }

        public async Task<UserReadDto> UpdateOneAsync(Guid id, UserUpdateDto updateDto)
        {
            if (id == Guid.Empty)
            {
                throw CustomException.BadRequest("Invalid user ID");
            }
            if (updateDto == null)
            {
                throw CustomException.BadRequest("Update data cannot be null");
            }

            var foundUser = await _userRepository.GetByIdAsync(id);
            if (foundUser == null)
            {
                throw CustomException.NotFound($"User with ID {id} not found.");
            }

            // Map the update DTO to the existing User entity
            _mapper.Map(updateDto, foundUser);

            // Hash password before saving to the database if it's provided
            if (!string.IsNullOrEmpty(updateDto.Password))
            {
                PasswordUtils.HashPassword(
                    updateDto.Password,
                    out string hashedPassword,
                    out byte[] salt
                );
                foundUser.Password = hashedPassword;
                foundUser.Salt = salt;
            }

            var updatedUser = await _userRepository.UpdateOneAsync(foundUser);
            return _mapper.Map<User, UserReadDto>(updatedUser);
        }

        public async Task<UserReadDto> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw CustomException.BadRequest("Email is required");
            }
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw CustomException.NotFound($"User with email {email} not found.");
            }
            return _mapper.Map<User, UserReadDto>(user);
        }

        public async Task<UserReadDto> GetByPhoneNumberAsync(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw CustomException.BadRequest("Phone Number is required");
            }

            var user = await _userRepository.GetByPhoneNumberAsync(phoneNumber);
            if (user == null)
            {
                throw CustomException.NotFound("User not found.");
            }
            return _mapper.Map<User, UserReadDto>(user);
            ;
        }

        public async Task<string> SignInAsync(UserSigninDto signinDto)
        {
            var foundUser = await _userRepository.GetByEmailAsync(signinDto.Email);
            if (foundUser == null)
            {
                throw CustomException.NotFound($"User with E-mail: {signinDto.Email} not found.");
            }

            // Verify the password
            bool isMatched = PasswordUtils.VerifyPassword(
                signinDto.Password,
                foundUser.Password,
                foundUser.Salt
            );

            if (!isMatched)
            {
                throw CustomException.UnAuthorized($"Unauthorized access.");
            }

            var TokenUtil = new TokenUtils(_configuration);
            var token = TokenUtil.GenerateToken(foundUser);

            if (string.IsNullOrEmpty(token))
            {
                throw CustomException.UnAuthorized("Failed to generate token.");
            }

            return token;
        }
    }
}
