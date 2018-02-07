using ComicsViewer.Web.Models.Comics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ComicsViewer.Web.Controllers
{
    public class ComicsController : Controller
    {
        private static readonly Comics _comics = new Comics
        {
            AllComics = Newtonsoft.Json.JsonConvert.DeserializeObject<Comics>(System.IO.File.ReadAllText(".\\Data\\comics.json"))
                .AllComics
                .Where(i => i.Count > 1)
                .OrderBy(i => i.Name)
                .Select(i => new Comic
                {
                    Issues = new Issues
                    {
                        Count = i.Issues.Count,
                        value = i.Issues.value.OrderBy(j => j.Issue).ToArray()
                    },
                    Count = i.Count,
                    Name = i.Name
                })
                .ToArray()
        };
        public static readonly HttpClient httpClient = new HttpClient();
        public ComicsController()
        {
            

        }
        public IActionResult Index()
        {
            return View(_comics);
        }
        [HttpGet("Comics/Comic/{comicName}")]
        public IActionResult Comic([FromRoute] string comicName)
        {
            var comic = _comics.AllComics.FirstOrDefault(i => i.Name == comicName);
            if (comic == null)
                return NotFound();
            return View(comic);
        }
        [HttpGet("Comics/Issue/{comicName}")] 
        [Produces("text/html")]
        public async Task<IActionResult> Issue([FromRoute] string comicName, [FromQuery] string i)
        {
            var comic = _comics.AllComics.FirstOrDefault(j => j.Name == comicName)?.Issues?.value?.FirstOrDefault(j => j.Issue == i);
            if (comic == null)
                return null;
            var res = await httpClient.GetStringAsync(comic.Href);
            var model = new IssueViewModel
            {
                Html = res,
                Href = comic.Href
            };
            return View(model);
        }
    }
}
