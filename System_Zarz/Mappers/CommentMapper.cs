using Riok.Mapperly.Abstractions;
using System_Zarz.DTOs;
using System_Zarz.Data;


namespace System_Zarz.Mappers;
[Mapper]
public partial class CommentMapper
{
    public partial CommentDto ToDto(Comment comment);
    public partial Comment ToEntity(CommentDto dto);
}