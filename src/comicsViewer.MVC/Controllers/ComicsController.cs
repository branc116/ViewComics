using ComicsViewer.Web.Models.Comics;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult IssueTest([FromRoute] string comicName, [FromQuery] string i)
        {
            var allIsues = new AllIssues
            {
                Links = new[]
                {
                    "https://4.bp.blogspot.com/-g6sZDpFnZT8/WnvJmxh7X9I/AAAAAAAC178/Fu_k1T0jf-oCownhje1kAYYs2BoYjXfWwCLcBGAs/s1600/084_000.jpg",
                    "https://3.bp.blogspot.com/-YkZFpgHJbS0/WnvJnm-zWUI/AAAAAAAC18A/kq14YjGE6dc0Im_yWEVlqb2lCNSA08GLACLcBGAs/s1600/084_001.jpg",
                    "https://1.bp.blogspot.com/-4HmDQqBNL6E/WnvJn284h3I/AAAAAAAC18E/0uSpPMEkv6sI-e0AFPNv-Wl8muogwbNswCLcBGAs/s1600/084_002.jpg",
                    "https://2.bp.blogspot.com/-UwnIzX9dxdU/WnvJnzsPTXI/AAAAAAAC18I/tEmcYHDRMgYhNuafw9nSWTbSu7MYvpWDQCLcBGAs/s1600/084_003.jpg",
                    "https://4.bp.blogspot.com/-3uWtdKlApjM/WnvJoGYEJ8I/AAAAAAAC18M/_vdireRVPEASbUlneQPVi0CntAWS8k3xQCLcBGAs/s1600/084_004.jpg",
                    "https://2.bp.blogspot.com/-rtRdMdVARH0/WnvJofXxk1I/AAAAAAAC18Q/X8AvCRJf1qQQInAzz4f-QUtWn9mdoVSzQCLcBGAs/s1600/084_005.jpg",
                    "https://3.bp.blogspot.com/-pyuWQrGXWEI/WnvJokCxv_I/AAAAAAAC18U/0y4kc25zTX089kgFqZkVsjrvAhCXXOaswCLcBGAs/s1600/084_006.jpg",
                    "https://4.bp.blogspot.com/-l20fZDzhyiY/WnvJozj2PCI/AAAAAAAC18Y/IXvKYU3PgRoXMRWx4tw6DeCoStVh4D3uwCLcBGAs/s1600/084_007.jpg",
                    "https://4.bp.blogspot.com/-tWMnuw62zTc/WnvJpN7SxJI/AAAAAAAC18c/0yxSP_UErnsXWDd2kwrGhEdv9gZHwY6NwCLcBGAs/s1600/084_008.jpg",
                    "https://3.bp.blogspot.com/-L3LtoTEWf7I/WnvJpAzPDEI/AAAAAAAC18g/cyDPHV1YerUlZ3CYKDYv25Hud8lDJNVEACLcBGAs/s1600/084_009.jpg",
                    "https://2.bp.blogspot.com/-B9LiHBVSQbs/WnvJpi3FPMI/AAAAAAAC18k/zGrAPv5AutoTkZe-YZDcxvdlXqv2cpnXwCLcBGAs/s1600/084_010.jpg",
                    "https://2.bp.blogspot.com/-0jwTFa4wRPg/WnvJp1HafgI/AAAAAAAC18o/1dSyiN8LZng90RnfGOdyrj8pLwbxWOVIACLcBGAs/s1600/084_011.jpg",
                    "https://4.bp.blogspot.com/-gmV76t69WHk/WnvJp9nCG3I/AAAAAAAC18s/ngnGk7uFtRQX14PYV_Ia4HIYNhAS7QNPwCLcBGAs/s1600/084_012.jpg",
                    "https://2.bp.blogspot.com/-wwjmx-nlhY0/WnvJqAq1CAI/AAAAAAAC18w/Kl6cH3_AszsmALxfSwGEXx0JXVH-H9vBwCLcBGAs/s1600/084_013.jpg",
                    "https://4.bp.blogspot.com/-1z36Op450Xo/WnvJqciEPFI/AAAAAAAC180/firqMEGCK78PuajImvXOguTH2MeQr5gQQCLcBGAs/s1600/084_014.jpg",
                    "https://1.bp.blogspot.com/-p1PgyscrrmI/WnvJqghUmoI/AAAAAAAC184/Nozkck74uIIJI0G0Ep80sGQHWtBmfRoSACLcBGAs/s1600/084_015.jpg",
                    "https://3.bp.blogspot.com/-NVdV-xhVdkk/WnvJqycqcZI/AAAAAAAC188/_Zw7cQwnU4Q_TnWHf2YZ4cKgRnVV_sebwCLcBGAs/s1600/084_016.jpg",
                    "https://4.bp.blogspot.com/-0d6v3Iu3zmo/WnvJrM30ozI/AAAAAAAC19A/VkqL9oulM_Yp0Oovt4woOp1f6ADv8pXiQCLcBGAs/s1600/084_017.jpg",
                    "https://4.bp.blogspot.com/-FpEsGov9yfc/WnvJrMTy_yI/AAAAAAAC19E/ZbChedXwycszWwYd1tw1nLxkb1K-cjHLgCLcBGAs/s1600/084_018.jpg",
                    "https://4.bp.blogspot.com/-dyV63pNRYP4/WnvJrao7NrI/AAAAAAAC19I/7-QY_2gaAnEJgrEOrOjEhq5BHqVH4MQRACLcBGAs/s1600/084_019.jpg",
                    "https://2.bp.blogspot.com/-EvP7K4qUu3k/WnvJrnwsAyI/AAAAAAAC19M/GeM7XkaBnVMqwCfV6M3EGKRACyY5TBjOQCLcBGAs/s1600/084_020.jpg",
                    "https://2.bp.blogspot.com/-q071t_SaTl0/WnvJr0juXkI/AAAAAAAC19Q/8BCPvzxa2s8XzkKMVA_FNNg2lQ_DfwvrgCLcBGAs/s1600/084_021.jpg"
                }
            };
            return View(allIsues);
        }
    }
}
