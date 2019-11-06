using System;

namespace Fakka.Core.Models
{
    /// <summary>
    /// Object used to initialize user session on Successfull login
    /// </summary>
    public class UserSession : BaseOfflineModel
    {
        public UserSession()
        {

        }
        public UserSession(string username, string password, string token, string refreshToken, int? userRole, string uid , DateTime expiryDate, string schoolId, string phoneNumber = null ) 
        {
            Username = username;
            Password = password;
            Token = token;
            RefreshToken = refreshToken;
            UserRole = userRole;
            Uid = uid;
            PhoneNumber = phoneNumber;
            ExpiryDate = expiryDate;
            SchoolId = schoolId;
        }

        public string Uid { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int? UserRole { get; set; }
        public string SchoolId { get; set; }
        public string PointOfSaleId { get; set; }

        public void UpdateToken(string token, string refreshToken, DateTime expiryDate)
        {
            Token = token;
            RefreshToken = refreshToken;
            ExpiryDate = expiryDate;
        }

        public void UpdateSchoolId(string schoolId)
        {
            SchoolId = schoolId;
        }

    }
}