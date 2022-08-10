using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeTest.Project.Website.Controllers
{
    public class WordCountController : Controller
    {
        // GET: WordCount
        public ActionResult GetWordCountReport(string textAreaContent, string count)
        {
            Dictionary<string, int> model = new Dictionary<string, int>();
            if (!string.IsNullOrEmpty(textAreaContent))
            {
                char[] chars = { ' ', '.', ',', ';', ':', '?', '\n', '\r' };
                string[] words = textAreaContent.Split(chars);
                int minWordLength = 2;// to count words having more than 2 characters

                // iterate over the word collection to count occurrences
                foreach (string word in words)
                {
                    string w = word.Trim().ToLower();
                    if (w.Length > minWordLength)
                    {
                        if (!model.ContainsKey(w))
                        {
                            // add new word to collection
                            model.Add(w, 1);
                        }
                        else
                        {
                            // update word occurrence count
                            model[w] += 1;
                        }
                    }
                }
            }
            int resultCount = !string.IsNullOrEmpty(count) ? Convert.ToInt32(count) : model.Count;
            // order the collection by word count
            model = model.Count > 0 ? model.OrderByDescending(x => x.Value).Take(resultCount).ToDictionary(x => x.Key, x => x.Value) : null;
            return View("_AjaxWordCount", model);
        }
    }
}