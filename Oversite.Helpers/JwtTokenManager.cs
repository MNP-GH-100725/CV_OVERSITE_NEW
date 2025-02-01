
using Microsoft.IdentityModel.Tokens;
using RSA_Angular_.NET_CORE.RSA;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Oversite.Helpers
{
    public class JwtTokenManager
    {
        //public string CreateToken(int branchID, string employeeID)
        //{
        //	var tokenString = "";
        //	try
        //	{
        //		var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("myEncryptionKey@143#"));

        //		var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        //		var claims = new List<Claim>
        //					{
        //					new Claim(ClaimTypes.Name,  branchID + "," + employeeID+"," +DateTime.Now.ToLongTimeString())
        //				};
        //		var tokeOptions = new JwtSecurityToken(
        //		issuer: "http://localhost:5000",
        //		audience: "http://localhost:5000",
        //		claims: claims,
        //		notBefore: DateTime.Now,
        //		signingCredentials: signinCredentials
        //		);
        //		tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        //	}
        //	catch (Exception e) { }

        //	return tokenString;
        //}
        private string generateTOTP()
        {
            int lenthofpass = 6;
            string allowedChars = "";
            allowedChars = "1,2,3,4,5,6,7,8,9,0,";
            char[] sep = new[] { ',' };
            string[] arr = allowedChars.Split(sep);
            string otpString = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i <= lenthofpass - 1; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                otpString += temp;
            }
            return otpString;
        }
        public string CreateToken(int branchID, string employeeID, string siganture)
        {
            var tokenString = "";
            try
            {
                //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("myEncryptionKey@143#"));
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("myEncryptionKey@143#123456789012345678901234567890"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>
                            {
                            new Claim(ClaimTypes.Name, new RsaHelper().Encrypt(siganture +","+branchID + "," + employeeID +","+ generateTOTP()))
                        };
                var tokeOptions = new JwtSecurityToken(
                issuer: "http://localhost:5000",
                audience: "http://localhost:5000",
                claims: claims,
                notBefore: DateTime.Now,
                signingCredentials: signinCredentials
                );
                tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            }
            catch (Exception e) { }

            return tokenString;
        }

        public string CreatePaymentToken(string policyId, string CustomerID)
        {
            var tokenString = "";
            try
            {

                //var symmetricKey = Convert.FromBase64String("bXlFbmNyeXB0aW9uS2V5QDE0MyM=");
                //var SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature);
                //var claims = new List<Claim>
                //			{
                //			new Claim(ClaimTypes.Name, policyId +","+ CustomerID )
                //		    };
                //var tokeOptions = new JwtSecurityToken(
                //issuer: "http://localhost:5000",
                //audience: "http://localhost:5000",
                //claims: claims,
                //notBefore: DateTime.Now,
                //signingCredentials: SigningCredentials
                //);
                //tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                string Key = ("bXlFbmNyeXB0aW9uS2V5QDE0MyM=");
                tokenString = Base64Encode(policyId + "-" + Key + "-" + CustomerID);
            }
            catch (Exception e) { }

            return tokenString;
        }
        public string CreatePreToken(string domain, string environment, string module, string hashkey)
        {
            var tokenString = "";
            try
            {
                //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("myEncryptionKey@143#"));
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("myEncryptionKey@143#123456789012345678901234567890"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>
                            {
                            new Claim(ClaimTypes.Name, new RsaHelper().Encrypt(hashkey +","+ generateTOTP()))
                        };
                var tokeOptions = new JwtSecurityToken(
                issuer: "http://localhost:5000",
                audience: "http://localhost:5000",
                claims: claims,
                notBefore: DateTime.Now,
                signingCredentials: signinCredentials
                );
                tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            }
            catch (Exception e) { }

            return tokenString;
        }
        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
