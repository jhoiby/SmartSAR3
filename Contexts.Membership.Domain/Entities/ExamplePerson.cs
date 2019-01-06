﻿using System;
using System.Runtime.CompilerServices;
using SSar.Contexts.Common.AbstractClasses;
using SSar.Contexts.Common.Helpers;
using SSar.Contexts.Common.Notifications;

namespace SSar.Contexts.Membership.Domain.Entities
{
    public class ExamplePerson : AggregateRoot
    {
        public string _name;
        public string _emailAddress;

        private ExamplePerson()
        {
        }
        
        public string Name => _name;
        public string EmailAddress => _emailAddress;

        public static AggregateResult<ExamplePerson> Create(string name, string emailAddress)
        {
            var aggregate = new ExamplePerson();
            var notifications = new NotificationList();

            aggregate.SetName(name).AddNotificationsTo(notifications);
            aggregate.SetEmailAddress(emailAddress).AddNotificationsTo(notifications);

            return AggregateResult<ExamplePerson>.FromConstruction(notifications, aggregate);
        }

        public AggregateResult<ExamplePerson> SetName(string name)
        {
            name = name ?? throw new ArgumentNullException(nameof(name));

            var notifications = new NotificationList();

            if (name.Length == 0)
            {
                notifications.AddNotification("Name is required.", nameof(Name));
            }

            if (notifications.Empty)
            {
                _name = name;
            }

            return AggregateResult<ExamplePerson>.FromNotificationList(notifications);
        }

        public AggregateResult<ExamplePerson> SetEmailAddress(string emailAddress)
        {
            emailAddress = emailAddress ?? throw new ArgumentNullException();

            var notifications = new NotificationList();

            if (emailAddress.Length == 0)
            {
                notifications.AddNotification("Email address is required.", nameof(EmailAddress));
            }

            if (notifications.Empty)
            {
                _emailAddress = emailAddress;
            }

            return AggregateResult<ExamplePerson>.FromNotificationList(notifications);
        }
    }
}
