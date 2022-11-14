using Imkery.Data.Storage.Core;
using Imkery.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.Tests
{
    public class MockUserProvider : IImkeryUserProvider
    {
        public static  MockImkeryUser AdminUser = new MockImkeryUser()
        {
            IsAdministrator = true
        };
        public static MockImkeryUser DefaultUser = new MockImkeryUser();

        public bool LoggedIn { get; set; }
        public bool Admin { get; set; }
        public async Task<IImkeryUser?> GetCurrentUserAsync()
        {
            if (LoggedIn)
            {
                if (Admin)
                {
                    return AdminUser;
                }
                return DefaultUser;
            }
            else
            {
                return null;
            }
        }
    }

    public class MockImkeryUser : IImkeryUser
    {
        public string UserName { get; set; } = "Test";
        public Guid DefaultUserId = new Guid("{A91DACCD-E69E-4A72-BB62-D6CF4EEC3D60}");
        public string Id => DefaultUserId.ToString();

        public bool IsAdministrator { get; set; }
    }
}