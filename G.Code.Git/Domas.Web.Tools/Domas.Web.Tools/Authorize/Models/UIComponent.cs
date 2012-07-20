using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Domas.Web.Tools.Authorize.Models;
using System.ComponentModel.DataAnnotations;

namespace Domas.Web.Models
{
    public class UIComponent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid ID { get; set; }
        public System.DateTime? CreatedOn { get; set; }
        public string Createdby { get; set; }
        public System.DateTime? ModifiedOn { get; set; }
        public string Modifiedby { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        //[System.Web.Script.Serialization.ScriptIgnore]
        public IList<UIMenu> UIMenus { get; set; }
    }
    public class UIMenu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid ID { get; set; }
        public System.DateTime? CreatedOn { get; set; }
        public string Createdby { get; set; }
        public System.DateTime? ModifiedOn { get; set; }
        public string Modifiedby { get; set; }
        public System.Guid? ParentID { get; set; }
        public UIMenu Parent { get; set; }
        public System.Guid OwnerID { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]
        public UIComponent Owner { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsLeaf { get; set; }
        public bool IsAdminSaferAuditor { get; set; } //三员专有
        public string ActionURL { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]
        public IList<UIOperation> UIOperations { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]
        public IList<UIMenu> SubMenus { get; set; }
    }
    public class UIOperation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid ID { get; set; }
        public System.DateTime? CreatedOn { get; set; }
        public string Createdby { get; set; }
        public System.DateTime? ModifiedOn { get; set; }
        public string Modifiedby { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public UIMenu Owner { get; set; }
        public System.Guid OwnerID { get; set; }
        public string FullName { get; set; }
    }
    //Button,Controll...
    public class Privilege
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid ID { get; set; }
        public System.DateTime? CreatedOn { get; set; }
        public string Createdby { get; set; }
        public System.DateTime? ModifiedOn { get; set; }
        public string Modifiedby { get; set; }
        public PrivilegeMasterType PrivilegeMaster { get; set; }
        public int PrivilegeMaster_EnumValue { get; set; }
        public string PrivilegeMasterValue { get; set; }
        public PrivilegeAccessType PrivilegeAccess { get; set; }
        public int PrivilegeAccess_EnumValue { get; set; }
        public string PrivilegeAccessValue { get; set; }
        public PrivilegeOperationType PrivilegeOperation { get; set; }
        public int PrivilegeOperation_EnumValue { get; set; }
        public string OperationFullName { get; set; }
    }


    public class DataPrivilege
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid ID { get; set; }

        public string DataPrivilegeView { get; set; }
        public string DataPrivilegeRule { get; set; }
    }
    public enum PrivilegeMasterType
    {
        User,
        Role,
        Department
    }
    public enum PrivilegeAccessType
    {
        Component,
        Menu,
        Operation
    }
    public enum PrivilegeOperationType
    {
        Enable,
        Disable
    }
    public class UIComponentContext : DbContext
    {
        public UIComponentContext()
            : base("UIComponent")
        {
            Database.SetInitializer(new UIComponentContextInitializer());
        }

        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<DataPrivilege> DataPrivileges { get; set; }
        public DbSet<Domas.Web.Models.UIComponent> UIComponents { get; set; }

        public DbSet<Domas.Web.Models.UIMenu> UIMenus { get; set; }

        public DbSet<Domas.Web.Models.UIOperation> UIOperations { get; set; }
        public DbSet<UIActionLogging> UiActionLoggings { get; set; }
        public DbSet<DataResource> DataResources { get; set; }
        public DbSet<DataRule> DataRules { get; set; }

        public DbSet<CustomerUI> CustomerUIs { get; set; }
        public DbSet<CustomerUIProperty> CustomerUIProperties { get; set; }

        internal partial class UIComponentContextInitializer : CreateDatabaseIfNotExists<UIComponentContext>
        {
            protected override void Seed(UIComponentContext context)
            {
                base.Seed(context);
                #region 预置数据
                #region 0.customerui
                var poEntity = DAP.ADF.Context.Context.PrintMetadata.EntityCollection.FirstOrDefault(c => c.Code == "PO");

                var cus = new CustomerUI
                {
                    Code = "Area_PrintPortal_PO_Submit",
                    CustomerUiProperties = new List<CustomerUIProperty>()
                };
                string approvalDimensions = "SubmitBy,SubmitOn,SafeLevel,UsageType,ApprovalType,DocumentType";
                poEntity.PropertyCollection.Where(d => approvalDimensions.Contains(d.Code)).ToList().ForEach(c => cus.CustomerUiProperties.Add(new CustomerUIProperty
                {
                    Code = c.Code,
                    Name = c.Name,
                    IsEnable = true,
                    IsReadOnly = false,
                    GroupCode = "ApprovalInfo",
                    GroupName = "审批信息",
                    PropertyType = c.TypeFullName
                }));
                poEntity.PropertyCollection.Where(d => !approvalDimensions.Contains(d.Code)).ToList().ForEach(c => cus.CustomerUiProperties.Add(new CustomerUIProperty
                {
                    Code = c.Code,
                    Name = c.Name,
                    IsEnable = true,
                    IsReadOnly = true,
                    GroupCode = "BaseInfo",
                    GroupName = "基础信息",
                    PropertyType = c.TypeFullName
                }));
                context.CustomerUIs.Add(cus);
                #endregion
                #region 1.系统管理
                PrivilegeMasterType privilegeMasterType = PrivilegeMasterType.Role;
                PrivilegeAccessType privilegeAccessType = PrivilegeAccessType.Menu;
                PrivilegeAccessType privilegeAccessType_Operation = PrivilegeAccessType.Operation;
                PrivilegeMasterType privilegeMasterType_User = PrivilegeMasterType.User;

                UIComponent uiComponent = CreateUIComponent("010SysManage", "系统管理");
                context.UIComponents.Add(uiComponent);
                string actionURL = string.Empty;
                #region 1.1组织机构
                actionURL = "Area_BasePortal/Department/Index";
                string controllerFullName = "Domas.Webs.BasePortal.Controllers.DepartmentController";
                string roleCode = "AdminRole";
                string userCode = "Admin";
                string menuCode = "SysManage0010";
                string menuName = "部门设置";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);

                actionURL = "Area_BasePortal/DepartmentPosition/Index";
                controllerFullName = "Domas.Webs.BasePortal.Controllers.DepartmentPositionController";
                roleCode = "AdminRole";
                userCode = "Admin";
                menuCode = "SysManage0012";
                menuName = "部门岗位设置";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);

                actionURL = "Area_BasePortal/Position/Index";
                controllerFullName = "Domas.Webs.BasePortal.Controllers.PositionController";
                roleCode = "AdminRole";
                userCode = "Admin";
                menuCode = "SysManage0015";
                menuName = "岗位设置";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "Area_BasePortal/User/Index";
                controllerFullName = "Domas.Webs.BasePortal.Controllers.UserController";
                roleCode = "AdminRole";
                userCode = "Admin";
                menuCode = "SysManage0020";
                menuName = "人员设置";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);

                actionURL = "Area_BasePortal/UserGroup/Index";
                controllerFullName = "Domas.Webs.BasePortal.Controllers.UserGroupController";
                roleCode = "AdminRole";
                userCode = "Admin";
                menuCode = "SysManage0022";
                menuName = "用户组维护";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                #endregion
                #region 1.2 帐户管理
                actionURL = "";
                controllerFullName = "";
                roleCode = "SaferRole";
                userCode = "Safer";
                menuCode = "SysManage0025";
                menuName = "密码设置";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "Area_BasePortal/Account/Index";
                controllerFullName = "Domas.Webs.BasePortal.Controllers.AccountController";
                roleCode = "AdminRole";
                userCode = "Admin";
                menuCode = "SysManage0030";
                menuName = "登录帐户";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);



                actionURL = "";
                controllerFullName = "";
                roleCode = "SaferRole";
                userCode = "Safer";
                menuCode = "SysManage0035";
                menuName = "帐户解锁";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                #endregion
                #region 1.3设备管理
                actionURL = "";
                controllerFullName = "";
                roleCode = "AdminRole";
                userCode = "Admin";
                menuCode = "SysManage0040";
                menuName = "刷卡控制终端";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "";
                controllerFullName = "";
                roleCode = "AdminRole";
                userCode = "Admin";
                menuCode = "SysManage0045";
                menuName = "文件回收设备";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "Area_BasePortal/Equipment/Index";
                controllerFullName = "Domas.Webs.BasePortal.Controllers.EquipmentController";
                roleCode = "AdminRole";
                userCode = "Admin";
                menuCode = "SysManage0050";
                menuName = "物理输出设备";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "";
                controllerFullName = "";
                roleCode = "AdminRole";
                userCode = "Admin";
                menuCode = "SysManage0055";
                menuName = "虚拟打印设备";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "";
                controllerFullName = "";
                roleCode = "AdminRole";
                userCode = "Admin";
                menuCode = "SysManage0060";
                menuName = "打印设备驱动";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);



                #endregion
                #region 1.4 权限管理
                actionURL = "";
                controllerFullName = "";
                roleCode = "AdminRole";
                userCode = "Admin";
                menuCode = "SysManage0065";
                menuName = "功能设置";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "Area_BasePortal/Role/Index";
                controllerFullName = "Domas.Webs.BasePortal.Controllers.RoleController";
                roleCode = "AdminRole";
                userCode = "Admin";
                menuCode = "SysManage0070";
                menuName = "角色设置";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "PrivilegeTest/Create";
                controllerFullName = "Domas.Webs.MVC.Controllers.PrivilegeTestController";
                roleCode = "AdminRole";
                userCode = "Admin";
                menuCode = "SysManage0075";
                menuName = "角色权限";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "Area_BasePortal/RoleUser/Index";
                controllerFullName = "Domas.Webs.BasePortal.Controllers.RoleUserController";
                roleCode = "SaferRole";
                userCode = "Safer";
                menuCode = "SysManage0080";
                menuName = "角色用户";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);



                actionURL = "";
                controllerFullName = "";
                roleCode = "SaferRole";
                userCode = "Safer";
                menuCode = "SysManage0085";
                menuName = "设备权限";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);



                #endregion
                #region 1.5 安全策略
                actionURL = "Area_BasePortal/Parameter/Index";
                controllerFullName = "Domas.Webs.BasePortal.Controllers.ParameterController";
                roleCode = "SaferRole";
                userCode = "Safer";
                menuCode = "SysManage0088";
                menuName = "系统参数";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "";
                controllerFullName = "";
                roleCode = "SaferRole";
                userCode = "Safer";
                menuCode = "SysManage0090";
                menuName = "IP密级设置";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "";
                controllerFullName = "";
                roleCode = "SaferRole";
                userCode = "Safer";
                menuCode = "SysManage0095";
                menuName = "岗位密级设置";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);



                actionURL = "";
                controllerFullName = "";
                roleCode = "SaferRole";
                userCode = "Safer";
                menuCode = "SysManage0100";
                menuName = "人员密级设置";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "";
                controllerFullName = "";
                roleCode = "SaferRole";
                userCode = "Safer";
                menuCode = "SysManage0105";
                menuName = "设备密级设置";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                #endregion
                #region 1.6 流程管理
                actionURL = "";
                controllerFullName = "";
                roleCode = "SaferRole";
                userCode = "Safer";
                menuCode = "SysManage0110";
                menuName = "审批维度设置";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "Area_BasePortal/ApprovalResource/ApprovalManagement";
                controllerFullName = "Domas.Webs.BasePortal.Controllers.ApprovalResourceController";
                roleCode = "SaferRole";
                userCode = "Safer";
                menuCode = "SysManage0115";
                menuName = "审批流程设置";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);

                actionURL = "Area_PrintPortal/PO/";
                controllerFullName = "Domas.Webs.PrintPortal.Controllers.POController";
                roleCode = "OrdinaryUserRole";
                userCode = "";
                menuCode = "SysManage0116";
                menuName = "打印作业";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);

                actionURL = "Area_ApprovalPortal/ApprovalOrder/";
                controllerFullName = "Domas.Webs.ApprovalPortal.Controllers.ApprovalOrderController";
                roleCode = "OrdinaryUserRole";
                userCode = "";
                menuCode = "SysManage0117";
                menuName = "审批作业";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);

                #endregion
                #endregion
                #region 2.基础设置
                uiComponent = CreateUIComponent("015Basis", "基础设置");
                context.UIComponents.Add(uiComponent);

                actionURL = "Area_BasePortal/CodingRule/Index";
                controllerFullName = "Domas.Webs.BasePortal.Controllers.CodingRuleController";
                roleCode = "AdminRole";
                userCode = "Admin";
                menuCode = "Basis0015";
                menuName = "编码方案设置";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "PrivilegeTest/Index";
                controllerFullName = "Domas.Webs.MVC.Controllers.PrivilegeTestController";
                roleCode = "AdminRole";
                userCode = "Admin";
                menuCode = "Basis0020";
                menuName = "表达式设置";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "";
                controllerFullName = "";
                roleCode = "AdminRole";
                userCode = "Admin";
                menuCode = "Basis0025";
                menuName = "操作系统设置";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "";
                controllerFullName = "";
                roleCode = "AdminRole";
                userCode = "Admin";
                menuCode = "Basis0030";
                menuName = "可扩展枚举设置";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "Area_BasePortal/Driver/Index";
                controllerFullName = "Domas.Webs.BasePortal.Controllers.DriverController";
                roleCode = "AdminRole";
                userCode = "Admin";
                menuCode = "Basis0035";
                menuName = "驱动程序维护";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);



                #endregion
                #region 3.文件管理
                uiComponent = CreateUIComponent("020FileManage", "文件管理");
                context.UIComponents.Add(uiComponent);

                actionURL = "";
                controllerFullName = "";
                roleCode = "OrdinaryUserRole";
                userCode = "";
                menuCode = "FileManage0010";
                menuName = "文件登记";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);

                actionURL = "";
                controllerFullName = "";
                roleCode = "OrdinaryUserRole";
                userCode = "";
                menuCode = "FileManage0015";
                menuName = "文件移交";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);


                actionURL = "";
                controllerFullName = "";
                roleCode = "OrdinaryUserRole";
                userCode = "";
                menuCode = "FileManage0020";
                menuName = "文件接收";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);

                actionURL = "";
                controllerFullName = "";
                roleCode = "FileManagerRole";
                userCode = "";
                menuCode = "FileManage0025";
                menuName = "文件回收";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);

                actionURL = "";
                controllerFullName = "";
                roleCode = "FileManagerRole";
                userCode = "";
                menuCode = "FileManage0030";
                menuName = "文件销毁";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);

                actionURL = "";
                controllerFullName = "";
                roleCode = "FileManagerRole";
                userCode = "";
                menuCode = "FileManage0035";
                menuName = "条码回收";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);

                actionURL = "";
                controllerFullName = "";
                roleCode = "FileManagerRole";
                userCode = "";
                menuCode = "FileManage0040";
                menuName = "条码销毁";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);

                actionURL = "";
                controllerFullName = "";
                roleCode = "FileManagerRole";
                userCode = "";
                menuCode = "FileManage0045";
                menuName = "插页换页";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);

                actionURL = "";
                controllerFullName = "";
                roleCode = "OrdinaryUserRole";
                userCode = "";
                menuCode = "FileManage0050";
                menuName = "智能回收";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);


                #endregion
                #region 4.审计管理
                uiComponent = CreateUIComponent("025AuditManage", "审计管理");
                context.UIComponents.Add(uiComponent);
                actionURL = "";
                controllerFullName = "";
                roleCode = "AuditorRole";
                userCode = "Auditor";
                menuCode = "AuditManage0010";
                menuName = "日志审计";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                #endregion
                #region 5.统计分析
                uiComponent = CreateUIComponent("030QueryAndAnalysis", "统计分析");
                context.UIComponents.Add(uiComponent);

                actionURL = "";
                controllerFullName = "";
                roleCode = "SaferRole";
                userCode = "Safer";
                menuCode = "QueryAndAnalysis0010";
                menuName = "操作日志查询";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "";
                controllerFullName = "";
                roleCode = "SaferRole";
                userCode = "Safer";
                menuCode = "QueryAndAnalysis0015";
                menuName = "作业日志查询";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);



                actionURL = "";
                controllerFullName = "";
                roleCode = "OrdinaryUserRole";
                userCode = "";
                menuCode = "QueryAndAnalysis0020";
                menuName = "打印作业查询";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);

                actionURL = "";
                controllerFullName = "";
                roleCode = "OrdinaryUserRole";
                userCode = "";
                menuCode = "QueryAndAnalysis0025";
                menuName = "输出作业查询";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);

                actionURL = "";
                controllerFullName = "";
                roleCode = "OrdinaryUserRole";
                userCode = "";
                menuCode = "QueryAndAnalysis0030";
                menuName = "刻录作业查询";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);

                actionURL = "";
                controllerFullName = "";
                roleCode = "OrdinaryUserRole";
                userCode = "";
                menuCode = "QueryAndAnalysis0035";
                menuName = "文件登记查询";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);

                actionURL = "";
                controllerFullName = "";
                roleCode = "OrdinaryUserRole";
                userCode = "";
                menuCode = "QueryAndAnalysis0040";
                menuName = "文件接收查询";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);

                actionURL = "";
                controllerFullName = "";
                roleCode = "OrdinaryUserRole";
                userCode = "";
                menuCode = "QueryAndAnalysis0045";
                menuName = "文件移交查询";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);

                actionURL = "";
                controllerFullName = "";
                roleCode = "FileManagerRole";
                userCode = "";
                menuCode = "QueryAndAnalysis0050";
                menuName = "文件回收查询";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);

                actionURL = "";
                controllerFullName = "";
                roleCode = "FileManagerRole";
                userCode = "";
                menuCode = "QueryAndAnalysis0055";
                menuName = "文件销毁查询";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);

                actionURL = "";
                controllerFullName = "";
                roleCode = "FileManagerRole";
                userCode = "";
                menuCode = "QueryAndAnalysis0060";
                menuName = "条码回收查询";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);

                actionURL = "";
                controllerFullName = "";
                roleCode = "FileManagerRole";
                userCode = "";
                menuCode = "QueryAndAnalysis0065";
                menuName = "条码销毁查询";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, false);

                actionURL = "";
                controllerFullName = "";
                roleCode = "SaferRole";
                userCode = "Safer";
                menuCode = "QueryAndAnalysis0070";
                menuName = "销毁报表";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "";
                controllerFullName = "";
                roleCode = "SaferRole";
                userCode = "Safer";
                menuCode = "QueryAndAnalysis0075";
                menuName = "回收报表";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                actionURL = "";
                controllerFullName = "";
                roleCode = "SaferRole";
                userCode = "Safer";
                menuCode = "QueryAndAnalysis0080";
                menuName = "输出作业统计报表";
                SetPresetData(context, uiComponent, actionURL, menuCode, menuName, privilegeMasterType, privilegeAccessType, privilegeAccessType_Operation, controllerFullName, roleCode, privilegeMasterType_User, userCode, true);


                #endregion

                #endregion
                context.SaveChanges();
            }
        }

        #region 创建UIComponent、UIMenu、Privilege
        /// <summary>
        /// 创建UIComponent
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        private static UIComponent CreateUIComponent(string code, string name)
        {
            UIComponent uiComponent = new UIComponent
            {
                CreatedOn = DateTime.Now,
                Createdby = "System",
                ModifiedOn = DateTime.Now,
                Modifiedby = "System",
                Code = code,
                Name = name
            };
            return uiComponent;
        }
        /// <summary>
        /// 创建UIMenu
        /// </summary>
        /// <param name="uiComponent">所属Component</param>
        /// <param name="code">编码</param>
        /// <param name="name">名称</param>
        /// <param name="actionURL"></param>
        /// <param name="isAdminSaferAuditor"></param>
        /// <returns></returns>
        private static UIMenu CreateUIMenu(UIComponent uiComponent, string code, string name, string actionURL, bool isAdminSaferAuditor)
        {
            UIMenu uiMenu = new UIMenu
            {
                CreatedOn = DateTime.Now,
                Createdby = "System",
                ModifiedOn = DateTime.Now,
                Modifiedby = "System",
                Code = code,
                Name = name,
                IsLeaf = true,
                Owner = uiComponent,
                ActionURL = actionURL,
                IsAdminSaferAuditor = isAdminSaferAuditor
            };
            return uiMenu;
        }
        /// <summary>
        /// 创建UIOperation
        /// </summary>
        /// <param name="uiMenu">所属菜单</param>
        /// <param name="code">编码</param>
        /// <param name="name">名称</param>
        /// <param name="fullName">全名称</param>
        /// <returns></returns>
        private static UIOperation CreateUIOperation(UIMenu uiMenu, string code, string name, string fullName)
        {
            UIOperation uiOperation = new UIOperation
            {
                CreatedOn = DateTime.Now,
                Createdby = "System",
                ModifiedOn = DateTime.Now,
                Modifiedby = "System",
                Code = code,
                Name = name,
                Owner = uiMenu,
                FullName = fullName
            };
            return uiOperation;
        }

        /// <summary>
        /// 创建CreatePrivilege
        /// </summary>
        /// <param name="privilegeMasterType">类型</param>
        /// <param name="privilegeMasterValue">类型对应值</param>
        /// <param name="privilegeAccessType">访问类型</param>
        /// <param name="privilegeAccessValue">访问类型值</param>
        private static Privilege CreatePrivilege(PrivilegeMasterType privilegeMasterType, string privilegeMasterValue, PrivilegeAccessType privilegeAccessType, string privilegeAccessValue)
        {
            Privilege privilege = new Privilege
            {
                CreatedOn = DateTime.Now,
                Createdby = "System",
                ModifiedOn = DateTime.Now,
                Modifiedby = "System",
                PrivilegeMaster = privilegeMasterType,
                PrivilegeMaster_EnumValue = (int)privilegeMasterType,
                PrivilegeMasterValue = privilegeMasterValue,
                PrivilegeAccess = privilegeAccessType,
                PrivilegeAccess_EnumValue = (int)privilegeAccessType,
                PrivilegeAccessValue = privilegeAccessValue
            };
            return privilege;
        }
        /// <summary>
        /// 创建CreatePrivilege
        /// </summary>
        /// <param name="privilegeMasterType">类型</param>
        /// <param name="privilegeMasterValue">类型对应值</param>
        /// <param name="privilegeAccessType">访问类型</param>
        /// <param name="privilegeAccessValue">访问类型值</param>
        /// <param name="operationFullName">具体操作</param>
        /// <returns></returns>
        private static Privilege CreatePrivilege(PrivilegeMasterType privilegeMasterType, string privilegeMasterValue, PrivilegeAccessType privilegeAccessType, string privilegeAccessValue
            , string operationFullName)
        {
            Privilege privilege = new Privilege
            {
                CreatedOn = DateTime.Now,
                Createdby = "System",
                ModifiedOn = DateTime.Now,
                Modifiedby = "System",
                PrivilegeMaster = privilegeMasterType,
                PrivilegeMaster_EnumValue = (int)privilegeMasterType,
                PrivilegeMasterValue = privilegeMasterValue,
                PrivilegeAccess = privilegeAccessType,
                PrivilegeAccess_EnumValue = (int)privilegeAccessType,
                PrivilegeAccessValue = privilegeAccessValue,
                OperationFullName = operationFullName
            };
            return privilege;
        }
        #endregion
        #region 预置数据方法
        private static void SetPresetData(UIComponentContext context, UIComponent uiComponent, string actionURL, string menuCode, string menuName
            , PrivilegeMasterType privilegeMasterType, PrivilegeAccessType privilegeAccessType, PrivilegeAccessType privilegeAccessType_Operation
            , string controllerFullName, string roleCode, PrivilegeMasterType privilegeMasterType_User, string userCode, bool isAdminSaferAuditor
            )
        {
            UIMenu uiMenu = CreateUIMenu(uiComponent, menuCode, menuName, actionURL, isAdminSaferAuditor);
            context.UIMenus.Add(uiMenu);
            Privilege privilege = CreatePrivilege(privilegeMasterType, roleCode, privilegeAccessType, uiMenu.Code);
            context.Privileges.Add(privilege);
            if (!string.IsNullOrEmpty(userCode))
            {
                privilege = CreatePrivilege(privilegeMasterType_User, userCode, privilegeAccessType, uiMenu.Code);
                context.Privileges.Add(privilege);
            }
            UIOperation uiOperation = CreateUIOperation(uiMenu, menuCode + "_Create", "新增", controllerFullName);
            context.UIOperations.Add(uiOperation);
            privilege = CreatePrivilege(privilegeMasterType, roleCode, privilegeAccessType_Operation, uiOperation.Code, controllerFullName);
            context.Privileges.Add(privilege);
            if (!string.IsNullOrEmpty(userCode))
            {
                privilege = CreatePrivilege(privilegeMasterType_User, userCode, privilegeAccessType_Operation, uiOperation.Code, controllerFullName);
                context.Privileges.Add(privilege);
            }
            uiOperation = CreateUIOperation(uiMenu, menuCode + "_Edit", "修改", controllerFullName);
            context.UIOperations.Add(uiOperation);
            privilege = CreatePrivilege(privilegeMasterType, roleCode, privilegeAccessType_Operation, uiOperation.Code, controllerFullName);
            context.Privileges.Add(privilege);
            if (!string.IsNullOrEmpty(userCode))
            {
                privilege = CreatePrivilege(privilegeMasterType_User, userCode, privilegeAccessType_Operation, uiOperation.Code, controllerFullName);
                context.Privileges.Add(privilege);
            }

            uiOperation = CreateUIOperation(uiMenu, menuCode + "_Delete", "删除", controllerFullName);
            context.UIOperations.Add(uiOperation);
            privilege = CreatePrivilege(privilegeMasterType, roleCode, privilegeAccessType_Operation, uiOperation.Code, controllerFullName);
            context.Privileges.Add(privilege);
            if (!string.IsNullOrEmpty(userCode))
            {
                privilege = CreatePrivilege(privilegeMasterType_User, userCode, privilegeAccessType_Operation, uiOperation.Code, controllerFullName);
                context.Privileges.Add(privilege);
            }

            uiOperation = CreateUIOperation(uiMenu, menuCode + "_Index_Search_List", "查询", controllerFullName);
            context.UIOperations.Add(uiOperation);
            privilege = CreatePrivilege(privilegeMasterType, roleCode, privilegeAccessType_Operation, uiOperation.Code, controllerFullName);
            context.Privileges.Add(privilege);
            if (!string.IsNullOrEmpty(userCode))
            {
                privilege = CreatePrivilege(privilegeMasterType_User, userCode, privilegeAccessType_Operation, uiOperation.Code, controllerFullName);
                context.Privileges.Add(privilege);
            }

            #region 个性UI，权限分配
            if (controllerFullName == "Domas.Webs.PrintPortal.Controllers.POController")
            {
                uiOperation = CreateUIOperation(uiMenu, menuCode + "_Submit_GetApprovers", "作业提交", controllerFullName);
                context.UIOperations.Add(uiOperation);
                privilege = CreatePrivilege(privilegeMasterType, roleCode, privilegeAccessType_Operation, uiOperation.Code, controllerFullName);
                context.Privileges.Add(privilege);
                if (!string.IsNullOrEmpty(userCode))
                {
                    privilege = CreatePrivilege(privilegeMasterType_User, userCode, privilegeAccessType_Operation, uiOperation.Code, controllerFullName);
                    context.Privileges.Add(privilege);
                }
            }
            if (controllerFullName == "Domas.Webs.ApprovalPortal.Controllers.ApprovalOrderController")
            {
                uiOperation = CreateUIOperation(uiMenu, menuCode + "_Approval", "作业审批", controllerFullName);
                context.UIOperations.Add(uiOperation);
                privilege = CreatePrivilege(privilegeMasterType, roleCode, privilegeAccessType_Operation, uiOperation.Code, controllerFullName);
                context.Privileges.Add(privilege);
                if (!string.IsNullOrEmpty(userCode))
                {
                    privilege = CreatePrivilege(privilegeMasterType_User, userCode, privilegeAccessType_Operation, uiOperation.Code, controllerFullName);
                    context.Privileges.Add(privilege);
                }
            }
            #endregion

        }
        #endregion
    }
}
