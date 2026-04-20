using Fly.Framework.Shared.Common.Dtos;

namespace Leapay.Client.ClientModels.Dto.DtoModels.PersonnelModels;

public class AttendanceDepartureDataModel: DtoBase
{
    public string? EmployeeName { get; set; }
    
    public DateTime AttendanceDate { get; set; }
    
    public DateTime DepartureDate { get; set; }
}