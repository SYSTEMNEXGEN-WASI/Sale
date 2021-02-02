using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class TaskVM
    {
    public string DealerCode{ get; set; }             
	public string TaskID{ get; set; }         
	public string TaskCreateDate{ get; set; } 
	public string AssignTo{ get; set; }       
	public string Status{ get; set; }         
	public string Subject{ get; set; }        
	public string Prospect_ID{ get; set; }    
	public string DueDate{ get; set; }        
	public string LeadSourceID{ get; set; }   
	public string Contact{ get; set; }        
	public string Email{ get; set; }          
	public string TaskType{ get; set; }       
	public string Priority{ get; set; }       
	public string Lead_ID{ get; set; }        
	public string Comments{ get; set; }       
	public string Frequency{ get; set; }      
	public string FreqTrun{ get; set; }       
	public string StartDate{ get; set; }      
	public string StartTime{ get; set; }      
	public string EndDate{ get; set; }        
	public string EndTime{ get; set; }        
	public string Ongoing{ get; set; }        
	public string Reminder{ get; set; }       
	public string ReminderTime{ get; set; }   
	public string Active{ get; set; }         
	public string StatusTypeId{ get; set; }   
	public string StatusId{ get; set; }       
	public string SubjectId{ get; set; }  
    public string TaskTypeId{ get; set; }     
    }
}
