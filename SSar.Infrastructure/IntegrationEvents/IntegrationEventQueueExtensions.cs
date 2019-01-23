﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SSar.Data.Outbox;
using SSar.Infrastructure.ServiceBus;

namespace SSar.Infrastructure.IntegrationEvents
{
    public static class IntegrationEventQueueExtensions
    {
        // TODO: Unit test these methods

        public static IIntegrationEventQueue CopyToOutbox(this IIntegrationEventQueue eventQueue, IOutboxService outboxService, TimeSpan validitySpan)
        {
            foreach (var @event in eventQueue)
            {
                outboxService.AddObject(@event.EventId, @event.GetType().ToString(), @event, DateTime.Now + validitySpan);
            }

            return eventQueue;
        }

        public static async Task<List<IIntegrationEvent>> PublishToBus(this IIntegrationEventQueue eventQueue, IServiceBusSender busSender)
        {
            List<IIntegrationEvent> publishedEvents = new List<IIntegrationEvent>();

            foreach (var @event in eventQueue)
            {
                // TODO: Batch these message sends in single awaiter to improve performance
                // TODO: Handle exceptions

                await busSender.SendAsync(@event);
                publishedEvents.Add(@event);
            }

            foreach (var @event in publishedEvents)
            {
                eventQueue.Remove(@event);
            }

            return publishedEvents;
        }

        public static void RemoveFromOutbox(this List<IIntegrationEvent> integrationEvents,
            IOutboxService outboxService)
        {
            outboxService.Delete(
                integrationEvents.Select(e => e.EventId).ToArray());
        }
    }
}