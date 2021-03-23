using AnimalCrossingNewHorizons.DAL;
using AnimalCrossingNewHorizons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnimalCrossingNewHorizons.Controllers
{
    public class ComController : BaseController
    {
        /// <summary>
        /// 商品一覧
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AjaxMethod(DTParameters param)
        {

            var comModelList = new List<ComModel>();
            using (var context = new AnimalEntities())
            {
                IQueryable<ComModel> ListData = context.T_COM.AsNoTracking()
                                .Where(x => x.del_flag == 0)
                                .Select(x =>
                                new ComModel
                                {
                                    Com_Id = x.com_id,
                                    ComName = x.com_name,
                                    ComDetail = x.com_detail,
                                }); // return type to be IQueryable

                //take and skip record according to pagination
                var takeData = ListData.OrderBy(q => q.Com_Id).Skip(param.Start).Take(param.Length).ToList();

                if (!string.IsNullOrEmpty(param.Search.Value))
                {
                    var sendData = takeData.Where(p => p.Com_Id.ToString().ToLower().Contains(param.Search.Value.ToLower())
                                   || p.ComName != null && p.ComName.ToLower().Contains(param.Search.Value.ToLower())
                                   || p.ComDetail != null && p.ComDetail.ToLower().Contains(param.Search.Value.ToLower())).ToList();

                    DTResult<ComModel> result = new DTResult<ComModel>
                    {
                        draw = param.Draw,
                        data = sendData.ToList(),
                        recordsFiltered = ListData.Count(),
                        recordsTotal = ListData.Count(),
                    };
                    return Json(result);
                }
                else
                {
                    var sendData = takeData;
                    DTResult<ComModel> result = new DTResult<ComModel>
                    {
                        draw = param.Draw,
                        data = sendData.ToList(),
                        recordsFiltered = ListData.Count(),
                        recordsTotal = ListData.Count(),
                    };
                    return Json(result);
                }
            }
        }

        /// <summary>
        /// 新規登録 表示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            var model = new ComModel();
            return View(model);
        }

        /// <summary>
        /// 新規登録
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ComModel model)
        {
            try
            {
                // 入力チェック
                if (!this.ModelState.IsValid)
                {
                    return View(model);
                }
                // 登録処理
                using (var context = new AnimalEntities())
                {
                    DateTime now = DateTime.Now;
                    string userID = User.Identity.Name;
                    T_COM targetData = new T_COM();
                    targetData.com_name = model.ComName;
                    targetData.com_detail = model.ComDetail;
                    targetData.create_date = now;
                    targetData.create_user = userID;
                    targetData.update_date = now;
                    targetData.update_user = userID;

                    context.T_COM.Add(targetData);
                    context.SaveChanges();
                }
                // 一覧画面にリダイレクト
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "問題が発生しました。");
            }
            return View(model);
        }

        /// <summary>
        /// 編集 表示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var model = new ComModel();

            if (id == null)
            {
                return RedirectToAction("Create");
            }

            T_COM comDetail = null;
            using (var context = new AnimalEntities())
            {
                comDetail = context.T_COM.AsNoTracking().Where(x => x.com_id == id).FirstOrDefault();
            }
            if (comDetail == null)
            {
                return HttpNotFound();
            }

            model.Com_Id = comDetail.com_id;
            model.ComName = comDetail.com_name;
            model.ComDetail = comDetail.com_detail;

            return View(model);
        }

        /// <summary>
        /// 編集
        /// </summary>
        /// <param name="comModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Edit")]
        [Button(ButtonName = "Save")]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ComModel model)
        {
            try
            {
                // 入力チェック
                if (!this.ModelState.IsValid)
                {
                    return View(model);
                }
                // 更新処理
                T_COM com = null;
                using (var context = new AnimalEntities())
                {
                    var now = DateTime.Now;
                    string userID = User.Identity.Name;

                    com = context.T_COM.Where(x => x.com_id == model.Com_Id).FirstOrDefault();
                    com.com_name = model.ComName;
                    com.com_detail = model.ComDetail;
                    com.update_date = now;
                    com.update_user = userID;

                    context.SaveChanges();
                }
                // 一覧画面にリダイレクト
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "問題が発生しました。");
            }
            return View(model);
        }

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Edit")]
        [Button(ButtonName = "Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ComModel model)
        {
            try
            {
                // 削除処理
                T_COM com = null;
                using (var context = new AnimalEntities())
                {
                    var now = DateTime.Now;
                    string userID = User.Identity.Name;

                    com = context.T_COM.Where(x => x.com_id == model.Com_Id).FirstOrDefault();
                    com.del_flag = 1;
                    com.update_date = now;
                    com.update_user = userID;

                    context.SaveChanges();
                }
                // 一覧画面にリダイレクト
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "問題が発生しました。");
            }
            return View(model);
        }
    }
}