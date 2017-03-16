#load "FunctionResult.csx"

using System;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

public class CreateUserService
{
    private readonly string DB = Environment.GetEnvironmentVariable("DocumentDBDatabase", EnvironmentVariableTarget.Process);
    private readonly string COLLECTION = Environment.GetEnvironmentVariable("DocumentDBCollection", EnvironmentVariableTarget.Process);
    private readonly string ENDPOINT = Environment.GetEnvironmentVariable("DocumentDBEndpoint", EnvironmentVariableTarget.Process);
    private readonly string KEY = Environment.GetEnvironmentVariable("DocumentDBKey", EnvironmentVariableTarget.Process);

    private DocumentClient client;

    public CreateUserService()
    {
        client = new DocumentClient(new Uri(ENDPOINT), KEY);
    }
    
    public async Task<HttpResponseMessage> CreateUser(HttpRequestMessage req)
    {
        User data = await req.Content.ReadAsAsync<User>();
        string id = data?.Id;
        if (string.IsNullOrEmpty(id))
            return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass an Id in the request body");

        try
        {
            await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DB, COLLECTION, id));

            return req.CreateResponse<object>(HttpStatusCode.Conflict, new FunctionResult ("This user already exists in the database.").SetError());
        }
        catch (DocumentClientException ex)
        {
            if (ex.StatusCode == HttpStatusCode.NotFound)
            {
                User user = new User();
                user.Id = data?.Id;
                //user.Idade = data?.Idade;
                user.FirstName = data?.FirstName;
                user.SurName = data?.SurName;
                user.JobTitle = data?.JobTitle;

                await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DB, COLLECTION), user);

                return req.CreateResponse(HttpStatusCode.OK, new FunctionResult($"The following user was created successfully: {user.SurName}, {user.FirstName}"));
            }
            else
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, new FunctionResult("Please pass a name on the query string or in the request body").SetError());
            }
        }
    }
}

private class User
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }
    //public int Idade { get; set; }
    public string FirstName { get; set; }
    public string SurName { get; set; }
    public string JobTitle { get; set; }
}