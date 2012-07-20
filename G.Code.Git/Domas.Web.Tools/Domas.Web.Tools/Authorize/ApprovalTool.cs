using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domas.DAP.ADF.Context;
using Domas.Service.Base;
using Domas.Service.Base.Approval;

namespace Domas.Web.Tools.Authorize
{
    public static class ApprovalTool<T> where T : class
    {
        public static ApprovalResultDTO Approval(T approval)
        {
            var resultDTO = new ApprovalResultDTO() { ApprovalResults = new List<ApprovalResult>(), LastApprovalResults = new List<LastApprovalResult>() ,ResourceIDs = new List<Guid>()};

            var type = typeof(T);
            var typeName = type.FullName;

            using (var context = new BaseContext())
            {
                var resList = context.ApprovalResources.Include("ApprovalRuleCollection").Include("ApprovalResultCollection").Where(r => r.ResFullName == typeName).OrderBy(r => r.ResNum);

                foreach (var resource in resList)
                {
                    var rules = resource.ApprovalRuleCollection.OrderBy(r => r.Seq);
                    int currentCount = 1;
                    int maxCount = rules.Count();
                    foreach (var rule in rules)
                    {
                        bool rv = false;
                        if (rule.RuleName == RuleType.CurrentUser.ToString())
                        {
                            rv = CalcExpress(rule.RuleValue, rule.Operator, Context.UserID);
                        }
                        //TODO: 当前角色(一个用户涉及到多个角色)，当前部门
                        else if (rule.RuleName == RuleType.CurrentRole.ToString())
                        {
                            rv = CalcExpress(rule.RuleValue, rule.Operator, Context.UserID);
                        }
                        else if (rule.RuleName == RuleType.CurrentDepartment.ToString())
                        {
                            rv = CalcExpress(rule.RuleValue, rule.Operator, Context.UserID);
                        }
                        else
                        {
                            var ruleType = rule.RuleType;
                            var ruleName = rule.RuleName;
                            if (ruleType.StartsWith("Domas."))
                            {
                                ruleName += "_EnumValue";
                            }
                            var av = type.GetProperty(ruleName).GetValue(approval, null);

                            rv = CalcExpress(rule.RuleValue, rule.Operator, av.ToString());
                        }

                        if (CustomRuleCalc(resultDTO, rv, rule, currentCount++ == maxCount))
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }

                    }
                    if (resultDTO.Result)
                    {
                        resultDTO.ResourceIDs.Add(resource.ID); //记录资源ID 
                        var results = resource.ApprovalResultCollection.ToList();
                        resultDTO.ApprovalResults.AddRange(results);
                        Hashtable ht = new Hashtable();
                        foreach (var approvalResult in results)
                        {
                            List<LastApprovalResult> listLastApprovalResult = GetLastApprovalResults((int)approvalResult.ApprovalMode_EnumValue, (int)approvalResult.ApprovalLevel_EnumValue, (int)approvalResult.ApprovalType_EnumValue,
                                                   approvalResult.ResultValue);
                            foreach (var lastApprovalResult in listLastApprovalResult)
                            {
                                string htKey = lastApprovalResult.Value;
                                string htValue = lastApprovalResult.ApprovalLevel.ToString() +
                                                 lastApprovalResult.ApprovalMode.ToString() + lastApprovalResult.Name;
                                if (ht[htKey] == null)
                                {
                                    ht.Add(htKey, htValue);
                                    resultDTO.LastApprovalResults.Add(lastApprovalResult);
                                }
                            }

                        }
                    }
                }
            }
            #region 取下最高审批等级
            foreach (var lastApprovalResult in resultDTO.LastApprovalResults)
            {
                if(lastApprovalResult.ApprovalLevel>resultDTO.MaxLevel)
                {
                    resultDTO.MaxLevel = lastApprovalResult.ApprovalLevel;
                }
            }
            
            #endregion
            return resultDTO;
        }

        private static bool CustomRuleCalc(ApprovalResultDTO resultDTO, bool rv, ApprovalRule rule, bool lastRule)
        {
            if ((rule.Compare == "and" && rv || rule.Compare == "or" && rv == false) && !lastRule)
            {
                return true;
            }

            if (!rv)
            {
                resultDTO.Result = false;
                return false;
            }

            resultDTO.Result = true;
            return false;
        }
        /// <summary>
        /// 表达式计算
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="oper"></param>
        /// <param name="ruleValue"></param>
        /// <returns></returns>
        static bool CalcExpress(string rule, string oper, string ruleValue)
        {
            if (oper == "equal")
            {
                return rule == ruleValue;
            }
            else if (oper == "nonequal")
            {
                return rule != ruleValue;
            }

            return false;
        }

