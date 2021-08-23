using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OSKI_Solutions_Test.Models;

namespace OSKI_Solutions_Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Repository _rep;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _rep = new Repository();
        }

        public IActionResult Aauthorization(string email, string password)
        {
            var UserId = _rep.GetUser(email, password);

            if (UserId != null)
            {
                _rep.SetCookies(HttpContext, "User", UserId.ToString());
                return RedirectToAction("Index");
            }
            else
                return View("Aauthorization");
        }

        public IActionResult Index()
        {
            if (HttpContext.Request.Cookies.ContainsKey("User"))
            {
                var UserId = Guid.Parse(_rep.GetCookie(HttpContext, "User"));

                var list = _rep.GetAllTests(UserId);
                return View(list);
            }
            else
            {
                return View("Aauthorization");
            }

        }

        [HttpPost]
        public IActionResult GetAnswers(bool Agree, Guid TestId)
        {
            var UserId = Guid.Parse(_rep.GetCookie(HttpContext, "User"));
            var answers = _rep.GetAnswers(TestId);
            var answerViewModel = new AnswerViewModel();
            answerViewModel.AswerList = answers;
            answerViewModel.TestId = TestId;
            answerViewModel.TestTitle = _rep.GetTest(TestId).Text;
            return View(answerViewModel);

        }

        [HttpPost]
        public IActionResult CheckTest(Guid AnswerId, Guid TestId)
        {
            var UserId = Guid.Parse(_rep.GetCookie(HttpContext, "User"));
            var IsCorrect = _rep.GetAnswer(AnswerId).IsCorrect;

            _rep.CheckTest(UserId, TestId, IsCorrect);
            var NotPassedTests = _rep.GetNotPassedTests(UserId);

            if (NotPassedTests.Count == 0)
            {
                var all = _rep.GetAllTests(UserId).Count;
                var correct = _rep.GetCorrectTestsCount(UserId);

                _rep.DeleteTestResults(UserId);
                return View(new ResultViewModel() { AllTestsCount = all.ToString(), CorrectTestsCount = correct.ToString() });

            }
            else
            {
                var answerViewModel = new AnswerViewModel();
                answerViewModel.AswerList = _rep.GetAnswers(NotPassedTests[0].Id);
                answerViewModel.TestId = NotPassedTests[0].Id;
                answerViewModel.TestTitle = _rep.GetTest(NotPassedTests[0].Id).Text;
                return View("GetAnswers", answerViewModel);
            }
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
