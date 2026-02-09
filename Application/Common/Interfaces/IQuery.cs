using MediatR;

namespace MyPortfolio.Application.Common.Interfaces
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}
