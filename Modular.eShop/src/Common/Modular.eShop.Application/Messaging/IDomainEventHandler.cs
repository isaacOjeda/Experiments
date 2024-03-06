using MediatR;
using Modular.eShop.Domain.Primitives;

namespace Modular.eShop.Application.Messaging;

/// <summary>
/// Represents the domain event handler interface.
/// </summary>
/// <typeparam name="TEvent">The event type.</typeparam>
public interface IDomainEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}
