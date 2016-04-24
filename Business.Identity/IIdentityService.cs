using System;
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
        IdentityUser GetIdentityUserByUserName(string userName);

        [OperationContract]
        IdentityUser GetIdentityUserByEmail(string email);

        [OperationContract]
        string GetUserNameById(Guid userId);

        [OperationContract]
        void AddRatingToUser(Guid userId, double rating);
    }
}