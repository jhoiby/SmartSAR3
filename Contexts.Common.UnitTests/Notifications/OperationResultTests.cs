﻿using SSar.Contexts.Common.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;
using Xunit;

namespace SSar.Contexts.Common.UnitTests.Results
{
    public class OperationResultTests
    {
        [Fact]
        public void CreateSuccessful_sets_status_to_successful()
        {
            var result = OperationResult.CreateSuccessful();
            result.Status.ShouldBe(OperationResultStatus.Successful);
        }

        [Fact]
        public void FromMessage_adds_notification_with_message()
        {
            var result = OperationResult.FromMessage("message1", "field1");

            result.Notifications.First().Message.ShouldBe("message1");
            result.Notifications.First().ForField.ShouldBe("field1");
            result.Status.ShouldBe(OperationResultStatus.HasNotifications);
        }

        [Fact]
        public void FromException_returns_exception_notification_and_status()
        {
            var exception = new ArgumentNullException("You screwed up.");

            var result = OperationResult.FromException(exception, "The programmer screwed up.");

            result.ShouldSatisfyAllConditions(
                () => result.Notifications.First().Message.ShouldBe("The programmer screwed up."),
                () => result.Notifications.First().ForField.ShouldBe("ExceptionInfo"),
                () => result.Exception.ShouldBe(exception)
                );
        }

        [Fact]
        public void FromException_returns_no_notifications_if_no_message()
        {
            var exception = new ArgumentNullException("You screwed up.");

            var result = OperationResult.FromException(exception);

            result.Notifications.ShouldBeEmpty();
        }

        [Fact]
        public void FromNotificationList_returns_notifications_and_status()
        {
            var notifications = new NotificationList();
            notifications.Add(new Notification("message1", "key1"));

            var result = OperationResult.FromNotificationList(notifications);

            result.ShouldSatisfyAllConditions(
                () => result.Notifications.Count.ShouldBe(1),
                () => result.Status.ShouldBe(OperationResultStatus.HasNotifications)
                );
        }
    }
}