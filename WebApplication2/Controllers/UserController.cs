using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;
using UMS.BL.Helpers;
using UMS.DAL.Interfaces;
using UMS.DAL.Models;
using UMS.DAL.Services;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly BaseRepository<Role> _roleRepo;
        private readonly BaseRepository<State> _stateRepo;
        private readonly BaseRepository<User> _userRepo;

        public UserController(ApplicationDbContext Context, IUserService userService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;

            _roleRepo = new BaseRepository<Role>(Context);
            _stateRepo = new BaseRepository<State>(Context);
            _userRepo = new BaseRepository<User>(Context);
        }

        // GET: User/Index
        public IActionResult Index()
        {
            var users = _userRepo.ListAllAsync();
            var userViewModels = _mapper.Map<List<UserViewModel>>(users);
            return View(users);
        }

        // GET: User/Dashboard
        public IActionResult Dashboard()
        {
            // Logic for displaying user dashboard
            return View();
        }

        // GET: User/Create
        public async Task<IActionResult> Create()
        {
            // fetch list of state and role
            var stateEntity = await _stateRepo.ListIEnumerableAllAsync();
            var roleEntity = await _roleRepo.ListIEnumerableAllAsync();

            var model = new UserViewModel
            {
                States = stateEntity.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList(),

                Hobbies = GetHobbies(),

                Roles = roleEntity.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.RoleName
                }).ToList()
            };

            return View(model);
        }

        // GET Districts
        [HttpGet("GetDistrictsByStateIdAsync")]
        public async Task<JsonResult> GetDistrictsByStateIdAsync(int stateId)
        {
            var districts = await _userService.GetDistrictsByStateIdAsync(stateId);
            var districtSelectList = districts.Select(d => new
            {
                value = d.Id,
                text = d.Name
            });

            return Json(districtSelectList);
        }

        // GET Talukas
        [HttpGet("User/GetTalukasByDistrictIdAsync")]
        public async Task<IActionResult> GetTalukasByDistrictIdAsync(int districtId)
        {
            var talukas = await _userService.GetTalukasByDistrictIdAsync(districtId);
            var talukaSelectList = talukas.Select(t => new
            {
                value = t.Id,
                text = t.Name
            });

            return Json(talukaSelectList);
        }

        // GET Hobbiess
        private List<SelectListItem> GetHobbies()
        {
            return new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Reading" },
            new SelectListItem { Value = "2", Text = "Sports" },
            new SelectListItem { Value = "3", Text = "Music" },
            new SelectListItem { Value = "4", Text = "Traveling" },
            new SelectListItem { Value = "5", Text = "Cooking" },
            new SelectListItem { Value = "6", Text = "Gaming" }
        };
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            // Check if the email already exists in the database
            var existingUser = await _userService.FindUserByEmail(model.Email);
            model.Hobbies = GetHobbies();
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                // Handle profile photo upload
                if (model.ProfilePhoto != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfilePhoto.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfilePhoto.CopyToAsync(fileStream);
                    }

                    // Save the file path to the PhotoPath property
                    model.PhotoPath = "/uploads/" + uniqueFileName;
                }


                // Map selected Hobbies
                var selectedHobbiesString = model.SelectedHobbies != null
                ? string.Join(",", model.SelectedHobbies)
                : null;

                // Map the ViewModel to the User entity and save to the database
                User user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    PhotoPath = model.PhotoPath, // Storing the uploaded file path
                    StateId = model.StateId,
                    DistrictId = model.DistrictId,
                    TalukaId = model.TalukaId,
                    Hobbies = selectedHobbiesString,
                    DateOfBirth = model.DateOfBirth,
                    RoleId = 2,
                    IsApproved = false,
                    IsPasswordChanged = false,
                    //PasswordHash = HashedPasswordHelper.HashPassword("000000")
                    //PasswordHash = HashedPassword.GenerateRandomPassword()
                    //PasswordHash = "000000"
                    //PasswordHash = HashPassword(model.Password),
                    //Hobbies = string.Join(",", model.SelectedHobbies ?? new List<int>()),
                };

                if (user != null)
                {
                    await _userService.addUser(user);
                    return RedirectToAction("Login", "Auth");
                }
            }

            // fetch list of state and role
            var stateEntity = await _stateRepo.ListIEnumerableAllAsync();
            var roleEntity = await _roleRepo.ListIEnumerableAllAsync();

            // Repopulate dropdowns if the model is invalid
            model.States = stateEntity.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();

            model.Hobbies = GetHobbies();

            model.Roles = roleEntity.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.RoleName
            }).ToList();

            return View(model);
        }

        // Generate Random Password
        //public static string GenerateRandomPassword()
        //{
        //    Random random = new Random();
        //    int randomNumber = random.Next(100000, 999999);
        //    //Console.WriteLine("Password: ", randomNumber.ToString());
        //    var password = randomNumber.ToString();
        //    return HashPassword(password);
        //}

        // password hashing logic
        //private static string HashPassword(string password)
        //{
        //    // Generate a 128-bit salt using a sequence of
        //    // cryptographically strong random bytes.
        //    byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes

        //    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //        password: password!,
        //        salt: salt,
        //        prf: KeyDerivationPrf.HMACSHA256,
        //        iterationCount: 100000,
        //        numBytesRequested: 256 / 8));

        //    return hashed;
        //}

    }
}
