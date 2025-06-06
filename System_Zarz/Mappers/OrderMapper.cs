using Riok.Mapperly.Abstractions;
using System_Zarz.DTOs;
using System_Zarz.Data;


namespace System_Zarz.Mappers;
[Mapper]
public partial class OrderMapper
{
    public partial OrderDto ToDto(Order order);
    public partial Order ToEntity(OrderDto dto);
}

