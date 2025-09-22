using Microsoft.AspNetCore.Identity;
using SchoolManangement.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Business.Helpers
{
    public static class CredentialHelper
    {
        private static readonly Random _random = Random.Shared;

        // Unique username üretimi
        public static async Task<string> GenerateUniqueUsernameAsync(UserManager<ApplicationUser> userManager, string firstName, string lastName)
        {
            var baseUsername = $"{firstName.ToLower()}{lastName.ToLower()}";
            var username = baseUsername;
            var counter = 100;

            while (await userManager.FindByNameAsync(username) != null)
            {
                username = $"{baseUsername}{counter}";
                counter++;
            }

            return username;
        }

        // Random password üretimi
        public static string GeneratePassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
