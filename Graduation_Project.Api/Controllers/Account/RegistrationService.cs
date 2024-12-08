using Graduation_Project.Api.DTO.Account;
using Graduation_Project.Repository.Repository.Interfaces.Clinics;
using Graduation_Project.Repository.Repository.Interfaces.Doctors;
using Graduation_Project.Repository.Repository.Interfaces.Patients;
using Graduation_Project.Repository.Repository.Interfaces.Pharmacies;

namespace Graduation_Project.Api.Controllers.Account
{
    public class RegistrationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IDoctorRepository doctorRepository;
        private readonly IPatientRepository patientRepository;
        private readonly IPharmacistRepository pharmacistRepository;
        private readonly ISecretaryRepository secretaryRepository;

        public RegistrationService(UserManager<ApplicationUser> userManager,
                                    RoleManager<IdentityRole> roleManager,
                                    IDoctorRepository doctorRepository,
                                    IPatientRepository patientRepository,
                                    IPharmacistRepository pharmacistRepository,
                                    ISecretaryRepository secretaryRepository)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.doctorRepository = doctorRepository;
            this.patientRepository = patientRepository;
            this.pharmacistRepository = pharmacistRepository;
            this.secretaryRepository = secretaryRepository;
        }

        public async Task RegisterAsync(RegisterDTO registerDto)
        {
            // ------------  Mapping Data to Doctor ------------
            var user = new ApplicationUser
            {
                Email = registerDto.Email,
                // FullName = registerDto.Fname + ' ' + registerDto.Lname
                UserName = registerDto.Username
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            if (!await roleManager.RoleExistsAsync(registerDto.Role))
            {
                throw new Exception("Role does not exist.");
            }
            await userManager.AddToRoleAsync(user, registerDto.Role);

            // ------------  Mapping Data to Doctor ------------
            switch (registerDto)
            {
                case DoctorDTO doctorDTO:
                    var doctor = new Doctor
                    {
                        Gender = doctorDTO.Gender,
                        ConsultationFees = doctorDTO.ConsultationFees,
                        SpecialtyId = doctorDTO.SpecialtyId,
                        UserId = user.Id
                    };
                    doctorRepository.Add(doctor);
                    doctorRepository.Save();
                    break;

                case PatientDTO patientDTO:
                    var Patient = new Patient
                    {
                        Gender = registerDto.Gender,
                        UserId = user.Id
                    };
                    patientRepository.Add(Patient);
                    patientRepository.Save();
                    break;

                case PharmacistDTO pharmacistDTO:
                    var Pharmacist = new Pharmacist
                    {
                        Gender = registerDto.Gender,
                        UserId = user.Id
                    };
                    pharmacistRepository.Add(Pharmacist);
                    pharmacistRepository.Save();
                    break;

                case SecretaryDTO secretaryDTO:
                    var Secretary = new Secretary
                    {
                        Gender = registerDto.Gender,
                        UserId = user.Id
                    };
                    secretaryRepository.Add(Secretary);
                    secretaryRepository.Save();
                    break;

                default:
                    throw new ArgumentException("Invalid role specified.");

            }
        }
    }
}
