using Riok.Mapperly.Abstractions;
using System_Zarz.DTOs;
using System_Zarz.Data;

namespace System_Zarz.Mappers;

[Mapper]
public partial class VehicleMapper
{
    public partial VehicleDto ToDto(Vehicle vehicle);
    public partial Vehicle ToEntity(VehicleDto dto);
}