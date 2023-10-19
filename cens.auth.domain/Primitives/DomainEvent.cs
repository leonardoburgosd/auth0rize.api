using MediatR;

namespace cens.auth.domain.Primitives;
public record DomainEvent(Guid Id):INotification;
