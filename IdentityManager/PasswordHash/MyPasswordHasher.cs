
using Microsoft.AspNetCore.Identity;


namespace IdentityManager.PasswordHash
{
    public class MyPasswordHasher : PasswordHasher<IdentityUser>
    {
        public override string HashPassword(IdentityUser user, string password)
        {
            //因为现在用户验证的流程回归Identity，所以不需要这个借口了，但是可以留着来方便查看密码
            //Basic HashPasswordAlgorithm
            /* string password = "Gnm19980521!";
               byte[] salt = new byte[128 / 8];
               using (var rng = RandomNumberGenerator.Create())
               {
                   rng.GetBytes(salt);
               }
               string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
               password: password,
               salt: salt,
               prf: KeyDerivationPrf.HMACSHA1,
               iterationCount: 10000,
               numBytesRequested: 256 / 8));*/
            return password;
        }
        public override PasswordVerificationResult VerifyHashedPassword(IdentityUser user, string hashedPassword, string providedPassword)
        {
            return hashedPassword.Equals(providedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}
