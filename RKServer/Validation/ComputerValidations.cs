using FluentValidation;
using Server.Models.Request;

namespace RKServer.Validation
{
    public class ComputerValidations : AbstractValidator<ComputerRequest>
    {
        public ComputerValidations()
        {
            RuleFor(x => x.ComputerId).GreaterThan(0);
            RuleFor(x => x.ComputerBrand).MaximumLength(25);
            RuleFor(x => x.ComputerBrand).MinimumLength(5);
        }
    }
}
