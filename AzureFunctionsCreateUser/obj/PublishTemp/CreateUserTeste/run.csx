#load "CreateUserService.csx"

using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    //--------------------------------------------------------------//
    //                      1ºnd Phase                              //
    //--------------------------------------------------------------//
    //string DB = "usersdb";
    //string COLLECTION = "users";
    //string ENDPOINT = "https://codificandoweek2017.documents.azure.com:443/";
    //string KEY = "Nd3Uf76rCxaFbPmmVRRGNDoo3hzsV0L5iE61wQqeMktb4pkgA8GMzZJk3xQpBoyqCt84YDXFcdZAkXdiD6Rb2g==";

    //dynamic data = await req.Content.ReadAsAsync<object>();
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
    //        User user = new User();
    //        user.Id = data?.Id;
    //        user.Idade = data?.Idade;
    //        user.FirstName = data?.FirstName;
    //        user.SurName = data?.SurName;
    //        user.JobTitle = data?.JobTitle;

    //        await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DB, COLLECTION), user);

    //        return req.CreateResponse(HttpStatusCode.OK, $"The following user was created successfully: {user.SurName}, {user.FirstName}");
    //    }
    //    else
    //    {
    //        return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body");
    //    }
    //}
    //--------------------------------------------------------//

    //--------------------------------------------------------//
    //                      2ºnd Phase                        //
    //--------------------------------------------------------//
    var service = new CreateUserService();
    return await service.CreateUser(req);
    //--------------------------------------------------------//
}