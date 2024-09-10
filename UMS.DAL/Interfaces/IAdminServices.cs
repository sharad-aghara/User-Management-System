using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.DAL.Interfaces
{
    public interface IAdminServices
    {

        Task<bool> ApproveUserAndGeneratePassword(int id);
        Task<bool> RejectUser(int id);
    }
}
