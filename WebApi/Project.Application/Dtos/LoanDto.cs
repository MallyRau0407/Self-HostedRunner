using System;

namespace Project.Application.Dtos
{
    public record LoanDto(Guid Id, Guid UserId, Guid ResourceId, DateTime StartDate, DateTime DueDate, DateTime? ReturnDate, string Status);
}
