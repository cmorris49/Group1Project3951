using System;
using System.Collections;
using System.Reflection;
using System.Text.Json;
using Group1Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Group 1 Project - TournamentJsonTests
/// Author: Cameron, Jun, Jonathan
/// Date: April 9, 2026; Revision: 1.0
/// Source:
///     docment on C# at https://www.w3schools.com/cs/index.php
///     MSTest info https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
/// </summary>
namespace Group1Project.Tests
{
    /// <summary>
    /// Provides focused tests for JSON export/import contract types used by Form1.
    /// </summary>
    [TestClass]
    public class TournamentJsonTests
    {
        /// <summary>
        /// Verifies export file contract initializes nested objects and collections.
        /// </summary>
        [TestMethod]
        public void TournamentExportFile_Defaults_InitializeCollectionsAndTournament()
        {
            Type exportFileType = GetForm1NestedType("TournamentExportFile");
            object exportFile = Activator.CreateInstance(exportFileType)!;

            var tournament = GetPropertyValue(exportFile, "Tournament");
            var teams = (IList)GetPropertyValue(exportFile, "Teams")!;
            var matches = (IList)GetPropertyValue(exportFile, "Matches")!;

            Assert.IsNotNull(tournament);
            Assert.IsNotNull(teams);
            Assert.IsNotNull(matches);
            Assert.AreEqual(0, teams.Count);
            Assert.AreEqual(0, matches.Count);
        }

        /// <summary>
        /// Verifies export contract can serialize and deserialize tournament/team/player/match data.
        /// </summary>
        [TestMethod]
        public void TournamentExportFile_Serialization_RoundTripsCoreData()
        {
            Type exportFileType = GetForm1NestedType("TournamentExportFile");
            Type tournamentType = GetForm1NestedType("TournamentExportDto");
            Type teamType = GetForm1NestedType("TeamExportDto");
            Type playerType = GetForm1NestedType("PlayerExportDto");
            Type matchType = GetForm1NestedType("MatchExportDto");

            object exportFile = Activator.CreateInstance(exportFileType)!;
            DateTime exportedAt = DateTime.UtcNow;

            SetPropertyValue(exportFile, "SchemaVersion", 1);
            SetPropertyValue(exportFile, "ExportedAtUtc", exportedAt);

            object tournament = Activator.CreateInstance(tournamentType)!;
            SetPropertyValue(tournament, "Name", "Spring Invitational");
            SetPropertyValue(tournament, "Location", "Main Arena");
            SetPropertyValue(tournament, "StartDateUtc", new DateTime(2026, 5, 10, 14, 0, 0, DateTimeKind.Utc));
            SetPropertyValue(tournament, "BracketType", "SingleElimination");
            SetPropertyValue(exportFile, "Tournament", tournament);

            object team = Activator.CreateInstance(teamType)!;
            SetPropertyValue(team, "Name", "Warriors");
            SetPropertyValue(team, "Seed", 1);

            object player = Activator.CreateInstance(playerType)!;
            SetPropertyValue(player, "DisplayName", "Alice");
            SetPropertyValue(player, "Number", 7);

            ((IList)GetPropertyValue(team, "Players")!).Add(player);
            ((IList)GetPropertyValue(exportFile, "Teams")!).Add(team);

            object match = Activator.CreateInstance(matchType)!;
            SetPropertyValue(match, "RoundNumber", 1);
            SetPropertyValue(match, "MatchNumber", 1);
            SetPropertyValue(match, "TeamAName", "Warriors");
            SetPropertyValue(match, "TeamBName", "Knights");
            SetPropertyValue(match, "ScheduledStartUtc", new DateTime(2026, 5, 10, 15, 0, 0, DateTimeKind.Utc));
            SetPropertyValue(match, "Status", "Scheduled");
            SetPropertyValue(match, "ScoreA", null);
            SetPropertyValue(match, "ScoreB", null);

            ((IList)GetPropertyValue(exportFile, "Matches")!).Add(match);

            string json = JsonSerializer.Serialize(exportFile, exportFileType);
            Assert.IsFalse(string.IsNullOrWhiteSpace(json));

            using var doc = JsonDocument.Parse(json);
            Assert.AreEqual(1, doc.RootElement.GetProperty("SchemaVersion").GetInt32());
            Assert.AreEqual("Spring Invitational", doc.RootElement.GetProperty("Tournament").GetProperty("Name").GetString());
            Assert.AreEqual(1, doc.RootElement.GetProperty("Teams").GetArrayLength());
            Assert.AreEqual(1, doc.RootElement.GetProperty("Matches").GetArrayLength());

            object? roundTrip = JsonSerializer.Deserialize(json, exportFileType);
            Assert.IsNotNull(roundTrip);

            var roundTripTournament = GetPropertyValue(roundTrip!, "Tournament");
            Assert.AreEqual("Spring Invitational", GetPropertyValue(roundTripTournament!, "Name"));
            Assert.AreEqual("SingleElimination", GetPropertyValue(roundTripTournament!, "BracketType"));
        }

        /// <summary>
        /// Gets a non-public nested type declared inside Form1 by name.
        /// </summary>
        private static Type GetForm1NestedType(string nestedTypeName)
        {
            var type = typeof(Form1).GetNestedType(nestedTypeName, BindingFlags.NonPublic);
            Assert.IsNotNull(type, $"Nested type '{nestedTypeName}' not found.");
            return type!;
        }

        /// <summary>
        /// Reads an instance property value by name.
        /// </summary>
        private static object? GetPropertyValue(object target, string propertyName)
        {
            var property = target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            Assert.IsNotNull(property, $"Property '{propertyName}' not found.");
            return property!.GetValue(target);
        }

        /// <summary>
        /// Sets an instance property value by name.
        /// </summary>
        private static void SetPropertyValue(object target, string propertyName, object? value)
        {
            var property = target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            Assert.IsNotNull(property, $"Property '{propertyName}' not found.");
            property!.SetValue(target, value);
        }
    }
}