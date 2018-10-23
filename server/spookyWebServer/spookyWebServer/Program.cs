using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Threading;

namespace spookyWebServer
{
    public static class Program
    {
        static HttpListener http = new HttpListener();
        static List<duel> activeDuelChallenges = new List<duel>();
        static Dictionary<int, List<notification>> _notifications = new Dictionary<int, List<notification>>();
        static PartyGoer[] _players = new PartyGoer[]{};
        static int Main(string[] args)
        {
            var testNote = new notification();
            testNote.text = "Crystal ball is working";
            _notifications.Add(0, new List<notification> { testNote });
            /*
            var playermock = new PartyGoer();
            playermock.id = 2;
            playermock.level = 4;
            playermock.characterName = "will";
            playermock.inventory = new int[] { 10, 42, 3, 4, 5, 2, 0, 0, 0, 0 };
            _players[0]= playermock;
            */


            #region setup
            var playerDb = File.Open("players.json", FileMode.OpenOrCreate);
            //json.write(playerDb, _players);
            _players = (json.read<PartyGoer[]>(playerDb)??new PartyGoer[] { });
            playerDb.Close();

            var duelDb = File.Open("ActiveDuels.json", FileMode.OpenOrCreate);
            activeDuelChallenges = (json.read<duel[]>(duelDb) ?? new duel[] { }).ToList();
            duelDb.Close();

            Console.WriteLine("Loaded " + _players.Length + " players, " + _notifications.Count + " notification lists, and " + activeDuelChallenges.Count + " duels from files.");
            #endregion

            http.Start();
            http.Prefixes.Add("http://localhost:8220/");
            while (true)
            {
                var context = http.GetContext();
                var request = context.Request;
                context.Response.Headers.Add("Access-Control-Allow-Origin: *");
                context.Response.Headers.Add("Cache-Control: max-age=0");
                if (request.HttpMethod == "OPTIONS")
                {
                    options(context);
                }
                else switch (request.Url.Segments[1])
                    {
                        case "duel":
                            duel(context); break;
                        case "duels":
                            duels(context); break;
                        case "notifications":
                            notifications(context);
                            break;
                        case "players":
                            players(context);
                            break;
                    }
                context.Response.Close();
            }
        }

        private static void players(HttpListenerContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    json.write(context.Response.OutputStream, _players);
                    break;
            }
        }

        private static void notifications(HttpListenerContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    var user = int.Parse(context.Request.QueryString[0]);
                    json.write(context.Response.OutputStream, _notifications[user]);
                    break;
            }
        }

        private static void options(HttpListenerContext context)
        {
            var response = context.Response;
            byte[] body = { };
            response.Headers.Add("Access-Control-Allow-Methods: PUT, GET, POST, DELETE, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers: Content-Type, origin");
            response.OutputStream.Write(body, 0, 0);
        }

        private static void duel(HttpListenerContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "PUT":
                    var challenge = json.read<duel>(context.Request.InputStream);
                    activeDuelChallenges.Add(challenge);
                    foreach (var x in _notifications)
                    {
                        if (x.Key == challenge.target)
                        {
                            var note = new notification();
                            var srcName = _players[challenge.src].characterName;
                            var dstName = _players[challenge.target].characterName;
                            note.text = srcName + " has challenged " + dstName + " to a duel!";
                            _notifications[x.Key].Add(note);
                        }
                        else
                        {
                            var note = new notification();
                            note.text = _players[challenge.src].characterName + " has challenged you to a duel!";
                            _notifications[x.Key].Add(note);
                        }
                    }
                    context.Response.Close();
                    var db = File.Open("duelChallenges.json", FileMode.OpenOrCreate);
                    json.write(db, activeDuelChallenges);
                    db.Close();
                    break;
                case "DELETE":
                    break;
                case "GET":
                    var playerId = int.Parse(context.Request.QueryString["playerId"]);
                    json.write(context.Response.OutputStream, activeDuelChallenges.Where(x=>x.target==playerId));
                    break;
            }
        }

        private static void duels(HttpListenerContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    var playerId = int.Parse(context.Request.QueryString["playerId"]);
                    json.write(
                        context.Response.OutputStream,
                        activeDuelChallenges.Where(x=>
                            x.src==int.Parse( 
                                context.Request.QueryString["duelId"])
                                ));
                    break;
            }
        }

    }
}
