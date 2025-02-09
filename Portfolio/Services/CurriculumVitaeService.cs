﻿using Blazored.LocalStorage;
using Portfolio.Errors;
using Portfolio.Models;
using Portfolio.Utils;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace Portfolio.Services
{
    public interface ICurriculumVitaeService
    {
        Task<Response<CurriculumVitae>> GetCVAsync();
        Task<Response<bool>> SaveCVAsync(CurriculumVitae cv);
    }

    internal class CurriculumVitaeService(ILogger<CurriculumVitaeService> logger, HttpClient httpClient, ILocalStorageService localStorage) : ICurriculumVitaeService
    {
        private readonly ILogger<CurriculumVitaeService> _logger = logger;
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILocalStorageService _localStorage = localStorage;

        public async Task<Response<CurriculumVitae>> GetCVAsync()
        {
            CurriculumVitae? cv = await TryToGetFromLocalStorageAsync();

            if (cv != null && cv.LastUpdate == DateHelper.Today())
            {
                return new Response<CurriculumVitae>(cv);
            }

            StringBuilder? errorMessage = null;
            string? warningMessage = null;
            try
            {
                CurriculumVitae? latestCV = await _httpClient.GetFromJsonAsync<CurriculumVitae>("cv");

                if (latestCV == null)
                {
                    throw new ClientAppException("Retrieved CV was null");
                }
                else
                {
                    await SaveToLocalStorageAsync(latestCV);
                    cv = latestCV;
                }
            }
            catch (Exception ex)
            {
                errorMessage = new("Request failed to load the CV.");
                TryAddStatusCodeToErrorMessage(ex, ref errorMessage);
                _logger.LogError(ex, "[Request failed] to [GET] CV");

                if (cv != null)
                {
                    warningMessage = "Failed to retrieved the latest CV version. Displaying an older version viewed before.";
                }
            }

            return new Response<CurriculumVitae>(cv, errorMessage?.ToString(), warningMessage);
        }

        public Task<Response<bool>> SaveCVAsync(CurriculumVitae cv)
        {
            throw new NotImplementedException();
        }

        private async Task SaveToLocalStorageAsync(CurriculumVitae cv)
        {
            try
            {
                cv.LastUpdate = DateHelper.Today();
                await _localStorage.SetItemAsync(Constants.LocalStorage.CvKey, cv);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save [CV] to [local storage]");
            }
        }

        private async Task<CurriculumVitae?> TryToGetFromLocalStorageAsync()
        {
            try
            {
                return await _localStorage.GetItemAsync<CurriculumVitae>(Constants.LocalStorage.CvKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to read [CV] form [local storage]");
                return null;
            }
        }

        private static void TryAddStatusCodeToErrorMessage(Exception ex, ref StringBuilder errorMessage)
        {
            if (ex is HttpRequestException)
            {
                HttpStatusCode? statusCode = (ex as HttpRequestException)?.StatusCode;
                if (statusCode != null)
                {
                    errorMessage.Append($" (Status code: {(int)statusCode})");
                }
            }
        }
    }
}
