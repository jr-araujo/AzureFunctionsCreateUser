#load "CreateUserService.csx"

using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    var teste = GetEnvironmentVariable("TesteKey");

    var service = new CreateUserService();
    return await service.CreateUser(req);

    //string DB = "Users";
    //string COLLECTION = "UsersCollection";
    //string ENDPOINT = "https://azurerocks.documents.azure.com:443/";
    //string KEY = "U72NRQUyVBiyxg1vfvNiep5utk8dDfNIxaTozsRAWudpSnuHYfJxTD5vYgQKPtUeTC1CFf54AnycFtX6DqjQLQ==";

    //User data = await req.Content.ReadAsAsync<User>();
    //string id = data?.Id;
    //if (string.IsNullOrEmpty(id))
    //    return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass an Id in the request body");
    //DocumentClient client = new DocumentClient(new Uri(ENDPOINT), KEY);
    //try
    //{
    //    await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DB, COLLECTION, id));

    //    return req.CreateResponse(HttpStatusCode.Conflict, "This user already exists in the database.");
    //}
    //catch (DocumentClientException ex)
    //{
    //    if (ex.StatusCode == HttpStatusCode.NotFound)
    //    {
    //        User u = new User();
    //        u.Id = data?.Id;
    //        u.JobTitle = data?.JobTitle;
    //        await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DB, COLLECTION), u);

    //        return req.CreateResponse(HttpStatusCode.OK, "The following user was created successfully: " + id);
    //    }
    //    else
    //    {
    //        return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body");
    //    }
    //}
}

public static string GetEnvironmentVariable(string name)
{
    return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
}

//public class User
//{
//    [JsonProperty(PropertyName = "id")]
//    public string Id { get; set; }
//    public string JobTitle { get; set; }
//}