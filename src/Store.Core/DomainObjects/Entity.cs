using Store.Core.Messages;
using System;
using System.Collections.Generic;

namespace Store.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        private List<Event> _notifications;
        public IReadOnlyCollection<Event> Notifications => _notifications?.AsReadOnly();

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public void AddEvent(Event evnt) {
            _notifications = _notifications ?? new List<Event>();
            _notifications.Add(evnt);
        }

        public void RemoveEvent(Event evnt)
        {
            _notifications?.Remove(evnt);
        }

        public void ClearEvents()
        {
            _notifications?.Clear();
        }
    }
}