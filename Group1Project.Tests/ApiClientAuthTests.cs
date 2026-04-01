using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Group1Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Group 1 Project - ApiClientAuthTests
/// Author: Cameron, Jun, Jonathan
/// Date: April 1, 2026; Revision: 1.0
/// Source:
///     docment on C# at https://www.w3schools.com/cs/index.php
///     MSTest info https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
/// </summary>
namespace Group1Project.Tests
{
    /// <summary>
    /// Provides unit tests for ApiClient authentication state and user-header behavior.
    /// </summary>
    [TestClass]
    public class ApiClientAuthTests
    {
        /// <summary>
        /// Verifies a new ApiClient starts in a logged-out state with empty user identity values.
        /// </summary>
        [TestMethod]
        public void Constructor_InitializesLoggedOutState()
        {
            var client = new ApiClient();

            Assert.IsFalse(client.IsLoggedIn);
            Assert.AreEqual(string.Empty, client.CurrentUserId);
            Assert.AreEqual(string.Empty, client.CurrentUserName);
        }

        /// <summary>
        /// Verifies IsLoggedIn returns true when current user identifier is populated.
        /// </summary>
        [TestMethod]
        public void IsLoggedIn_ReturnsTrue_WhenCurrentUserIdIsSet()
        {
            var client = new ApiClient();
            SetPrivateField(client, "_currentUserId", Guid.NewGuid().ToString());

            Assert.IsTrue(client.IsLoggedIn);
        }

        /// <summary>
        /// Verifies Logout clears user identity values and returns client to logged-out state.
        /// </summary>
        [TestMethod]
        public void Logout_ClearsUserIdentityState()
        {
            var client = new ApiClient();
            SetPrivateField(client, "_currentUserId", Guid.NewGuid().ToString());
            SetPrivateField(client, "_currentUserName", "testuser");

            client.Logout();

            Assert.IsFalse(client.IsLoggedIn);
            Assert.AreEqual(string.Empty, client.CurrentUserId);
            Assert.AreEqual(string.Empty, client.CurrentUserName);
        }

        /// <summary>
        /// Verifies ApplyUserHeader adds the expected X-User-Id header when user context exists.
        /// </summary>
        [TestMethod]
        public void ApplyUserHeader_AddsHeader_WhenUserIdExists()
        {
            var client = new ApiClient();
            var userId = Guid.NewGuid().ToString();

            SetPrivateField(client, "_currentUserId", userId);
            InvokePrivateInstance(client, "ApplyUserHeader");

            var httpClient = GetPrivateField<HttpClient>(client, "_httpClient");
            Assert.IsTrue(httpClient.DefaultRequestHeaders.Contains("X-User-Id"));

            var values = httpClient.DefaultRequestHeaders.GetValues("X-User-Id").ToList();
            Assert.AreEqual(1, values.Count);
            Assert.AreEqual(userId, values[0]);
        }

        /// <summary>
        /// Verifies ApplyUserHeader removes any existing X-User-Id header when user context is cleared.
        /// </summary>
        [TestMethod]
        public void ApplyUserHeader_RemovesHeader_WhenUserIdMissing()
        {
            var client = new ApiClient();
            var httpClient = GetPrivateField<HttpClient>(client, "_httpClient");

            SetPrivateField(client, "_currentUserId", Guid.NewGuid().ToString());
            InvokePrivateInstance(client, "ApplyUserHeader");
            Assert.IsTrue(httpClient.DefaultRequestHeaders.Contains("X-User-Id"));

            SetPrivateField(client, "_currentUserId", null);
            InvokePrivateInstance(client, "ApplyUserHeader");

            Assert.IsFalse(httpClient.DefaultRequestHeaders.Contains("X-User-Id"));
        }

        /// <summary>
        /// Reads a private instance field value by name for test inspection.
        /// </summary>
        /// <typeparam name="T">Expected field type.</typeparam>
        /// <param name="target">Target object containing the field.</param>
        /// <param name="fieldName">Private field name.</param>
        /// <returns>The field value cast to the requested type.</returns>
        private static T GetPrivateField<T>(object target, string fieldName)
        {
            var field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, $"Field '{fieldName}' not found.");
            return (T)field!.GetValue(target)!;
        }

        /// <summary>
        /// Sets a private instance field value by name for test setup.
        /// </summary>
        /// <param name="target">Target object containing the field.</param>
        /// <param name="fieldName">Private field name.</param>
        /// <param name="value">Value to assign to the field.</param>
        private static void SetPrivateField(object target, string fieldName, object? value)
        {
            var field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, $"Field '{fieldName}' not found.");
            field!.SetValue(target, value);
        }

        /// <summary>
        /// Invokes a private instance method using reflection.
        /// </summary>
        /// <param name="instance">Object instance containing the method.</param>
        /// <param name="methodName">Method name to invoke.</param>
        /// <param name="args">Arguments passed to the method.</param>
        /// <returns>The method return value.</returns>
        private static object? InvokePrivateInstance(object instance, string methodName, params object[] args)
        {
            var method = instance.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(method, $"Method '{methodName}' not found.");
            return method!.Invoke(instance, args);
        }
    }
}