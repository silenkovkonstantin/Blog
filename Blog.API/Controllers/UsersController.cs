using AutoMapper;
using Blog.Data.Models.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Blog.Data;
using Blog.Data.Repository;
using BlogAPI.Contracts.Models.Users;
using NuGet.Common;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private IRepository<Post> _postsRepository;
        private IRepository<Comment> _commentRepository;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager,
            IMapper mapper, IRepository<Post> postsRepository, IRepository<Comment> commentRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _postsRepository = postsRepository;
            _commentRepository = commentRepository;
        }

        /// <summary>
        /// Вход в аккунт пользователя
        /// </summary>
        /// <remarks>
        /// POST /Users/Login
        /// </remarks>
        /// <param name="model">LoginModel object</param>
        /// <returns>Logining in account</returns>
        /// <response code="200">Success</response>
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Просмотр списка пользователей
        /// </summary>
        /// <remarks>
        /// GET /Users
        /// </remarks>
        /// <returns>Returns All Users</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpGet]
        [Route("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Users()
        {
            var users = _userManager.Users;

            var request = new GetUsersResponse
            {
                UserAmount = users.Count(),
                Users = _mapper.Map<IQueryable<User>, List<UserView>>(users)
            };

            return StatusCode(200, request);
        }

        /// <summary>
        /// Просмотр пользователя по id
        /// </summary>
        /// <remarks>
        /// POST /Users/User/id
        /// </remarks>
        /// <param name="id">User id (string)</param>
        /// <returns>Возвращает запрашиваемого пользователя</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> User([FromRoute] string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return StatusCode(400, $"Ошибка: Пользователь с данным id не найден. Проверьте корректность ввода!");
            }

            //var userPosts = await _postsRepository.GetAllByUserIdAsync(id);
            //user.Posts = userPosts.ToList();
            //var userComments = await _commentRepository.GetAllByUserIdAsync(id);
            //user.Comments = userComments.ToList();

            var response = _mapper.Map<UserView>(user);

            return StatusCode(200, response);
        }

        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        /// <remarks>
        /// POST /Users/Add
        /// </remarks>
        /// <param name="request">AddUserModel object</param>
        /// <returns>Добавляет нового пользователя</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Add([FromBody] AddUserRequest request)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(request);

                var result = await _userManager.CreateAsync(user, request.PasswordReg);
                if (result.Succeeded)
                {
                    // Установка куки
                    await _userManager.AddToRoleAsync(user, "Пользователь");
                    await _signInManager.SignInAsync(user, false);

                    return StatusCode(200, $"Пользователь: {request.FirstName} {request.LastName}, добавлен!");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return StatusCode(400, $"Ошибка: Не удалось добавить пользователя {request.FirstName} {request.LastName}!");
                }
            }

            return StatusCode(400, $"Ошибка: Некорректный запрос!");
        }

        /// <summary>
        /// Редактирование информации о пользователе
        /// </summary>
        /// <remarks>
        /// PATCH /Users/Edit/id
        /// </remarks>
        /// <param name="id">User id (string)</param>      
        /// <param name="request">UserEditRequest object</param>
        /// <returns>Возвращает отредактированного пользователя</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPatch]
        [Route("[action]/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromBody] EditUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return StatusCode(400, $"Ошибка: Пользователь: {request.Email} не зарегестрирован. Сначало пройдите регистрацию!");
            }

            user = _mapper.Map<EditUserRequest, User>(request, user);
            var rolesList = request.Roles;
            await _userManager.AddToRolesAsync(user, request.Roles.Where(r => r.IsChecked == true).Select(r => r.Name));
            var result = await _userManager.UpdateAsync(user);

            return StatusCode(200, $"Данные пользователя: {request.FirstName} {request.LastName}, обновлены!");
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <remarks>
        /// DELETE /Users/Delete/id
        /// </remarks>
        /// <param name="id">User id (string)</param>      
        /// <returns>Возвращает: NoContent</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpDelete]
        [Route("[action]/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return StatusCode(400, $"Ошибка: Пользователь с идентификатором: {id} не существует!");
            }
                
            await _userManager.DeleteAsync(user);

            return StatusCode(200, $"Пользователь: \"{user.Email}\", с идентификатором: {user.Id} удален!");
        }

        //[HttpGet]
        //public async Task<IActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();
            
        //    return Ok();
        //}
    }
}
