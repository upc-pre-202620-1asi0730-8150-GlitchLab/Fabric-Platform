using Cortex.Mediator.Notifications;
using FabricPlatform.API.Shared.Domain.Model.Events;

namespace FabricPlatform.API.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
}