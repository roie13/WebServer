using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.Json;
using System.Web;




class Program
{
  public static void Main()
  {
    int[] votes = [0, 0, 0];
    Dictionary<string, Teacher> teachers = new Dictionary<string, Teacher>();

    HttpListener listener = new();
    listener.Prefixes.Add("http://*:5000/");
    listener.Start();

    Console.WriteLine("Server started. Listening for requests...");
    Console.WriteLine("Main page on http://localhost:5000/website/index.html");

    while (true)
    {
      HttpListenerContext context = listener.GetContext();
      HttpListenerRequest request = context.Request;
      HttpListenerResponse response = context.Response;

      string rawPath = request.RawUrl!;
      string absPath = request.Url!.AbsolutePath;

      Console.WriteLine($"Received a request with path: " + rawPath);

      string filePath = "." + absPath;
      bool isHtml = request.AcceptTypes!.Contains("text/html");

      if (File.Exists(filePath))
      {
        byte[] fileBytes = File.ReadAllBytes(filePath);
        if (isHtml) { response.ContentType = "text/html; charset=utf-8"; }
        response.OutputStream.Write(fileBytes);
      }
      else if (isHtml)
      {
        response.StatusCode = (int)HttpStatusCode.Redirect;
        response.RedirectLocation = "/website/404.html";
      }
      else if (absPath == "/addVote")
      {
        int voteIndex = int.Parse(GetBody(request));
        votes[voteIndex]++;
        Console.WriteLine(votes[voteIndex]);
      }
      else if (absPath == "/getVotes")
      {
        string votesJson = JsonSerializer.Serialize(votes);
        byte[] votesBytes = Encoding.UTF8.GetBytes(votesJson);
        response.OutputStream.Write(votesBytes);
      }
      else if (absPath == "/addPointsVote")
      {
        int voteIndex = int.Parse(GetBody(request));
        votes[voteIndex]++;
        Console.WriteLine(votes[voteIndex]);
      }
      else if (absPath == "/getPointsVotes")
      {
        string votesJson = JsonSerializer.Serialize(votes);
        byte[] votesBytes = Encoding.UTF8.GetBytes(votesJson);
        response.OutputStream.Write(votesBytes);
      }
      else if (absPath == "/getTeacher")
      {
        string query = request.Url.Query;
        Console.WriteLine(query);
        var parsed = HttpUtility.ParseQueryString(query);
        string teacherName = parsed["teacher"]!;

        // string votesJson = JsonSerializer.Serialize(votes);
        // byte[] votesBytes = Encoding.UTF8.GetBytes(votesJson);
        // response.OutputStream.Write(votesBytes);
      }
      
      response.Close();
    }
  }
  public static string GetBody(HttpListenerRequest request)
  {
    return new StreamReader(request.InputStream).ReadToEnd();
  }
}

class Teacher(string name,string image, string description){
string name = name;
string image = image;
string description = description;
}