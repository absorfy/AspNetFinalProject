using AspNetFinalProject.DTOs;

namespace AspNetFinalProject.Common;

public interface ILogEntityMapper<out T> where T: ILogUpdateDto
{
    ILogUpdateDto CreateUpdateDto();
}