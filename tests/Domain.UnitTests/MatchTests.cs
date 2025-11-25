using System;
using DatingApp.Domain.Matches;
using DatingApp.Domain.Matches.Events;
using Xunit;
using FluentAssertions;

namespace DatingApp.Domain.UnitTests.Matches
{
    public class MatchTests
    {
        [Fact]
        public void CreateNew_ShouldReturnFailure_WhenPartnerIdsAreTheSame()
        {
            // Arrange
            var partnerId = "user-1";
            var now = DateTime.UtcNow;

            // Act
            var result = Match.CreateNew(partnerId, partnerId, now);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(MatchErrors.CannotMatchSelf);
        }

        [Fact]
        public void CreateNew_ShouldReturnSuccess_WhenPartnerIdsAreDifferent()
        {
            // Arrange
            var partnerOneId = "user-1";
            var partnerTwoId = "user-2";
            var createdAt = DateTime.UtcNow;

            // Act
            var result = Match.CreateNew(partnerOneId, partnerTwoId, createdAt);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();

            var match = result.Value;
            match.PartnerOneId.Should().Be(partnerOneId);
            match.PartnerTwoId.Should().Be(partnerTwoId);
            match.CreatedAt.Should().Be(createdAt);
        }

        [Fact]
        public void CreateNew_ShouldRaiseMatchCreatedDomainEvent()
        {
            // Arrange
            var partnerOneId = "user-1";
            var partnerTwoId = "user-2";
            var createdAt = DateTime.UtcNow;

            // Act
            var result = Match.CreateNew(partnerOneId, partnerTwoId, createdAt);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var match = result.Value;

            match.GetDomainEvents().Should().ContainSingle()
                .Which.Should().BeOfType<MatchCreatedDomainEvent>();

            var domainEvent = (MatchCreatedDomainEvent)match.GetDomainEvents().FirstOrDefault();

            domainEvent.PartnerOneId.Should().Be(partnerOneId);
            domainEvent.PartnerTwoId.Should().Be(partnerTwoId);
            domainEvent.MatchId.Should().Be(match.Id);
        }
    }
}
