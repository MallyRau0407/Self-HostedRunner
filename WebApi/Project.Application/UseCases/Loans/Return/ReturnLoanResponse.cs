namespace Project.Application.UseCases.Loans.Return
{
    public class ReturnLoanResponse
    {
        public Guid LoanId { get; set; }
        public DateTime ReturnedAt { get; set; }
        public bool WasLate { get; set; }
        public Guid? PenaltyId { get; set; }
    }
}
