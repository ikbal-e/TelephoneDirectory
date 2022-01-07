using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Contact.API.Infrastructure.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var failures = Validate(request);

        if (failures.Any())
        {
            var failureMessage = string.Join(". ", failures.Select(x => x.ErrorMessage));

            throw new ValidationException(failureMessage);
        }

        return await next();
    }

    public IEnumerable<ValidationFailure> Validate(TRequest request)
    {
        var context = new ValidationContext<TRequest>(request);
        var failures = _validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .ToList();

        return failures;
    }
}