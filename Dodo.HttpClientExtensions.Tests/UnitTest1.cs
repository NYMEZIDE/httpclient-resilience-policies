using System;using System.Net.Http;using Microsoft.Extensions.DependencyInjection;using NUnit.Framework;namespace Dodo.HttpClientExtensions.Tests{	internal sealed class TestHttpClient : ITestHttpClient	{			}	internal interface ITestHttpClient	{	}	public class Tests	{		[SetUp]		public void Setup()		{						var services = new ServiceCollection();			services				.AddJsonClient<ITestHttpClient, TestHttpClient>(new Uri("http://localhost:5000"), ClientSettings.Default());			var serviceProvider = services.BuildServiceProvider();			var factory = serviceProvider.GetService<IHttpClientFactory>();		}		[Test]		public void Test1()		{		}	}}