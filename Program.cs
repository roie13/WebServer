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
    teachers.Add("liora", new Teacher("ליאורה בוגומולניק", "https://i.imgur.com/w5y3Anu.jpeg", "ליאורה מורה שמלדמת מתמטיקה ברמה גבוהה לתלמידי התיכון. היא מורה מאוד מאוד קשוחה באופי ובעלת חדות מטרה ללימודים ופחות צחוקים. ליאורה שונה בשיטות הלימוד שלה משאר המורים אבל השיטות שלה מובילות לאחוז ההצטיינות הגבוה ביותר של תלמידים.", votes[0]));
    teachers.Add("hadar", new Teacher("הדר בוצר", "https://i.imgur.com/HOvwHhQ.jpeg", "הדר מורה אהובה על כולם. מלמדת את המקצוע היסטוריה. שיטת הלימוד שלה היא מאוד כיפית שמחה ואנרגטית בשונה ממורות אחרות. הדר מאמינה ביחס מורה-תלמיד שאם התלמידים מכבדים אותה היא תחזיר בחזרה ואפילו כפול. הדר מבינה את הקושי של התלמידים בלמידה ממושכת ומעניקה לנו הפסקות בין השיעורים אם אנחנו מתנהגים יפה", votes[1]));
    teachers.Add("orit", new Teacher("אורית מילר", "https://i.imgur.com/FMYN5zf.jpeg", "אורית היא מורה שמלמדת אזרחות והיסטוריה בחטיבת הביניים. היא מורה מאוד קשוחה אבל הקשיחות עוזרת לחנך את התלמידים ולהכין אותם לעולם הגדול. אורית היא דוקטור להיסטוריה ובעלת הרבה תארים שעליהם היא עושה הרצאות ומלמדת אנשים.", votes[2]));

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
        try
        {
          string query = request.Url.Query;
          Console.WriteLine(query);
          var parsed = HttpUtility.ParseQueryString(query);
          string teacherName = parsed["teacher"]!;

          Teacher teacher = teachers[teacherName];

          string teachersJson = JsonSerializer.Serialize(teacher);
          byte[] teachersBytes = Encoding.UTF8.GetBytes(teachersJson);
          response.OutputStream.Write(teachersBytes);
        }
        catch {
          Console.WriteLine("Couldn't find teacher in dictonary");
        }
      }

      response.Close();
    }
  }
  public static string GetBody(HttpListenerRequest request)
  {
    return new StreamReader(request.InputStream).ReadToEnd();
  }
}

class Teacher(string name, string image, string description, int score)
{
  public string Name { get; set; } = name;
  public string Image { get; set; } = image;
  public string Description { get; set; } = description;
  public int Score { get; set; } = score;

}