        public static List<LastApprovalResult> GetLastApprovalResults(int approvalMode, int approvalLevel, int approvalResultType, string iDs)
        {
            List<LastApprovalResult> listLastApprovalResult = new List<LastApprovalResult>();
            if (approvalMode == 2) //会签
            {
                LastApprovalResult lastApprovalResult = new LastApprovalResult();
                lastApprovalResult.ApprovalLevel = approvalLevel;
                lastApprovalResult.ApprovalMode = approvalMode;
                if (approvalResultType == 1) //人员
                {
                    lastApprovalResult.Value = GetUserIDorName(true, approvalResultType, iDs);
                    lastApprovalResult.Name = "(" + GetUserIDorName(false, approvalResultType, iDs) + ")";
                }
                listLastApprovalResult.Add(lastApprovalResult);
            }
            else //抢签或通知
            {
                string[] userIDs = GetUserIDorName(true, approvalResultType, iDs).Split(',');
                string[] userNames = GetUserIDorName(false, approvalResultType, iDs).Split(',');
                for (int i = 0; i < userIDs.Count(); i++)
                {
                    LastApprovalResult lastApprovalResult = new LastApprovalResult();
                    lastApprovalResult.ApprovalLevel = approvalLevel;
                    lastApprovalResult.ApprovalMode = approvalMode;
                    lastApprovalResult.Value = userIDs[i];
                    lastApprovalResult.Name = userNames[i];
                    listLastApprovalResult.Add(lastApprovalResult);
                }
            }
            return listLastApprovalResult;

        }

        private static string GetUserIDorName(bool isGetUserID, int approvalResultType, string iDs)
        {
            string returnValue = string.Empty;
            using (var context = new BaseContext())
            {
                switch (approvalResultType)
                {
                    case 1:
                        #region 人员
                        List<Guid> list = new List<Guid>();
                        if (iDs.IndexOf(",") > -1)
                        {
                            #region 多选
                            var idArray = iDs.Split(',').ToList();
                            foreach (var id in idArray)
                            {
                                if (id == "DeptManager")
                                {
                                    //按当前用户ID找本部门负责人
                                    string deptMangerIDs = GetDeptMangerIDByUserID(Context.UserID);
                                    if (deptMangerIDs.IndexOf(",") > -1)
                                    {
                                        var deptMangerIDList = deptMangerIDs.Split(',').ToList();
                                        for (int i = 0; i < deptMangerIDList.Count; i++)
                                        {
                                            list.Add(new Guid(deptMangerIDList[i]));
                                        }
                                    }
                                    else
                                    {
                                        list.Add(new Guid(deptMangerIDs));
                                    }

                                }
                                else
                                {
                                    list.Add(new Guid(id));
                                }

                            }

                            #endregion
                        }
                        else
                        {
                            if (iDs == "DeptManager")
                            {
                                //按当前用户ID找本部门负责人
                                string deptMangerIDs = GetDeptMangerIDByUserID(Context.UserID);
                                if (!string.IsNullOrEmpty(deptMangerIDs))
                                {
                                    if (deptMangerIDs.IndexOf(",") > -1)
                                    {
                                        var deptMangerIDList = deptMangerIDs.Split(',').ToList();
                                        for (int i = 0; i < deptMangerIDList.Count; i++)
                                        {
                                            list.Add(new Guid(deptMangerIDList[i]));
                                        }
                                    }
                                    else
                                    {
                                        list.Add(new Guid(deptMangerIDs));
                                    }
                                }
                            }
                            else
                            {
                                list.Add(new Guid(iDs));
                            }
                        }
                        #region

                        var disList = list.Distinct();//过滤重复项
                        var users = (from u in context.Users
                                     join idItem in disList on u.ID equals idItem
                                     select u).ToList();
                        for (int i = 0; i < users.Count; i++)
                        {
                            if (i == users.Count - 1)
                            {
                                if (isGetUserID)
                                {
                                    returnValue += users[i].ID.ToString();
                                }
                                else
                                {
                                    returnValue += users[i].Name.ToString();
                                }
                            }
                            else
                            {
                                if (isGetUserID)
                                {
                                    returnValue += users[i].ID.ToString() + ",";
                                }
                                else
                                {
                                    returnValue += users[i].Name.ToString() + ",";
                                }
                            }
                        }
                        #endregion
                        #endregion
                        break;
                    default:
                        break;
                }
            }

            return returnValue;

        }
        private static string GetDeptMangerIDByUserID(string userID)
        {
            string returnValue = string.Empty;
            using (var context = new BaseContext())
            {
                Guid userIDGuid = new Guid(userID);
                var departMentPositions = context.DepartmentPositions.Where(p => p.UserID == userIDGuid).ToList();
                if (departMentPositions != null)
                {
                    for (int i = 0; i < departMentPositions.Count; i++)
                    {
                        Guid departmentID = new Guid(departMentPositions[i].DepartmentID.ToString());
                        var departMent = context.Departments.Where(d => d.ID == departmentID).FirstOrDefault();
                        returnValue += i == departMentPositions.Count - 1 ? departMent.PrimaryManagerID.ToString() : departMent.PrimaryManagerID.ToString() + ",";
                    }
                }

            }
            return returnValue;
        }

    }
    public class ApprovalResultDTO
    {
        public bool Result { get; set; }
        public List<ApprovalResult> ApprovalResults { get; set; }
        public List<LastApprovalResult> LastApprovalResults { get; set; }
        public int MaxLevel { get; set; }
        public List<Guid> ResourceIDs { get; set; }
    }
    public class LastApprovalResult
    {
        public string Value { get; set; }
        public string Name { get; set; }
        public int ApprovalLevel { get; set; }
        public int ApprovalMode { get; set; }
    }
    public enum RuleType
    {
        CurrentUser = 1,
        CurrentRole = 2,
        CurrentDepartment = 3,
    }

}
