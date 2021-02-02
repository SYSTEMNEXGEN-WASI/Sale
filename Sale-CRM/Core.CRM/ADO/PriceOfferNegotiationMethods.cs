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
    public class PriceOfferNegotiationMethods
    {
        static DataTable dt = new DataTable();
        static string strAutoCode = string.Empty;
        static string autoProspect_ID = string.Empty;
        static bool IsSaved = false;
        static bool IsDeleted = false;
        static SqlParameter[] nullSqlParam = null;
        static SysFunction sysfun = new SysFunction();
        static DateTime recDate = new DateTime();

        static Transaction ObjTrans = new Transaction();
        static SqlTransaction Trans;


        public static List<UCS_EvaluationVM> Get_PriceOfferNegociationData(string dealerCode)
        {
            DataTable dt = new DataTable();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            // string TransType = "CSI";
            //string SaleType = "Cash";


            List<UCS_EvaluationVM> lst = new List<UCS_EvaluationVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode)//0
                                    // , new SqlParameter("@TransType",TransType)//0
                                    //, new SqlParameter("@SaleType",SaleType)//0
									};

                dt = DataAccess.getDataTable("Select_EvaluationCode", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<UCS_EvaluationVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }

        public static bool Insert_PriceOfferNegociation(UCS_EvaluationVM model, string dealerCode)
        {

            try
            {

                if (string.IsNullOrEmpty(model.EvaluationCode))
                {
                    if (sysfun.IsExist("RegNo", model.RegNo, "UCS_EvaluationMaster", dealerCode))
                    {
                        return IsSaved;
                    }
                    strAutoCode = sysfun.AutoGen("UCS_EvaluationMaster", "EvaluationCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);
                    //strAutoCode = sysfun.GetNewMaxID("UCS_EvaluationMaster", "EvaluationCode", 8, dealerCode);
                }
                else
                {
                    strAutoCode = model.EvaluationCode;
                }


                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode          ",dealerCode),//0
								 new SqlParameter("@EvaluationCode       ",strAutoCode),//1
                                 new SqlParameter("@EvaluationDate       ",sysfun.SaveDate(model.EvaluationDate)),//2
                                 new SqlParameter("@UCSourceCode         ",model.UCSourceCode),//3
                                 new SqlParameter("@RegNo                ",model.RegNo),//4
                                 new SqlParameter("@RegDate              ",sysfun.SaveDate(model.RegDate)),//5
                                 new SqlParameter("@DateOfInspection     ",sysfun.SaveDate(model.DateOfInspection)),//6
                                 new SqlParameter("@EngineNo            " ,model.EngineNo),//7
                                 new SqlParameter("@ChassisNo            ",model.ChassisNo            ),//8
                                 new SqlParameter("@BrandCode            ",model.BrandCode            ),//9
                                 new SqlParameter("@ProdCode             ",model.ProdCode             ),//10
                                 new SqlParameter("@VersionCode          ",model.VersionCode          ),//11
                                 new SqlParameter("@ColorCode"            ,model.ColorCode           ),//12
                                 new SqlParameter("@CusCode              ",model.CusCode              ),//13
                                 new SqlParameter("@TransmissionType     ",model.TransmissionType     ),//14
                                 new SqlParameter("@UserName             ",model.UserName             ),//15
                                 new SqlParameter("@OfferCategory        ",model.OfferCategory        ),//16
                                 new SqlParameter("@CurrentMilage        ",model.CurrentMilage        ),//17
                                 new SqlParameter("@CustomerExpectedPrice",model.CustomerExpectedPrice),//18
                                 new SqlParameter("@ExpectationCustomer  ",model.ExpectationCustomer  ),//19
                                 new SqlParameter("@BuyingPriceOffer     ",model.BuyingPriceOffer     ),//20
                                 new SqlParameter("@ReCondition          ",model.ReCondition          ),//21
                                 new SqlParameter("@UCSourceRemarks      ",model.UCSourceRemarks      ),//22
                                 new SqlParameter("@UpdUser",AuthBase.UserId),//23
								 new SqlParameter("@UpdTerm",General.CurrentIP),//24
                                 new SqlParameter("@IsVerificationFromCPLC ",model.IsVerificationFromCPLC),//25
                                 new SqlParameter("@RegistrationBook",model.RegistrationBook),//26
                                 new SqlParameter("@SaleInvoice",model.SaleInvoice),//27
                                 new SqlParameter("@ExchangeType",(model.OfferCategory == "Exchange" ? 'Y' : 'N')),//27
                                 new SqlParameter("@CellNo          ",model.CellNo         ),//21

                };
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    sysfun.ExecuteSP_NonQuery("Insert_PriceOfferNegociation", param, Trans);


                    IsSaved = true;
                }
                //if (sysfun.ExecuteSP_NonQuery("Insert_PriceOfferNegociation", param))
                //{
                //    IsSaved = true;
                //}

            }
            catch (Exception)
            {

                throw;
            }

            return IsSaved;
        }
        public static bool Insert_PriceOfferNegociation_EvaluationDetail(UCS_EvaluationVM model, string dealerCode)
        {

            try
            {

                SqlParameter[] param = {
                                 new SqlParameter("@EvaluationCode                   "    ,strAutoCode        ),//0
								 new SqlParameter("@DealerCode                       "          ,dealerCode             ),//1
                                 new SqlParameter("@C_Engine                         "     ,Convert.ToDecimal(model.C_Engine )               ),//2
                                 new SqlParameter("@C_IgnitionSyatem                 "     ,Convert.ToDecimal(model.C_IgnitionSyatem)          ),//3
                                 new SqlParameter("@C_FuelSystem                     "     ,Convert.ToDecimal(model.C_FuelSystem    )        ),//4
                                 new SqlParameter("@C_Drivetrain                     "     ,Convert.ToDecimal(model.C_Drivetrain    )        ),//5
                                 new SqlParameter("@C_Brakes                         "     ,Convert.ToDecimal(model.C_Brakes        )           ),//6
                                 new SqlParameter("@C_Suspension                     "     ,Convert.ToDecimal(model.C_Suspension    )        ),//7
                                 new SqlParameter("@C_Steering                       "     ,Convert.ToDecimal(model.C_Steering      )        ),//8
                                 new SqlParameter("@C_Electrical                     "     ,Convert.ToDecimal(model.C_Electrical    )        ),//9
                                 new SqlParameter("@C_Body                           "     ,Convert.ToDecimal(model.C_Body          )        ),//10
                                 new SqlParameter("@C_ACHeater                       "     ,Convert.ToDecimal(model.C_ACHeater      )        ),//11
                                 new SqlParameter("@C_OperationOfAccInSecB           "     ,Convert.ToDecimal(model.C_OperationOfAccInSecB)  ),//12
                                 new SqlParameter("@C_CNGOperation                   "     ,Convert.ToDecimal(model.C_CNGOperation         ) ),//13
                                 new SqlParameter("@C_IsEngine                       "     ,(model.C_Engine== null ? 'N' : 'Y')),//15
                                 new SqlParameter("@C_IsIgnitionSyatem               "     ,(model.C_IgnitionSyatem== null ? 'N' : 'Y')),//16
                                 new SqlParameter("@C_IsFuelSystem                   "     ,(model.C_FuelSystem== null ? 'N' : 'Y')),//17
                                 new SqlParameter("@C_IsDrivetrain                   "     ,(model.C_Drivetrain==null ? 'N' : 'Y')),//18
                                 new SqlParameter("@C_IsBrakes                       "     ,(model.C_Brakes== null ? 'N' : 'Y')),//19
                                 new SqlParameter("@C_IsSuspension                   "     ,(model.C_Suspension==null ? 'N' : 'Y')),//20
                                 new SqlParameter("@C_IsSteering                     "     ,(model.C_Steering==null ? 'N' : 'Y')),//21
                                 new SqlParameter("@C_IsElectrical                   "     ,(model.C_Electrical== null ? 'N' : 'Y')),//22
                                 new SqlParameter("@C_IsBody                         "     ,(model.C_Body==null ? 'N' : 'Y')),//23
								 new SqlParameter("@C_IsACHeater                     "     ,(model.C_ACHeater== null ? 'N' : 'Y')),//24
                                 new SqlParameter("@C_IsOperationOfAccInSecB         "     ,(model.C_IsOperationOfAccInSecB=="" ? 'N' : 'Y')),//25
                                 new SqlParameter("@UpdUser",AuthBase.UserId),//27
                                 new SqlParameter("@Updterm",General.CurrentIP),//27
								 
							};
                if (sysfun.ExecuteSP_NonQuery("Insert_PriceOfferNegociation_EvaluationDeatail", param, Trans) == true)
                {

                    IsSaved = true;
                }
                else
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                    IsSaved = false;
                }

                //if (sysfun.ExecuteSP_NonQuery("Insert_PriceOfferNegociation_EvaluationDeatail", param))
                //{
                //    IsSaved = true;
                //}


            }
            catch (Exception)
            {

                throw;
            }
            ObjTrans.CommittTransaction(ref Trans);

            return IsSaved;
        }
        public static bool Insert_DealFail(UCS_EvaluationVM model, ref string msg)
        {

            try
            {

                SqlParameter[] param2 = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@EvaluationCode",model.EvaluationCode),//1
								 new SqlParameter("@IsDealFail",model.IsDealFail),//2								 
								
                            };

                if (sysfun.ExecuteSP_NonQuery("sp_IsDealFail_Evaluation", param2))
                {
                    IsSaved = true;
                }
                else
                {

                    IsSaved = false;
                }

            }

            catch (Exception ex)
            {

                msg = ex.Message;
                IsSaved = false;
            }

            return IsSaved;
        }
        public static bool Delete_PriceOfferNegociation_Record(string enquiryId, string dealerCode)
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = {
                new SqlParameter("@DealerCode",dealerCode),
                new SqlParameter("@EvaluationCode",enquiryId)
            };

            if (sysfun.ExecuteSP_NonQuery("sp_PriceOfferNegociation_Delete", param))
            {
                IsDeleted = true;
            }
            else
            {
                IsDeleted = false;
            }


            return IsDeleted;
        }
    }
}
