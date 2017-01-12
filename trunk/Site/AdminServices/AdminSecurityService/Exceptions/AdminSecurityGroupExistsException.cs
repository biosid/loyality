namespace Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions
{
    public class AdminSecurityGroupExistsException : AdminSecurityServiceException
    {
        public string Name { get; private set; }

        public AdminSecurityGroupExistsException(string name)
            : base(ResultCodes.GroupAlreadyExists, "������ � ������ \"" + name + "\" ��� ����������")
        {
            Name = name;
        }
    }
}
