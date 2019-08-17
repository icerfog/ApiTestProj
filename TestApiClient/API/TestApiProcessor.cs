using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TestApiClient
{
	public abstract class TestApiBase
	{
		protected HttpClient ApiClient=new HttpClient();
		protected void Initialize(Uri baseAdress)
		{
			ApiClient.BaseAddress = baseAdress;
			ApiClient.DefaultRequestHeaders.Accept.Clear();
			ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			ServicePointManager.FindServicePoint(baseAdress).ConnectionLeaseTimeout = 60*1000;
		}

		public async Task<T> GetRequest<T>(string url)
		{
			using (HttpResponseMessage response = await ApiClient.GetAsync(url))
			{
				if (response.IsSuccessStatusCode)
				{
					T res = await response.Content.ReadAsAsync<T>();
					return res;
				}
				throw new Exception(response.ReasonPhrase);
			}
		}

		public async Task<T> PostRequest<T>(string url, T entity)
		{
			using (HttpResponseMessage response = await ApiClient.PostAsJsonAsync<T>(url, entity))
			{
				if (response.IsSuccessStatusCode)
				{
					T res = await response.Content.ReadAsAsync<T>();
					return res;
				}
				throw new Exception(response.ReasonPhrase);
			}
		}

		public async Task PutRequest<T>(string url, T entity)
		{
			using (HttpResponseMessage response = await ApiClient.PutAsJsonAsync(url, entity))
			{
				if (response.IsSuccessStatusCode)
				{
					return;
				}
				throw new Exception(response.ReasonPhrase);
			}
		}

		public async Task DeleteRequest(string url)
		{
			using (HttpResponseMessage response = await ApiClient.DeleteAsync(url))
			{
				if (response.IsSuccessStatusCode)
				{
					return;
				}
				throw new Exception(response.ReasonPhrase);
			}
		}
	}
}
