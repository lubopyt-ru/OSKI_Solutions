using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OSKI_Solutions_Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSKI_Solutions_Test
{
    public class Repository
    {

        OSKI_DBContext ctx = new OSKI_DBContext();
        public Guid GetUser(string email, string password)
        {
            return ctx.User.FirstOrDefault(u => u.Email == email & u.Password == password).Id;
        }


        public Test GetTest(Guid TestId)
        {
            return ctx.Test.FirstOrDefault(t => t.Id == TestId);
        }

        public List<Test> GetAllTests(Guid UserId)
        {
            var list = new List<Test>();

            var user = ctx.User.Include(u => u.Test_User_Junction).FirstOrDefault(u => u.Id == UserId);

            foreach (Test_User_Junction tu in user.Test_User_Junction)
            {
                var test = ctx.Test.Where(t => t.Id == tu.TestId).ToList();

                list.AddRange(test);
            }
            return list;
        }


        public List<Answers> GetAnswers(Guid TestId)
        {
            return ctx.Answers.Where(a => a.TestId == TestId).ToList();

        }

        public Answers GetAnswer(Guid AnswerId)
        {
            return ctx.Answers.FirstOrDefault(a => a.Id == AnswerId);

        }

        public void SetCookies(HttpContext ctx, string key, string value)
        {

            //ctx.Response.Cookies.Delete(key);
            ctx.Response.Cookies.Append(key, value);


        }
        public string GetCookie(HttpContext ctx, string key)
        {
            if (ctx.Request.Cookies.ContainsKey(key))
                return ctx.Request.Cookies[key];
            else
                return "";
        }

        public void CheckTest(Guid UserId, Guid TestId, bool IsCorrect)
        {
            var query = ctx.Test_User_Junction.FirstOrDefault(u => u.UserId == UserId & u.TestId == TestId);
            query.IsCorrect = IsCorrect;
            query.IsPassed = true;
            ctx.SaveChanges();
        }

        public int GetCorrectTestsCount(Guid UserId)
        {
            return ctx.Test_User_Junction.Where(tu => tu.UserId == UserId & tu.IsCorrect == true).ToList().Count;
        }

        public List<Guid> GetPassedTests(Guid UserId)
        {
            return ctx.Test_User_Junction.Where(tu => tu.UserId == UserId & tu.IsPassed == true).Select(tu => tu.TestId).ToList();

        }
        public List<Guid> GetNotPassedTestsId(Guid UserId)
        {
            return ctx.Test_User_Junction.Where(tu => tu.UserId == UserId & tu.IsPassed == false).Select(tu => tu.TestId).ToList();

        }
        public List<Test> GetNotPassedTests(Guid UserId)
        {
            var list = new List<Test>();

            var user = ctx.User.Include(u => u.Test_User_Junction).FirstOrDefault(u => u.Id == UserId);

            foreach (Test_User_Junction tu in user.Test_User_Junction)
            {
                var test = ctx.Test.Where(t => t.Id == tu.TestId & tu.IsPassed == false).ToList();

                list.AddRange(test);
            }
            return list;
        }

        public void DeleteTestResults(Guid UserId)
        {
            var ut = ctx.Test_User_Junction.Where(ut => ut.UserId == UserId);
            foreach (Test_User_Junction u in ut)
            {
                u.IsCorrect = false;
                u.IsPassed = false;
            }
            ctx.SaveChanges();
        }
    }
}
