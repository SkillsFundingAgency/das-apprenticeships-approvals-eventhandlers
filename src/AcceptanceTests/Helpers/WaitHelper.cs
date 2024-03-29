﻿using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.AcceptanceTests.Helpers;

public static class WaitHelper
{
    private static WaitConfiguration Config => new();

    public static async Task WaitForIt(Func<bool> lookForIt, string failText)
    {
        var endTime = DateTime.Now.Add(Config.TimeToWait);

        while (DateTime.Now <= endTime)
        {
            if (lookForIt()) return;

            await Task.Delay(Config.TimeToPause);
        }

        Assert.Fail($"{failText}  Time: {DateTime.Now:G}.");
    }
}

public class WaitConfiguration
{
    public TimeSpan TimeToWait { get; set; } = TimeSpan.FromSeconds(40);
    public TimeSpan TimeToPause { get; set; } = TimeSpan.FromMilliseconds(10);
}