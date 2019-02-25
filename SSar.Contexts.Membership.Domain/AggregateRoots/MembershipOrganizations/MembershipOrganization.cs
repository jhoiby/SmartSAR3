﻿using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using SSar.Contexts.Common.Domain.AggregateRoots;
using SSar.Contexts.Common.Domain.Notifications;
using SSar.Contexts.Common.Domain.ValueTypes;

namespace SSar.Contexts.Membership.Domain.AggregateRoots.MembershipOrganizations
{
    public class MembershipOrganization : AggregateRoot
    {
        private OrganizationName _name;

        private MembershipOrganization()
        {
        }

        public OrganizationName Name => _name;

        public static AggregateResult<MembershipOrganization> Create(OrganizationName name)
        {
            var memberOrg = new MembershipOrganization();
            var notifications = new NotificationList();
            
            memberOrg.SetOrgName(name);

            if (!notifications.HasNotifications)
            {
                memberOrg.AddEvent(new MembershipOrganizationCreated(memberOrg.Id, memberOrg.Name));
            }

            return memberOrg.OrNotifications(notifications)
                .AsResult<MembershipOrganization>();
        }

        private AggregateResult<MembershipOrganization> SetOrgName(OrganizationName name)
        {
            var requirements = RequirementList.Create()
                .AddExceptionRequirement(
                    () => name != null,
                    new ArgumentNullException(nameof(name)));
            
            Action action = () => _name = name;

            return AggregateExecution
                .CheckRequirements(requirements)
                .ExecuteAction(action)
                .ReturnAggregateResult(this);
        }
    }
}
