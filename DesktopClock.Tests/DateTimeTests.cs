﻿using System;

namespace DesktopClock.Tests;

public class DateTimeTests
{
    [Fact]
    public void TimeZones_ShouldContainSystemTimeZones()
    {
        // Act
        var timeZones = DateTimeUtil.TimeZones;

        // Assert
        Assert.NotEmpty(timeZones);
        Assert.Contains(timeZones, tz => tz.Id == "UTC");
    }

    [Theory]
    [InlineData("UTC", true)]
    [InlineData("Pacific Standard Time", true)]
    [InlineData("NonExistentTimeZone", false)]
    public void TryFindSystemTimeZoneById_ShouldReturnExpectedResult(string timeZoneId, bool expectedResult)
    {
        // Act
        var result = DateTimeUtil.TryFindSystemTimeZoneById(timeZoneId, out var timeZoneInfo);

        // Assert
        Assert.Equal(expectedResult, result);
        if (expectedResult)
        {
            Assert.NotNull(timeZoneInfo);
            Assert.Equal(timeZoneId, timeZoneInfo.Id);
        }
        else
        {
            Assert.Null(timeZoneInfo);
        }
    }

    [Theory]
    [InlineData("2024-07-15T00:00:00Z", "00:00:00")]
    [InlineData("2024-07-15T00:00:00Z", "01:00:00")]
    [InlineData("0001-01-01T00:00:00Z", "00:00:00")]
    public void ToDateTimeOffset_ShouldConvertDateTimeToExpectedOffset(string dateTimeString, string offsetString)
    {
        // Arrange
        var dateTime = DateTime.Parse(dateTimeString);
        var offset = TimeSpan.Parse(offsetString);

        // Act
        var dateTimeOffset = dateTime.ToDateTimeOffset(offset);

        // Assert
        Assert.Equal(new DateTimeOffset(dateTime.Ticks, offset), dateTimeOffset);
    }

    [Theory]
    [InlineData("dddd, MMMM dd", "Monday, January 01")]
    [InlineData("yyyy-MM-dd", "2024-01-01")]
    [InlineData("HH:mm:ss", "00:00:00")]
    [InlineData("MMMM dd, yyyy", "January 01, 2024")]
    public void FromFormat_CreatesCorrectExample(string format, string expected)
    {
        // Arrange
        var dateTimeOffset = new DateTime(2024, 01, 01);

        // Act
        var dateFormatExample = DateFormatExample.FromFormat(format, dateTimeOffset);

        // Assert
        Assert.Equal(format, dateFormatExample.Format);
        Assert.Equal(expected, dateFormatExample.Example);
    }
}
