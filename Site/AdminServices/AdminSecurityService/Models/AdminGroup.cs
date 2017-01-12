namespace Vtb24.Arms.AdminServices.AdminSecurityService.Models
{
    public class AdminGroup
    {
        public string Name { get; set; }

        public string[] Users { get; set; }

        public IPermissionsSource Permissions { get; set; }
    }
}
