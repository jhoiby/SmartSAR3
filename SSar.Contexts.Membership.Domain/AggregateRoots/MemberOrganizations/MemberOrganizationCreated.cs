﻿using SSar.Contexts.Common.Domain.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;
using SSar.Contexts.Common.Domain.ValueTypes;
using SSar.Contexts.Common.Helpers;

namespace SSar.Contexts.Membership.Domain.AggregateRoots.MemberOrganizations
{
    [Serializable]
    public class MemberOrganizationCreated : DomainEvent
    {
        public MemberOrganizationCreated(Guid id, OrganizationName name)
        {
           Id = id.Require(nameof(id));
           FullName = name.FullName.Require(nameof(name.FullName));
           Nickname = name.Nickname.Require(nameof(name.Nickname));
           ReportingCode = name.ReportingCode.Require(nameof(name.ReportingCode));
        }

        public Guid Id { get;  }
        public string FullName { get; }
        public string Nickname { get; }
        public string ReportingCode { get; }
    }
}

