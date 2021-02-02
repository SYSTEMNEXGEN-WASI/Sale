using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM
{
    public class Constants
    {

        public static List<string> DocumentFormatList
        {
            get
            {
                return new List<string> { ".jpeg", ".pdf", ".png", ".jpg" };

            }
        }

        public enum ImageType
        {
            ProductImage = 1,
            AuctionCertificate = 2,
            AuctionImage = 3
        }

        public enum Client
        {
            JidoshaToredaJapan = 1,
            SKTJapan = 2
        }

        public enum Product
        {
            Chat = 1,
            ChatManagement = 2
        }

        public enum ProductPackage
        {
            FreeTrial = 1,
            Professional = 2,
            Enterprise = 3,
            AllProductPackageId = -1
        }

        public enum MessageType
        {
            Requirement = 1,
            Detail = 2,
            ContactUs = 3
        }
        public enum OG_ProductDocumentTypeId
        {
            CarDocuments_Pending = 15,
            CarDocument_Received = 15,
            CarDocument_Uploaded = 15,
            ExportCertificate_Pending = 6,
            ExportCertificateReceived = 6,
            ExportCertifcateUploaded = 6,
            UnitPicturesPending = 4,
            UnitPicturesUploaded = 4,
            AlterationPending = 18,
            AlterationRequested = 18,
            AlterationDone = 18,
            InspectionPending = 10,
            InspectionRequested = 10,
            InpectionFailed = 10,
            InpectionPass = 10,
            InspectionCertificateRequested = 10,
            InspectionCertificateReceived = 10,
            InspectionCertificateUploaded = 10,
            ShipmentBookingPending = 5,
            ShipmentBookingRequested = 5,
            ShipmentBookingDone = 5,
            ShipmentBookingDocumentReceived = 5,
            ShipmentBookingDocumentUploaded = 5,
            BLPending = 7,
            BLRequested = 7,
            BLUploaded_PaymentPending = 7,
            BLPaymentDone_RecevingPending = 7,
            BLReceived = 7,
            BLAmendmentRequested = 7
        }
        public enum OG_ProductDocumentStatus
        {

            InspectionPending = 50010,
            InspectionRequested = 50020,
            InpectionFailed = 50030,
            InpectionPassed = 50040,
            InspectionCertificateRequested = 50050,
            InspectionCertificateReceived = 50060,
            InspectionCertificateUploaded = 50070,
            CarDocuments_Pending = 10010,
            CarDocument_Received = 10020,
            CarDocument_Uploaded = 10030,
            ExportCertificate_Pending = 20010,
            ExportCertificate_Received = 20020,
            ExportCertifcate_Uploaded = 20030,
            UnitPictures_Pending = 30010,
            UnitPictures_Uploaded = 30020,
            Alteration_Pending = 40010,
            Alteration_Requested = 40020,
            Alteration_Done = 40030,
            Inspection_Pending = 50010,
            Inspection_Requested = 50020,
            Inspection_Certificate_Requested = 50050,
            Inspection_Certificate_Received = 50060,
            Inspection_Certificate_Uploaded = 50070,
            ShipmentBooking_Pending = 60010,
            ShipmentBooking_Requested = 60020
        }





        public enum SignUpThroughCode
        {
            Website = 1,
            Contactus = 2,
            QuickSignUp = 3,
            AgentSignUp = 4,
            LandingPageSignUp = 5,
            LeadDistribution = 8,
            LandingPageSignUp_myjapanautos = 6,
            LandingPageSignUp_auctioneermotors = 7,
            LandingPageSignUp_getusedcarjapan_com = 9
        }

        public enum SignUpMedium
        {
            Website = 1,
            Google = 2,
            Email = 3,
            msn = 4,
            yahoo = 5,
            live = 6,
            bing = 7,
            ask = 8,
            search = 9,
            daum = 10,
            altavista = 11,
            alltheweb = 12,
            netscape = 13,
            mama = 14,
            mamma = 15,
            terra = 16,
            cnn = 17,
            virgilio = 18,
            alice = 19,
            onet = 20,
            eniro = 21,
            about = 22,
            voila = 23,
            baidu = 24,
            yandex = 25,
            yam = 26,
            rambler = 27,
            fb = 28


        }

        public enum LifeCycleStatus
        {

            SubscriptionTerminated = 11
        }


        public enum CustomerType
        {
            AdminAgent = 1,
        }



        public enum CRMFunction
        {
            AddCustomer = 7,

        }

        public enum CRMExecutionStatus
        {
            first = 1,

        }



        public enum AppMedium
        {
            Webisite = 1,
            WebApi = 2,
            WindowsService = 3,
            ClassLibrary = 4,
            ExternalService = 5,

        }

        public enum OGEmailTemplate
        {
            OTPMailNotification = 1,
            ThankyouMailNotification = 2,
            PasswordResetEmailNotification = 3

        }

        public enum OGHelpSupportContent
        {
            Content = 1,
            MOSTREAD = 2,
            GETTINGSTARTED = 9,
            KNOWLEDGEBASE = 16
        }


        public enum ProductPackageTypeId
        {
            NormalWebsite = 1,
            ManageLiveChat = 2
        }




        public enum PaymentMethodCode
        {
            Card = 1,
            WireTransfer = 6,
            Stripe = 27,
            PayPal = 7,
            Bitcoin = 38

        }

        public enum BillingCycle
        {
            Monthly = 1,
            Yearly = 2
        }



        public enum SignUpPageCode
        {
            Register = 1,
            QuickSignup = 2,
            DetailRegister = 3,
            Contactus = 9,
            CRMRegister = 10,
            RequestACallBack = 11,
            CustomerSignUp = 12,
            LeadDistributionSignUpPage = 13
        }


        public enum ProductStatus
        {
            Instock = 1,
            InNegotiation = 2,
            Sold = 3,
            Outofstock = 4,
        }

        public enum OrderStatusCode
        {
            Pending = 6001,
            Completed = 6011,
            Expired = 6031
        }

        public enum InvoiceStatus
        {
            Pending = 1,
            InitialPaymentReceived = 2,
            HalfPaymentReceived = 3,
            FullPaymentReceived = 4
        }

        public enum OrderType
        {
            NormalOrder = 1,
            ChildOrder = 2,
            FundOrder = 3

        }

        public enum UserType
        {
            Admin = 1,
            SalesSupport = 2,
            Finance = 3,
            Customer = 4,
        }

        public enum CustomerProductStatus
        {
            SignupDone_OrderPending = 1010,
            OrderInvoiceSubmitted_PaymentPending = 1020,
            PartialPaymentinitialized_ReceivingPending = 1030,
            PartialPaymentReceived_AuctionPending = 1040,
            CompletePaymentinitialized_ReceivingPending = 1050,
            CompletePaymentReceived_AuctionPending = 1060,
            AuctionBiddingRequested = 1070,
            AuctionBiddingWon_ShipmentPending = 1080,
            VehicleShipmentInProgress_ArrivalatPortPending = 1090,
            ShipmentArrivedatPort_RemainingPaymentPending = 1100,
            ShipmentArrivedatPort_CompletePaymentDone_DocumentsShipmentPending = 1110,
            RemainingPaymentInitialized_ReceivingPending = 1120,
            CompletePaymentReceived_DocumentsShipmentPending = 1130,
            DocumentsReceived_VehicleDeliveryPending = 1140,
            VehicleDelivered = 1150

        }

        public const string TelizeJsonCountryData = @"ipcountrydata";
        public const string VarClientCode = @"{{VAR_ClientCode}}";

        public enum QuoteStatus
        {
            Request = 1,
            Recommended = 2
        }
        public enum BidRequestStatus
        {
            Requested = 1,
            Won = 2,
            Lost = 3,
            UnableToBid = 4,
            Other = 5,
            ApprovedbyTC = 6,
            RejectedbyTC = 7,
            ApprovedbyBU = 8,
            RejectedbyBU = 9,
            AttemptedbyBU = 10,

            ApprovedbyInspectionVerificationUser = 11,
            DisaprovebyInspectionVerificationUser = 12,
            Canceled = 13,
            EmailSent = 14,
            Hold = 15,
            RequestedBySubjectToApprovalUser = 16,
            RejectedBySubjectToApprovalUser = 17,
            ReOpenSubjectToApprovalUser = 18,
            ReOpenVerificationUser = 19,
            ReOpenInspectionVerificationUser = 20
        }

        public enum ShipmentCheck
        {
            BeforeShipment = 1,
            AfterShipment = 2,
        }

        public enum InvoiceDocumentStatus
        {
            Pending = 1,
            Generated = 2
        }
        public enum DocumentType
        {

            UnitPictures = 4,
            ShipmentOrderFile = 5,
            ExportCertificate = 6,
            BL = 7,
            ShipmentInvoice = 8,
            ExportLetter = 9,
            InspectionCertificate = 10,
            CarDocument = 15,
            MembershipPlan = 25

        }

        public enum ConsigneeStatus
        {
            Consigneedetailpending = 1,
            Consigneedetailsubmitted = 2,
            Consigneedetailemailedtocustomer = 3,
            Consigneedetailapprovedbycustomer = 4,
            Consigneedetaildisapprovedbycustomer = 5,

        }

        public enum CountryCode
        {
            Japan = 110,
            US = 224
        }
        public enum AuditInquiryCommentTypeId
        {
            AuditInquiryComment = 1,
            ConsigneeFollowUpComment = 2,

        }


        public enum ClientPaymentStatusCode
        {
            PreAuth = 2,
            Captured = 8,
            Refund = 25,
            Expired = 28,
            WireRefundRejected = 30,
        }

        public enum RefundStatus
        {
            Pending = 1,
            Approved = 2,
            Rejected = 3,
        }

        public enum EmailTeplateIdCA
        {
            EmailVerification = 5,
            SignUp = 6,
            ReferralEmail = 7,
            UnitInquiry = 8,
            DocumentUpload = 9,
            CustomerReferralThanks = 10
        }
        public enum LeadDistribution
        {
            MiddleEast = 1,
            MiddleEastII = 2,
            International = 3

        }

        public enum ShipmentServiceStatus
        {
            Pending = 1,
            Error = 2,
            Success = 3,
        }
        public enum AddressType
        {
            Normal = 1,
            Shipment = 2
        }

        public enum ShipmentInvoiceVerificationStatus
        {
            Reject = 0,
            Approve = 1
        }


        public enum OG_UnitTransportationStatus
        {
            BIdWonEmailPending = 100,
            EmailSentCordinationPending = 200,
            CordinationDoneReceivingPending = 300,
            UnitReceivedatYard = 400
        }


        public enum OG_BidStatusType
        {
            EmailPending = 1,
            EmailSentResultPending = 2,
            ResultMarked = 3

        }
        public enum MembershipType
        {
            Green = 1,
            Silver = 2,
            Gold = 3,
            Platinum = 4
        }

        public enum InvoiceType
        {
            MembershipInvoice = 1

        }
        public enum CustomerInvoiceStatus
        {
            Requested = 100,
            PaymentDone = 101
        }
    }
}
