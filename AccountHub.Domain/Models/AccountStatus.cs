namespace AccountHub.Domain.Models;

public enum AccountStatus
{
    Available = 0,      
    PendingReview,  
    Sold,           
    Suspended,      
    Banned,         
    Reserved,       
    Expired 
}