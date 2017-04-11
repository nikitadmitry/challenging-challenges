using System;
using System.Collections.Generic;
using System.ServiceModel;
using Business.Identity.ViewModels;

namespace Business.Identity
{
    [ServiceContract]
    public interface IIdentityService
    {
        [OperationContract]
        IdentityUser CreateIdentityUser(IdentityUser identityUser);

        [OperationContract]
        IdentityUser UpdateIdentityUser(IdentityUser identityUser);

        [OperationContract]
        IdentityUser GetIdentityUserById(Guid userId);

        [OperationContract]
        IdentityUser GetIdentityUserByUserName(string normalizedUserName);

        [OperationContract]
        IdentityUser GetIdentityUserByEmail(string normalizedEmail);

        [OperationContract]
        string GetUserNameById(Guid userId);

        [OperationContract]
        void AddRatingToUser(Guid userId, double rating);

        [OperationContract]
        IList<UserTopViewModel> GetTopUsers();

        [OperationContract]
        void ConfirmEmail(Guid userId);

        [OperationContract]
        IdentityRole GetRoleById(Guid roleId);

        [OperationContract]
        IdentityRole GetRoleByName(string roleName);

        [OperationContract]
        UserModel GetUser(Guid userId);

        [OperationContract]
        void SetAbout(Guid userId, string about);
    }
}