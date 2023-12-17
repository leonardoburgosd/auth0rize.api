using cens.auth.drive.Entities;
using Newtonsoft.Json;
using RestSharp;

namespace cens.auth.drive;
public static class restClient<T>
{
    public static ResponseDrive<T> post<F>(string url, string token, F objectRequest) where F : class
    {
        ResponseDrive<T>? request = new ResponseDrive<T>();

        RestClient client = new RestClient(url);
        client.Timeout = -1;
        RestRequest restRequest = new RestRequest(Method.POST);
        restRequest.AddHeader("Authorization", "Bearer " + token);
        restRequest.AddHeader("Content-Type", "application/json");
        restRequest.AddParameter("application/json", JsonConvert.SerializeObject(objectRequest), ParameterType.RequestBody);
        IRestResponse restResponse = client.Execute(restRequest);
        if (restResponse.IsSuccessful)
            request = JsonConvert.DeserializeObject<ResponseDrive<T>>(restResponse.Content);

        return request!;
    }

    public static ResponseDrive<T> get(string url, string token)
    {
        ResponseDrive<T>? request = new ResponseDrive<T>();

        RestClient client = new RestClient(url);
        client.Timeout = -1;
        RestRequest restRequest = new RestRequest(Method.GET);
        restRequest.AddHeader("Authorization", "Bearer " + token);
        IRestResponse restResponse = client.Execute(restRequest);
        if (restResponse.IsSuccessful)
            request = JsonConvert.DeserializeObject<ResponseDrive<T>>(restResponse.Content);
        return request!;
    }
}
