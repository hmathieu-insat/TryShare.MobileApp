﻿using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Net.Http;
using INSAT._4I4U.TryShare.MobileApp.Exceptions;

namespace INSAT._4I4U.TryShare.MobileApp.Services.RequestProvider;

public class RequestProvider : IRequestProvider
{
    private readonly Lazy<HttpClient> _httpClient =
        new(() =>
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        },
            LazyThreadSafetyMode.ExecutionAndPublication);

    public async Task<TResult?> GetAsync<TResult>(Uri uri, string token = "")
    {
        HttpClient httpClient = GetOrCreateHttpClient(token);
        HttpResponseMessage response = await httpClient.GetAsync(uri).ConfigureAwait(false);

        try
        {
            await RequestProvider.HandleResponse(response).ConfigureAwait(false);
        }
        catch (ArgumentNullException)
        {
            return default;
        }
        var result = await response.Content.ReadFromJsonAsync<TResult?>();

        return result;
    }

    public async Task<TResult?> PostAsync<TResult>(Uri uri, TResult data, string token = "", string header = "", bool shouldReturnContent = false)
    {
        HttpClient httpClient = GetOrCreateHttpClient(token);

        if (!string.IsNullOrEmpty(header))
        {
            RequestProvider.AddHeaderParameter(httpClient, header);
        }

        var content = new StringContent(JsonSerializer.Serialize(data));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await httpClient.PostAsync(uri, content).ConfigureAwait(false);

        await RequestProvider.HandleResponse(response).ConfigureAwait(false);

        TResult? result = default;

        if (shouldReturnContent)
        {
            result = await response.Content.ReadFromJsonAsync<TResult?>();
        }
        
        return result;
    }

    public async Task<TResult?> PostAsync<TResult>(Uri uri, string data, string clientId, string clientSecret)
    {
        HttpClient httpClient = GetOrCreateHttpClient(string.Empty);

        if (!string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret))
        {
            RequestProvider.AddBasicAuthenticationHeader(httpClient, clientId, clientSecret);
        }

        var content = new StringContent(data);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        HttpResponseMessage response = await httpClient.PostAsync(uri, content).ConfigureAwait(false);
        
        await HandleResponse(response).ConfigureAwait(false);
        var result = await response.Content.ReadFromJsonAsync<TResult?>();

        return result;
    }

    public async Task<TResult?> PutAsync<TResult>(Uri uri, TResult data, string token = "", string header = "")
    {
        HttpClient httpClient = GetOrCreateHttpClient(token);

        if (!string.IsNullOrEmpty(header))
        {
            RequestProvider.AddHeaderParameter(httpClient, header);
        }

        var content = new StringContent(JsonSerializer.Serialize(data));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await httpClient.PutAsync(uri, content).ConfigureAwait(false);
        
        await RequestProvider.HandleResponse(response).ConfigureAwait(false);
        var result = await response.Content.ReadFromJsonAsync<TResult?>();

        return result;
    }

    public async Task DeleteAsync(Uri uri, string token = "")
    {
        HttpClient httpClient = GetOrCreateHttpClient(token);
        await httpClient.DeleteAsync(uri).ConfigureAwait(false);
    }

    private HttpClient GetOrCreateHttpClient(string token = "")
    {
        var httpClient = _httpClient.Value;

        httpClient.DefaultRequestHeaders.Authorization =
            !string.IsNullOrEmpty(token)
                ? new AuthenticationHeaderValue("Bearer", token)
                : null;

        return httpClient;
    }

    private static void AddHeaderParameter(HttpClient httpClient, string parameter)
    {
        if (httpClient == null)
            return;

        if (string.IsNullOrEmpty(parameter))
            return;

        httpClient.DefaultRequestHeaders.Add(parameter, Guid.NewGuid().ToString());
    }

    private static void AddBasicAuthenticationHeader(HttpClient httpClient, string clientId, string clientSecret)
    {
        //if (httpClient == null)
        //    return;

        //if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
        //    return;

        //httpClient.DefaultRequestHeaders.Authorization = new Authent(clientId, clientSecret);
        Debug.WriteLine("Va te faire toi et toutes tes dependencies, acculé!!!!");
    }

    private static async Task HandleResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Forbidden ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ServiceAuthentificationException(content);
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ArgumentNullException(nameof(response));
            }

            throw new HttpRequestExceptionEx(response.StatusCode, content);
        }
    }
}