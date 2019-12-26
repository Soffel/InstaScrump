namespace InstaScrump.Common.Extension.ModelExtension
{
    public static class LoginModelExtension
    {
        public static bool Validate(this LoginModel loginData)
        {
            return !loginData.UserName.IsNullOrWhiteSpace() &&
                   !loginData.Pswd.IsNullOrWhiteSpace() &&
                   !loginData.Key.IsNullOrWhiteSpace() && 
                   loginData.Pswd.Length > 4;
        }
    }
}
