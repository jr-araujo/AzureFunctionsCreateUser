using Newtonsoft.Json;
using RestSharp;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RequestFunctionConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://azurerocksfunction.azurewebsites.net");
            var request = new RestRequest("/api/CreateUser", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("x-functions-key", "3HU9oWWo6uhW4J4HkEaMPTvPwuUi3oFNkSAagtHxEU2KzgWaBP47vw==");

            ThreadPool.SetMinThreads(10, 10);
            ParallelEnumerable.Range(1, 1000000)
                .WithDegreeOfParallelism(8)
                .ForAll(i =>
                {
                    request.AddJsonBody(new User
                    {
                        Id = i.ToString(),
                        Idade = 35,
                        FirstName = $"Primeiro Nome [{i}]",
                        SurName = $"Sobrenome do usuário [{i}]",
                        JobTitle = "Developer"
                    });

                    IRestResponse response = client.Execute(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"Usuário[{i}] criado com sucesso");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Problemas ao criar o Usuário[{i}]");
                    }

                    request.Parameters.RemoveAll(x => x.Type == RestSharp.ParameterType.RequestBody);
                    Thread.Sleep(2000);
             });

            Console.WriteLine("Fim da inclusão de usuários !");

            Console.ReadKey();
        }
    }

    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public int? Idade { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string JobTitle { get; set; }
    }
}