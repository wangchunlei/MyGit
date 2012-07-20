using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domas.Web.Models;
using Domas.Service.Base;
using Domas.Service.Base.Role;

namespace Domas.Web.Tools.Authorize
{
    public class AuthManager
    {
        public static bool HasPermission(string controllerFullName, string action, string user)
        {
            var success = false;

            using (var context = new UIComponentContext())
            {

                //人员
                bool successUser = context.Privileges.Any(p =>
                       (p.PrivilegeMaster_EnumValue == (int)PrivilegeMasterType.User &&
                        p.PrivilegeMasterValue == user && p.PrivilegeAccess_EnumValue == (int)PrivilegeAccessType.Operation &&
                        p.OperationFullName == controllerFullName && p.PrivilegeOperation_EnumValue == (int)PrivilegeOperationType.Enable)
                     );
                //角色
                List<string> listRoles = GetRoleCodeList(user);
                bool successRole = false;
                if (listRoles.Count > 0)
                {
                    successRole =
                  context.Privileges.Any(p =>
                    (
                    p.PrivilegeMaster_EnumValue == (int)PrivilegeMasterType.Role
                    && listRoles.Contains(p.PrivilegeMasterValue)
                     && p.PrivilegeAccess_EnumValue == (int)PrivilegeAccessType.Operation
                     && p.OperationFullName == controllerFullName
                     && p.PrivilegeAccessValue.Contains(action)
                     && p.PrivilegeOperation_EnumValue == (int)PrivilegeOperationType.Enable
                     )
                  );
                }

                success = successUser | successRole;
            }
            return success;
        }
        /// <summary>
        /// 根据用户编码取角色集合
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns></returns>
        private static List<string> GetRoleCodeList(string userCode)
        {
            List<string> list = new List<string>();
            using (var context = new BaseContext())
            {
                if (!string.IsNullOrEmpty(userCode))
                {
                    var user = context.Users.FirstOrDefault(u => u.Code == userCode);
                    if (user != null)
                    {
                        var roleCodes = (from ru in context.RoleUsers
                                         join r in context.Roles on ru.RoleID equals r.ID
                                         where ru.UserID == user.ID
                                         select r.Code
                          ).ToList();
                        list = roleCodes;
                    }

                }
            }
            return list;
        }
    }
}
