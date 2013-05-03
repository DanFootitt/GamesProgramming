using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace ZombieGame
{
    static class HighScores
    {

        public static HighScoreTable getScores(string format)
        {
            string postString = "";
            return parseToHighscoreTable(webPost("http://zisforzombie.danfootitt.co.uk/requestscores.php", postString));
        }

        static HighScoreTable parseToHighscoreTable(string tableString)
        {
            const string SERVER_VALID_DATA_HEADER = "SERVER_";
            if (tableString.Trim().Length < SERVER_VALID_DATA_HEADER.Length ||
            !tableString.Trim().Substring(0, SERVER_VALID_DATA_HEADER.Length).Equals(SERVER_VALID_DATA_HEADER)) return null;
            string toParse = tableString.Trim().Substring(SERVER_VALID_DATA_HEADER.Length);
            string[] names = new string[10];
            int[] scores = new int[10];

            string[] rows = Regex.Split(toParse, "_ROW_");
            for (int i = 0; i < 10; i++)
            {
                if (rows.Length > i && rows[i].Trim() != "")
                {
                    string[] cols = Regex.Split(rows[i], "_COL_");
                    if (cols.Length == 3)
                    {
                        names[i] = cols[0].Trim();
                        scores[i] = int.Parse(cols[1]);
                    }
                }
                else
                {
                    names[i] = "";
                    scores[i] = 0;
                }
            }

            return new HighScoreTable(names, scores);
        }



        static String webPost(string _URI, string _postString)
        {

            String REQUEST_METHOD_POST = "POST";
            String CONTENT_TYPE = "application/x-www-form-urlencoded";

            Stream dataStream = null;
            StreamReader reader = null;
            WebResponse response = null;
            String responseString = null;

            WebRequest request = WebRequest.Create(_URI);
            request.Method = REQUEST_METHOD_POST;
            String postData = _postString;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = CONTENT_TYPE;
            request.ContentLength = byteArray.Length;
            dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();
            reader = new StreamReader(dataStream);
            responseString = reader.ReadToEnd();

            if (reader != null) reader.Close();
            if (dataStream != null) dataStream.Close();
            if (response != null) response.Close();

            return responseString;
        }

        public static string sendScore(string name, int score)
        {
            string highscoreString = name + score;
            string postString = "?name=" + name + "&score=" + score;
            string response = null;
            response = webPost("http://zisforzombie.danfootitt.co.uk/newscore.php" + postString, postString);
            return response.Trim();
        }


    }
}
