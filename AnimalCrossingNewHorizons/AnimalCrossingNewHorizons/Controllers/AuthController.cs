using AnimalCrossingNewHorizons.DAL;
using AnimalCrossingNewHorizons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AnimalCrossingNewHorizons.Controllers
{
    public class AuthController : Controller
    {
        /// <summary>
        /// ログイン 表示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// ログイン処理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(AuthModel model)
        {
            bool hasUser = false;
            using (var context = new AnimalEntities())
            {
                hasUser = context.M_USER.AsNoTracking().Where(x => x.user_id == model.Id &&
                                                            x.password == model.Password &&
                                                            x.del_flag == 0).Any();
            }
            //if (model.Id == "test" && model.Password == "passwd")
            if (hasUser)
            {
                // ユーザー認証 成功
                FormsAuthentication.SetAuthCookie(model.Id, true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // ユーザー認証 失敗
                this.ModelState.AddModelError(string.Empty, "指定されたユーザー名またはパスワードが正しくありません。");
                return this.View(model);
            }
        }

        /// <summary>
        /// ログアウト処理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Auth", "Index");
        }
    }
}