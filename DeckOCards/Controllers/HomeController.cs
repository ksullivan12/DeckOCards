using DeckOCards.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DeckOCards.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory newHttpClientFactory)
        {
            _httpClientFactory = newHttpClientFactory;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult DisplayDeckofCards()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            //
            const string createDeckofCardsAPIUrl = "https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=1";
            var apiResponse = httpClient.GetFromJsonAsync<DeckofCards_Create>(createDeckofCardsAPIUrl).GetAwaiter().GetResult();
            //https://deckofcardsapi.com/api/deck/<<deck_id>>/draw/?count=2
            string deckId = apiResponse.deck_id;
            int numCardsToDraw = 5;
            string drawDeckofCardsAPIUrl = $"https://deckofcardsapi.com/api/deck/{deckId}/draw/?count={numCardsToDraw}";

            var drawCardsResponse = httpClient.GetFromJsonAsync<DeckofCards_Draw>(drawDeckofCardsAPIUrl).GetAwaiter().GetResult();
            var displayCardsModel = new DisplayResultsModel();
            displayCardsModel.createResult = apiResponse;
            displayCardsModel.drawResult = drawCardsResponse;
            return View(displayCardsModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
        public class DeckofCards_Create
        {
        public bool     success { get; set; }
        public string   deck_id { get; set; }
        public bool     shuffled { get; set; }
        public int      remaining { get; set; }

        }
        public class DeckofCards_Draw
        {
        public bool success { get; set; }
        public string deck_id { get; set; }
        public int remaining { get; set; }
        public DeckofCards_DrawCard[] card { get; set; }
        }
        public class DeckofCards_DrawCard
        {
        public string image { get; set; }
        public string value { get; set; }
        public string suit { get; set; }
        public string code { get; set; }
        }
        public class DisplayResultsModel
        {
        public DeckofCards_Create createResult { get; set; }
        public DeckofCards_Draw drawResult { get; set; }
        }
    
}