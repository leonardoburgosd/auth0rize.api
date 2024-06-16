using auth0rize.auth.domain.Application;
using auth0rize.auth.domain.ApplicationMenu;
using auth0rize.auth.domain.ApplicationUser;
using auth0rize.auth.domain.Domain;
using auth0rize.auth.domain.Menu;
using auth0rize.auth.domain.MenuOption;
using auth0rize.auth.domain.Option;
using auth0rize.auth.domain.TypeUser;
using auth0rize.auth.domain.User;
using System.Security.Cryptography;
using System.Text;

namespace auth0rize.auth.infraestructure.Extensions
{
    public static class LocalData
    {
        public static List<Application> applications = listApplications();
        public static List<Menu> menus = listMenu();
        public static List<Option> options = listOptions();
        public static List<TypeUser> types = listType();
        public static List<User> users = listUser();
        public static List<Domain> domains = listDomain();

        public static List<ApplicationMenu> applicationMenus = listApplicationsMenu();
        public static List<ApplicationDomain> applicationDomain = listApplicationDomain();
        public static List<MenuOption> menuOptions = listMenuOptions();

        private static List<Application> listApplications()
        {
            List<Application> response = new List<Application>();

            response.Add(new Application()
            {
                Id = 1,
                Code = "f9ae88c2-7efb-4d49-bc0b-928c10aebeed",
                IsDeleted = false,
                Description = "Aplicacion para poder acceder a la aplicacion de administracion de archivos.",
                Name = "[Driv3rize]",
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Application()
            {
                Id = 2,
                Code = "6f45e2c1-1d69-4e56-8350-c48727df5a23",
                IsDeleted = false,
                Description = "Aplicacion para poder acceder a la aplicacion de autenticacion.",
                Name = "[WorkfloU]",
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Application()
            {
                Id = 3,
                Code = "a7d3c7f7-c1f3-4f4e-a5ab-ae5dc7f59b52",
                IsDeleted = false,
                Description = "Aplicacion para poder tener acceso a la aplicación de mensajes internos.",
                Name = "[M3ssage]",
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Application()
            {
                Id = 4,
                Code = "8e9a23df-8810-4a0a-ba2e-1abf83dd6b7e",
                IsDeleted = false,
                Description = "Aplicacion para poder tener acceso a la aplicación de proyectos.",
                Name = "[ProjMag]",
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Application()
            {
                Id = 5,
                Code = "3176bb3d-6b0a-4be0-8ac1-1a56a0d5a3bf",
                IsDeleted = false,
                Description = "Aplicacion para poder tener acceso a la aplicación de compartir archivos temporales.",
                Name = "[F4stFile]",
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            return response;
        }

        private static List<Menu> listMenu()
        {
            List<Menu> response = new List<Menu>();

            response.Add(new Menu()
            {
                Id = 1,
                Name = "Menu [Driv3rize] superadmin",
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Menu()
            {
                Id = 2,
                IsDeleted = false,
                Name = "Menu [Driv3rize] user",
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Menu()
            {
                Id = 3,
                IsDeleted = false,
                Name = "Menu [WorkfloU] superadmin",
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Menu()
            {
                Id = 4,
                Name = "Menu [WorkfloU] recursos humanos",
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Menu()
            {
                Id = 5,
                Name = "Menu [WorkfloU] TI",
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Menu()
            {
                Id = 6,
                Name = "Menu [M3ssage]",
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Menu()
            {
                Id = 7,
                Name = "Menu [ProjMag] superadmin",
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Menu()
            {
                Id = 8,
                Name = "Menu [ProjMag] Lider tecnico",
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Menu()
            {
                Id = 9,
                Name = "Menu [ProjMag] Tecnico",
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            return response;
        }

        private static List<Option> listOptions()
        {
            List<Option> response = new List<Option>();

            response.Add(new Option()
            {
                Id = 1,
                Name = "Elemento base 1",
                Icon = "",
                Address = "/dashboard",
                Parent = 0,
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Option()
            {
                Id = 2,
                Name = "Elemento base 1.1",
                Address = "/dashboard/users",
                Icon = "",
                Parent = 1,
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Option()
            {
                Id = 3,
                Name = "Elemento base 1.2",
                Address = "/dashboard/application",
                Icon = "",
                Parent = 1,
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Option()
            {
                Id = 4,
                Name = "Elemento base 2",
                Icon = "/dashboard",
                Address = "",
                Parent = 0,
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Option()
            {
                Id = 5,
                Name = "Elemento base 2.1",
                Icon = "",
                Address = "/dashboard/menu",
                Parent = 5,
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            return response;
        }

        private static List<TypeUser> listType()
        {
            List<TypeUser> response = new List<TypeUser>();

            response.Add(new TypeUser()
            {
                Id = 1,
                Name = "Superadmin",
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new TypeUser()
            {
                Id = 2,
                Name = "Admin",
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new TypeUser()
            {
                Id = 3,
                Name = "Técnico",
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new TypeUser()
            {
                Id = 4,
                Name = "Jefe de Recursos Humanos",
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new TypeUser()
            {
                Id = 5,
                Name = "Jefe de Proyectos",
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new TypeUser()
            {
                Id = 6,
                Name = "Jefe de TI",
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });


            return response;
        }

        private static List<Domain> listDomain()
        {
            List<Domain> response = new List<Domain>();

            response.Add(new Domain()
            {
                Id = 1,
                Name = "1645TIYGWJ",
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Domain()
            {
                Id = 2,
                Name = "ZI8956G46V",
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Domain()
            {
                Id = 3,
                Name = "A3612JQ2AZ",
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Domain()
            {
                Id = 4,
                Name = "YMK6J61374",
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            response.Add(new Domain()
            {
                Id = 5,
                Name = "C469817GE8",
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            return response;
        }
        
        private static List<User> listUser()
        {
            List<User> response = new List<User>();

            (byte[] salt, byte[] hashPassword) code = generateHash("123456");
            response.Add(new User()
            {
                Id = 1,
                Name = "Leonardo",
                LastName = "Burgos",
                MotherLastName = "Díaz",
                Avatar = "default.png",
                Email = "leburgosdiaz@gmail.com",
                UserName = "leburgos",
                Domain = 1,
                Password = code.hashPassword,
                Salt = code.salt,
                IsDoubleFactorActivate = false,
                Type = 1,
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            code = generateHash("g6Pt(*?6:1");
            response.Add(new User()
            {
                Id = 2,
                Name = "Alejandro",
                LastName = "González",
                MotherLastName = "Rodríguez",
                Avatar = "default.png",
                Email = "alejandrogrodriguez@gmail.com",
                UserName = "agrodriguez",
                Domain = 1,
                Password = code.hashPassword,
                Salt = code.salt,
                IsDoubleFactorActivate = false,
                Type = 1,
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            code = generateHash(":wg>J8a2*0");
            response.Add(new User()
            {
                Id = 3,
                Name = "Valentina",
                LastName = "Mendoza",
                MotherLastName = "Sánchez",
                Avatar = "default.png",
                Email = "vmendozasanchez@gmail.com",
                UserName = "vmendonza",
                Domain = 1,
                Password = code.hashPassword,
                Salt = code.salt,
                IsDoubleFactorActivate = false,
                Type = 1,
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            code = generateHash("qXd;118W");
            response.Add(new User()
            {
                Id = 4,
                Name = "Diego",
                LastName = "Herrera",
                MotherLastName = "Flores",
                Avatar = "default.png",
                Email = "dherreraflores@gmail.com",
                UserName = "dherrera",
                Domain = 2,
                Password = code.hashPassword,
                Salt = code.salt,
                IsDoubleFactorActivate = false,
                Type = 1,
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            code = generateHash("wy7G;_%0t1");
            response.Add(new User()
            {
                Id = 5,
                Name = "Camila",
                LastName = "Martínez",
                MotherLastName = "Gómez",
                Avatar = "default.png",
                Email = "cmartinezgomez@gmail.com",
                UserName = "cmartinez",
                Domain = 3,
                Password = code.hashPassword,
                Salt = code.salt,
                IsDoubleFactorActivate = false,
                Type = 1,
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            code = generateHash("$C238~qRJ8");
            response.Add(new User()
            {
                Id = 6,
                Name = "Javier",
                LastName = "Pérez",
                MotherLastName = "López",
                Avatar = "default.png",
                Email = "jperezlopez@gmail.com",
                UserName = "jperez",
                Domain = 4,
                Password = code.hashPassword,
                Salt = code.salt,
                IsDoubleFactorActivate = false,
                Type = 1,
                IsDeleted = false,
                RegistrationDate = DateTime.Now,
                UserRegistration = 1
            });

            return response;
        }
         
        private static List<ApplicationMenu> listApplicationsMenu()
        {
            List<ApplicationMenu> response = new List<ApplicationMenu>();

            //[Driv3rize]
            response.Add(new ApplicationMenu()
            {
                Application = 1,
                Menu = 1
            });

            response.Add(new ApplicationMenu()
            {
                Application = 1,
                Menu = 2
            });

            //[WorkfloU]
            response.Add(new ApplicationMenu()
            {
                Application = 2,
                Menu = 3
            });

            response.Add(new ApplicationMenu()
            {
                Application = 2,
                Menu = 4
            });

            response.Add(new ApplicationMenu()
            {
                Application = 2,
                Menu = 5
            });

            //[ProjMag]
            response.Add(new ApplicationMenu()
            {
                Application = 4,
                Menu = 7
            });

            response.Add(new ApplicationMenu()
            {
                Application = 4,
                Menu = 8
            });

            response.Add(new ApplicationMenu()
            {
                Application = 4,
                Menu = 9
            });

            return response;
        }

        private static List<ApplicationDomain> listApplicationDomain()
        {
            List<ApplicationDomain> response = new List<ApplicationDomain>();

            response.Add(new ApplicationDomain()
            {
                Application = 1,
                Domain = 1
            });

            response.Add(new ApplicationDomain()
            {
                Application = 2,
                Domain = 1
            });

            response.Add(new ApplicationDomain()
            {
                Application = 3,
                Domain = 1
            });

            response.Add(new ApplicationDomain()
            {
                Application = 1,
                Domain = 2
            });

            response.Add(new ApplicationDomain()
            {
                Application = 2,
                Domain = 2
            });

            response.Add(new ApplicationDomain()
            {
                Application = 5,
                Domain = 2
            });

            response.Add(new ApplicationDomain()
            {
                Application = 4,
                Domain = 3
            });

            response.Add(new ApplicationDomain()
            {
                Application = 5,
                Domain = 3
            });

            response.Add(new ApplicationDomain()
            {
                Application = 1,
                Domain = 4
            });

            response.Add(new ApplicationDomain()
            {
                Application = 2,
                Domain = 4
            });

            response.Add(new ApplicationDomain()
            {
                Application = 4,
                Domain = 4
            });

            response.Add(new ApplicationDomain()
            {
                Application = 2,
                Domain = 5
            });

            return response;
        }

        private static List<MenuOption> listMenuOptions()
        {
            List<MenuOption> response = new List<MenuOption>();

            response.Add(new MenuOption()
            {
                Menu = 1,
                Option = 1
            });

            response.Add(new MenuOption()
            {
                Menu = 1,
                Option = 2
            });

            response.Add(new MenuOption()
            {
                Menu = 1,
                Option = 3
            });

            response.Add(new MenuOption()
            {
                Menu = 2,
                Option = 4
            });

            response.Add(new MenuOption()
            {
                Menu = 2,
                Option = 5
            });

            return response;
        }

        public static (byte[] salt, byte[] hashPassword) generateHash(string password)
        {
            using (HMACSHA512 sha512 = new HMACSHA512())
            {
                byte[] salt = sha512.Key;
                byte[] hashPassword = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));
                return (salt, hashPassword);
            }
        }

    }
}