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
                User u = new User();
                u.Id = data?.Id;
                u.JobTitle = data?.JobTitle;
                await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DB, COLLECTION), u);

                return req.CreateResponse(HttpStatusCode.OK, "The following user was created successfully: " + id);
            }
            else
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body");
            }
        }
    }
}

private class User
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }
    public string JobTitle { get; set; }
}