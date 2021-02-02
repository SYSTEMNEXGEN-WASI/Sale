using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Core.CRM.ADO
{
    public class TaskMethods
    {
        static bool IsSaved = false;
        static bool IsDeleted = false;
        static SqlParameter[] nullSqlParam = null;
        static DataTable dt = new DataTable();
        static string strAutoCode = string.Empty;
        static SysFunction sysfun = new SysFunction();
        static Transaction ObjTrans = new Transaction();
        static SqlTransaction Trans;

        public static string Get_TaskMasterDetail(string taskID, string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<TaskVM> lst = new List<TaskVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@TaskID",taskID)//0
									
									};

                dt = DataAccess.getDataTable("SP_Select_TaskMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<TaskVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static bool Insert_TaskMaster(TaskVM model)
        {
            //DateTime DueDate = Convert.ToDateTime(model.DueDate);
            //DateTime StartDate = Convert.ToDateTime(model.StartDate);
            //DateTime EndDate = Convert.ToDateTime(model.EndDate);

            try
            {
                if (model.TaskID == "0")
                {
                    strAutoCode = sysfun.AutoGen("CRM_TaskMaster", "TaskID", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), model.DealerCode);
                    
                }
                else
                {
                    strAutoCode = model.TaskID;                  

                }

                //DateTime EndTime = Convert.ToDateTime(model.EndTime);
              //  DateTime StartTime = Convert.ToDateTime(model.StartTime);
                
                

                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@TaskID",strAutoCode),//1						 
								 new SqlParameter("@AssignTo",model.AssignTo),//2	
								 new SqlParameter("@Prospect_ID",model.Prospect_ID),//5
								 new SqlParameter("@DueDate",(model.DueDate == null ? (object) DBNull.Value : sysfun.SaveDate(model.DueDate))),//6
								 new SqlParameter("@LeadSourceID",model.LeadSourceID),//7
								 new SqlParameter("@Contact",model.Contact),//8
								 new SqlParameter("@Email",model.Email),//9
								 new SqlParameter("@Lead_ID",model.Lead_ID),//12
								 new SqlParameter("@Comments",model.Comments),//13
								 new SqlParameter("@Frequency",model.Frequency),//14
								 new SqlParameter("@FreqTrun",model.FreqTrun),//15						 
								 new SqlParameter("@StartDate",sysfun.SaveDate(model.StartDate)),//16
								 new SqlParameter("@StartTime",sysfun.SaveTime(model.StartTime)),//17
								 new SqlParameter("@EndDate",(model.EndDate == null ? (object) DBNull.Value : sysfun.SaveDate(model.EndDate))),//18
								 new SqlParameter("@EndTime",(model.EndTime == null ? (object) DBNull.Value : Convert.ToDateTime(model.EndTime))),//19
								 new SqlParameter("@Ongoing",model.Ongoing),//20
								 new SqlParameter("@Reminder",model.Reminder),//21
								 new SqlParameter("@ReminderTime",(model.ReminderTime == null ? (object) DBNull.Value : Convert.ToDateTime(model.ReminderTime))),//22
								 new SqlParameter("@StatusTypeId",model.StatusTypeId),//24
                                 new SqlParameter("@StatusId",model.StatusId),//25
                                 new SqlParameter("@SubjectId",model.SubjectId),//26
                                 new SqlParameter("@TaskTypeId",model.TaskTypeId),//27

                            };

                if (sysfun.ExecuteSP_NonQuery("SP_Insert_TaskMaster", param))
                {
                    IsSaved = true;
                }               

            }
            catch (Exception)
            {
                throw;
            }

            return IsSaved;
        }
    }
}
