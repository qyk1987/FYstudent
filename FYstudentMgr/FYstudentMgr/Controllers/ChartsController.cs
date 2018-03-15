using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using FYstudentMgr.Models;
using FYstudentMgr.ViewModels;
using System.Globalization;
using System.Web.Http.Cors;
using FYstudentMgr.Manager;

namespace FYstudentMgr.Controllers
{
    [Authorize]
    [RoutePrefix("api/charts")]
    //[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class ChartsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private PostManager postmanager = new PostManager();
        //GET: api/Charts
        /// <summary>
        /// 获取时间段内个产品按时间单位分组的业绩数据  用于堆积折线图
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="timeSpan"></param>
        /// <param name="postUserId"></param>
        /// <returns></returns>
        [Route("GetStackBySeller")]
        public LineChartVM GetChartByDate(string startDate, string endDate, int timeSpan, int postUserId)
        {

            DateTime sdate = Convert.ToDateTime(startDate.Replace("\"", "")).Date;
            DateTime edate = Convert.ToDateTime(endDate.Replace("\"", ""));
            
            var orders = db.OrderDetails.Where(o => o.Order.PostUserId == postUserId)
                            .Where(o=> o.Order.State!=OrderState.已删除)
                            .Where(o => o.Order.OrderDate >= sdate)
                            .Where(o => o.Order.OrderDate <= edate);
            var products = orders.GroupBy(o => o.Product.ProductName)
                            .Select(g => g.Key).ToList();
            var list = new List<LineDataPoint>();
            switch (timeSpan)
            {
                case 0:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year)
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Product.ProductName)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.ActualPay )
                                            }).ToList(),
                                label = g.Key + "年"
                            }).ToList();
                        break;

                    }
                case 1:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year + "年" + o.Order.OrderDate.Month + "月")
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Product.ProductName)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key 
                            }).ToList();
                        break;

                    }
                case 2:
                    {
                        var neworders = orders.Select(o => new WeekData()
                                {
                                    year=o.Order.OrderDate.Year,
                                    date=o.Order.OrderDate,
                                    pay=o.ActualPay,
                                    name=o.Product.ProductName                                 
                                }).ToList();
                        foreach(var l in neworders)
                        {
                            l.week =GetWeekOfYear(l.date);
                        }
                        list = neworders
                            .GroupBy(o => o.year+"年"+o.week+"th周")
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.name)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.pay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }
                case 3:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year + "/" + o.Order.OrderDate.Month + "/" + o.Order.OrderDate.Day)
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Product.ProductName)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }

            }
            List<LineChartData> datas = new List<LineChartData>();
            foreach(var product in products)
            {
                List<double> ds = new List<double>();
                list.ForEach(l =>
                {
                    var pay = l.datas.Where(d => d.name == product).SingleOrDefault();
                    if (pay == null)
                        ds.Add(0);
                    else
                        ds.Add(pay.data);
                });
                datas.Add(new LineChartData()
                {
                    data = ds,
                    name = product
                });
            }
            
            return new LineChartVM()
            {
                chartDatas = datas,
                chartLabels = list.Select(l => l.label).ToList()

            };
        }
        /// <summary>
        /// 获取时间段内个产品业绩数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="postUserId"></param>
        /// <returns></returns>
        [Route("GetPieBySeller")]
        public IList<PieData> GetPieChartByDate(string startDate, string endDate,  int postUserId,string flag)
        {

            DateTime sdate = Convert.ToDateTime(startDate.Replace("\"", "")).Date;
            DateTime edate = Convert.ToDateTime(endDate.Replace("\"", ""));

            var orders = db.OrderDetails.Where(o => o.Order.PostUserId == postUserId)
                             .Where(o => o.Order.State != OrderState.已删除)
                            .Where(o => o.Order.OrderDate >= sdate)
                            .Where(o => o.Order.OrderDate <= edate);
            List<PieData> datas = new List<PieData>();
            switch (flag)
            {
                case "product":
                    {
                        datas = orders.GroupBy(o => o.Product.ProductName)
                            .Select(g => new PieData()
                            {
                                name = g.Key,
                                value = g.Sum(d => d.ActualPay)
                            }).ToList();
                        break;
                    }
                case "category":
                    {
                        datas = orders.GroupBy(o => o.Product.Subject.Category.CategoryName)
                           .Select(g => new PieData()
                           {
                               name = g.Key,
                               value = g.Sum(d => d.ActualPay)
                           }).ToList();
                        break;
                    }
                case "channel":
                    {
                        datas = orders.GroupBy(o => o.Order.Channel.ToString())
                           .Select(g => new PieData()
                           {
                               name = g.Key,
                               value = g.Sum(d => d.ActualPay)
                           }).ToList();
                        break;
                    }
            }                 

            return datas;
        }
        /// <summary>
        /// 根据管理员大区返回该大区下辖所有校区的业绩走势
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="timeSpan"></param>
        /// <param name="postUserId"></param>
        /// <returns></returns>
        [Route("GetCampusStackByManager")]
        public async Task<LineChartVM>  GetCampusStackByManager(string startDate, string endDate, int timeSpan, int postId)
        {

            DateTime sdate = Convert.ToDateTime(startDate.Replace("\"", "")).Date;
            DateTime edate = Convert.ToDateTime(endDate.Replace("\"", ""));
            //var pu = await db.PostUsers.FindAsync(postUserId);
            var post = await db.Posts.FindAsync(postId);
            var orders = db.OrderDetails.Include(o=>o.Order.postUser.Post.Campus).Where(o => o.Order.postUser.Post.DistrictId == post.DistrictId)
                            .Where(o => o.Order.State != OrderState.已删除)
                            .Where(o => o.Order.OrderDate >= sdate)
                            .Where(o => o.Order.OrderDate <= edate);
            var campusids = orders.GroupBy(o => o.Order.postUser.Post.Campus.CampusName)
                            .Select(g => g.Key).ToList();
            var list = new List<LineDataPoint>();
            switch (timeSpan)
            {
                case 0:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year)
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Order.postUser.Post.Campus.CampusName)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key + "年"
                            }).ToList();
                        break;

                    }
                case 1:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year + "年" + o.Order.OrderDate.Month + "月")
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Order.postUser.Post.Campus.CampusName)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key.ToString(),
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }
                case 2:
                    {
                        var neworders = orders.Select(o => new WeekData()
                        {
                            year = o.Order.OrderDate.Year,
                            date = o.Order.OrderDate,
                            pay = o.ActualPay,
                            name = o.Order.postUser.Post.Campus.CampusName
                        }).ToList();
                        foreach (var l in neworders)
                        {
                            l.week = GetWeekOfYear(l.date);
                        }
                        list = neworders
                            .GroupBy(o => o.year + "年" + o.week + "th周")
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.name)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.pay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }
                case 3:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year + "/" + o.Order.OrderDate.Month + "/" + o.Order.OrderDate.Day)
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Order.postUser.Post.Campus.CampusName)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }

            }
            List<LineChartData> datas = new List<LineChartData>();
            foreach (var campus in campusids)
            {
                List<double> ds = new List<double>();
                list.ForEach(l =>
                {
                    var pay = l.datas.Where(d => d.name == campus).SingleOrDefault();
                    if (pay == null)
                        ds.Add(0);
                    else
                        ds.Add(pay.data);
                });
                datas.Add(new LineChartData()
                {
                    data = ds,
                    name = campus
                });
            }

            return new LineChartVM()
            {
                chartDatas = datas,
                chartLabels = list.Select(l => l.label).ToList()

            };
        }

        /// <summary>
        /// 根据大区管理员岗位返回各校区下各服务点的业绩
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="timeSpan"></param>
        /// <param name="postUserId"></param>
        /// <returns></returns>
        [Route("GetSpotStackByManager")]
        public async Task<List<LineChartVMObj>> GetSpotStackByManager(string startDate, string endDate, int timeSpan, int postId)
        {
            DateTime sdate = Convert.ToDateTime(startDate.Replace("\"", "")).Date;
            DateTime edate = Convert.ToDateTime(endDate.Replace("\"", ""));
            //var pu = await db.PostUsers.FindAsync(postUserId);
            var post = await db.Posts.FindAsync(postId);
            List<LineChartVMObj> list = new List<LineChartVMObj>();

            var orders = db.OrderDetails.Include(o => o.Order.postUser.Post.Campus).Where(o => o.Order.postUser.Post.DistrictId == post.DistrictId)
                            .Where(o => o.Order.State != OrderState.已删除)
                            .Where(o => o.Order.OrderDate >= sdate)
                            .Where(o => o.Order.OrderDate <= edate);
            var campusids = orders.GroupBy(o => o.Order.postUser.Post.Campus.Id)
                            .Select(g => new{
                                 id=g.Key,
                                 title=g.FirstOrDefault().Order.postUser.Post.Campus.CampusName
                            }).ToList();
           foreach(var campus in campusids)
            {
                var item = new LineChartVMObj();
                item.data =await  GetSpotStackByCampus(sdate, edate, timeSpan, campus.id);
                item.title = campus.title;
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 根据校区id返回该校区下所有服务点的业绩
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="timeSpan"></param>
        /// <param name="campusid"></param>
        /// <returns></returns>
        [Route("GetSpotStackByMaster")]
        public async Task<LineChartVM> GetSpotStackByMaster(string startDate, string endDate, int timeSpan, int campusid)
        {

            DateTime sdate = Convert.ToDateTime(startDate.Replace("\"", "")).Date;
            DateTime edate = Convert.ToDateTime(endDate.Replace("\"", ""));

            var orders = db.OrderDetails.Include(o => o.Order.postUser.Post.Spot).Where(o => o.Order.postUser.Post.CampusId == campusid)
                            .Where(o => o.Order.State != OrderState.已删除)
                            .Where(o => o.Order.OrderDate >= sdate)
                            .Where(o => o.Order.OrderDate <= edate);
            var spots = orders.GroupBy(o => o.Order.postUser.Post.Spot.SpotName)
                            .Select(g => g.Key).ToList();
            var list = new List<LineDataPoint>();
            switch (timeSpan)
            {
                case 0:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year)
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Order.postUser.Post.Spot.SpotName)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key + "年"
                            }).ToList();
                        break;

                    }
                case 1:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year + "年" + o.Order.OrderDate.Month + "月")
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Order.postUser.Post.Spot.SpotName)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key.ToString(),
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }
                case 2:
                    {
                        var neworders = orders.Select(o => new WeekData()
                        {
                            year = o.Order.OrderDate.Year,
                            date = o.Order.OrderDate,
                            pay = o.ActualPay,
                            name = o.Order.postUser.Post.Spot.SpotName
                        }).ToList();
                        foreach (var l in neworders)
                        {
                            l.week = GetWeekOfYear(l.date);
                        }
                        list = neworders
                            .GroupBy(o => o.year + "年" + o.week + "th周")
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.name)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.pay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }
                case 3:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year + "/" + o.Order.OrderDate.Month + "/" + o.Order.OrderDate.Day)
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Order.postUser.Post.Spot.SpotName)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }

            }
            List<LineChartData> datas = new List<LineChartData>();
            foreach (var spot in spots)
            {
                List<double> ds = new List<double>();
                list.ForEach(l =>
                {
                    var pay = l.datas.Where(d => d.name == spot).SingleOrDefault();
                    if (pay == null)
                        ds.Add(0);
                    else
                        ds.Add(pay.data);
                });
                datas.Add(new LineChartData()
                {
                    data = ds,
                    name = spot
                });
            }

            return new LineChartVM()
            {
                chartDatas = datas,
                chartLabels = list.Select(l => l.label).ToList()

            };
        }


        /// <summary>
        /// 根据服务点id返回该服务点业级
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="timeSpan"></param>
        /// <param name="spotid"></param>
        /// <returns></returns>
        [Route("GetSpotBySpot")]
        public async Task<List<BasicLinePoint>> GetSpotBySpot(string startDate, string endDate, int timeSpan, int spotid)
        {

            DateTime sdate = Convert.ToDateTime(startDate.Replace("\"", "")).Date;
            DateTime edate = Convert.ToDateTime(endDate.Replace("\"", ""));

            var orders = db.OrderDetails.Include(o => o.Order.postUser.Post.Spot).Where(o => o.Order.postUser.Post.SpotId == spotid)
                            .Where(o => o.Order.State != OrderState.已删除)
                            .Where(o => o.Order.OrderDate >= sdate)
                            .Where(o => o.Order.OrderDate <= edate);
            
            var list = new List<BasicLinePoint>();
            switch (timeSpan)
            {
                case 0:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year)
                            .Select(g => new BasicLinePoint()
                            {
                                
                                data = g.Sum(d => d.ActualPay),                               
                                label = g.Key + "年"
                            }).ToList();
                        break;

                    }
                case 1:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year + "年" + o.Order.OrderDate.Month + "月")
                            .Select(g => new BasicLinePoint()
                            {
                                data = g.Sum(d => d.ActualPay),
                                label = g.Key
                            }).ToList();
                        break;

                    }
                case 2:
                    {
                        var neworders = orders.Select(o => new WeekData()
                        {
                            year = o.Order.OrderDate.Year,
                            date = o.Order.OrderDate,
                            pay = o.ActualPay
                            
                        }).ToList();
                        foreach (var l in neworders)
                        {
                            l.week = GetWeekOfYear(l.date);
                        }
                        list = neworders
                            .GroupBy(o => o.year + "年" + o.week + "th周")
                            .Select(g => new BasicLinePoint()
                            {
                                data = g.Sum(d => d.pay),
                                label = g.Key
                            }).ToList();
                        break;

                    }
                case 3:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year + "/" + o.Order.OrderDate.Month + "/" + o.Order.OrderDate.Day)
                            .Select(g => new BasicLinePoint()
                            {
                                data = g.Sum(d => d.ActualPay),
                                label = g.Key
                            }).ToList();
                        break;

                    }

            }
            return list;
        }




        /// <summary>
        /// 根据岗位返回对应单位各大类业绩
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="timeSpan"></param>
        /// <param name="postUserId"></param>
        /// <returns></returns>
        [Route("GetCategoryStackByManager")]
        public async Task<LineChartVM> GetCategoryStackByManager(string startDate, string endDate, int timeSpan, int postId)
        {

            DateTime sdate = Convert.ToDateTime(startDate.Replace("\"", "")).Date;
            DateTime edate = Convert.ToDateTime(endDate.Replace("\"", ""));
            //var pu = await db.PostUsers.FindAsync(postUserId);
            var post = await db.Posts.FindAsync(postId);
            var orders = db.OrderDetails.Include(o => o.Product.Subject.Category).Where(o => o.Order.State != OrderState.已删除)
                            .Where(o => o.Order.OrderDate >= sdate)
                            .Where(o => o.Order.OrderDate <= edate);
            if (post.SpotId != null)
            {
                orders = orders.Where(o => o.Order.postUser.Post.SpotId == post.SpotId);
            }else if (post.CampusId != null)
            {
                orders = orders.Where(o => o.Order.postUser.Post.CampusId == post.CampusId);
            }else if(post.DistrictId != null)
            {
                orders = orders.Where(o => o.Order.postUser.Post.DistrictId == post.DistrictId);
            }                 
            var categoyrs = orders.GroupBy(o => o.Product.Subject.Category.CategoryName)
                            .Select(g => g.Key).ToList();
            var list = new List<LineDataPoint>();
            switch (timeSpan)
            {
                case 0:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year)
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Product.Subject.Category.CategoryName)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key + "年"
                            }).ToList();
                        break;

                    }
                case 1:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year + "年" + o.Order.OrderDate.Month + "月")
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Product.Subject.Category.CategoryName)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key.ToString(),
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }
                case 2:
                    {
                        var neworders = orders.Select(o => new WeekData()
                        {
                            year = o.Order.OrderDate.Year,
                            date = o.Order.OrderDate,
                            pay = o.ActualPay,
                            name = o.Product.Subject.Category.CategoryName
                        }).ToList();
                        foreach (var l in neworders)
                        {
                            l.week = GetWeekOfYear(l.date);
                        }
                        list = neworders
                            .GroupBy(o => o.year + "年" + o.week + "th周")
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.name)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.pay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }
                case 3:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year + "/" + o.Order.OrderDate.Month + "/" + o.Order.OrderDate.Day)
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Product.Subject.Category.CategoryName)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }

            }
            List<LineChartData> datas = new List<LineChartData>();
            foreach (var campus in categoyrs)
            {
                List<double> ds = new List<double>();
                list.ForEach(l =>
                {
                    var pay = l.datas.Where(d => d.name == campus).SingleOrDefault();
                    if (pay == null)
                        ds.Add(0);
                    else
                        ds.Add(pay.data);
                });
                datas.Add(new LineChartData()
                {
                    data = ds,
                    name = campus
                });
            }

            return new LineChartVM()
            {
                chartDatas = datas,
                chartLabels = list.Select(l => l.label).ToList()

            };
        }


        [Route("GetsubjectStackByManager")]
        public async Task<List<LineChartVMObj>> GetsubjectStackByManager(string startDate, string endDate, int timeSpan, int postId)
        {
            DateTime sdate = Convert.ToDateTime(startDate.Replace("\"", "")).Date;
            DateTime edate = Convert.ToDateTime(endDate.Replace("\"", ""));
            //var pu = await db.PostUsers.FindAsync(postUserId);
            var post = await db.Posts.FindAsync(postId);
            List<LineChartVMObj> list = new List<LineChartVMObj>();

            var orders = db.OrderDetails.Include(o => o.Product.Subject.Category).Where(o => o.Order.State != OrderState.已删除)
                           .Where(o => o.Order.OrderDate >= sdate)
                           .Where(o => o.Order.OrderDate <= edate);
            if (post.SpotId != null)
            {
                orders = orders.Where(o => o.Order.postUser.Post.SpotId == post.SpotId);
            }
            else if (post.CampusId != null)
            {
                orders = orders.Where(o => o.Order.postUser.Post.CampusId == post.CampusId);
            }
            else if (post.DistrictId != null)
            {
                orders = orders.Where(o => o.Order.postUser.Post.DistrictId == post.DistrictId);
            }

            var categories = orders.GroupBy(o => o.Product.Subject.CategoryId)
                            .Select(g => new {
                                id = g.Key,
                                title = g.FirstOrDefault().Product.Subject.Category.CategoryName
                            }).ToList();
            foreach (var cat in categories)
            {
                var item = new LineChartVMObj();
                item.data = await GetSubjectStackByCategory(sdate, edate, timeSpan, cat.id);
                item.title = cat.title;
                list.Add(item);
            }
            return list;
        }


        /// <summary>
        /// 根据岗位id获取所属业务员的业绩分布情况
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="timeSpan"></param>
        /// <param name="postUserId"></param>
        /// <returns></returns>
        [Route("GetSellerStackByManager")]
        public async Task<LineChartVM> GetSellerStackByManager(string startDate, string endDate, int timeSpan, int postId)
        {

            DateTime sdate = Convert.ToDateTime(startDate.Replace("\"", "")).Date;
            DateTime edate = Convert.ToDateTime(endDate.Replace("\"", ""));
            //var pu = await db.PostUsers.FindAsync(postUserId);
            var postids = postmanager.GetSons(postId).Select(p=>p.Id).ToList();
            var orders = db.OrderDetails.Include(od=>od.Order.postUser.User).Where(o => o.Order.State != OrderState.已删除)
                            .Where(o => o.Order.OrderDate >= sdate)
                            .Where(o => o.Order.OrderDate <= edate)
                            .Where(o=>postids.Contains(o.Order.postUser.PostId));
           
            var users = orders.GroupBy(o => o.Order.postUser.User.Name)
                            .Select(g => g.Key).ToList();
            var list = new List<LineDataPoint>();
            switch (timeSpan)
            {
                case 0:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year)
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Order.postUser.User.Name)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key + "年"
                            }).ToList();
                        break;

                    }
                case 1:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year + "年" + o.Order.OrderDate.Month + "月")
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Order.postUser.User.Name)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key.ToString(),
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }
                case 2:
                    {
                        var neworders = orders.Select(o => new WeekData()
                        {
                            year = o.Order.OrderDate.Year,
                            date = o.Order.OrderDate,
                            pay = o.ActualPay,
                            name = o.Order.postUser.User.Name
                        }).ToList();
                        foreach (var l in neworders)
                        {
                            l.week = GetWeekOfYear(l.date);
                        }
                        list = neworders
                            .GroupBy(o => o.year + "年" + o.week + "th周")
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.name)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.pay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }
                case 3:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year + "/" + o.Order.OrderDate.Month + "/" + o.Order.OrderDate.Day)
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Order.postUser.User.Name)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }

            }
            List<LineChartData> datas = new List<LineChartData>();
            foreach (var campus in users)
            {
                List<double> ds = new List<double>();
                list.ForEach(l =>
                {
                    var pay = l.datas.Where(d => d.name == campus).SingleOrDefault();
                    if (pay == null)
                        ds.Add(0);
                    else
                        ds.Add(pay.data);
                });
                datas.Add(new LineChartData()
                {
                    data = ds,
                    name = campus
                });
            }

            return new LineChartVM()
            {
                chartDatas = datas,
                chartLabels = list.Select(l => l.label).ToList()

            };
        }

        // GET: api/Charts/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrder(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Charts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Charts
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orders.Add(order);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = order.Id }, order);
        }

        // DELETE: api/Charts/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> DeleteOrder(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            await db.SaveChangesAsync();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                postmanager.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }

        #region 工具
        /// <summary>
        /// 获取指定日期是一年中的第几周
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private  int GetWeekOfYear(DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return weekOfYear;
        }
        /// <summary>
        /// 根据校区编号返回该小区下辖所有服务点的业绩走势
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="timeSpan"></param>
        /// <param name="campusId"></param>
        /// <returns></returns>
        private async Task<LineChartVM> GetSpotStackByCampus(DateTime sdate, DateTime edate, int timeSpan, int campusId)
        {
            var orders = db.OrderDetails.Include(o => o.Order.postUser.Post.Spot).Where(o => o.Order.postUser.Post.CampusId == campusId)
                            .Where(o => o.Order.State != OrderState.已删除)
                            .Where(o => o.Order.OrderDate >= sdate)
                            .Where(o => o.Order.OrderDate <= edate);
            var spots = orders.GroupBy(o => o.Order.postUser.Post.Spot.SpotName)
                            .Select(g => g.Key).ToList();
            var list = new List<LineDataPoint>();
            switch (timeSpan)
            {
                case 0:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year)
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Order.postUser.Post.Spot.SpotName)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key + "年"
                            }).ToList();
                        break;

                    }
                case 1:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year + "年" + o.Order.OrderDate.Month + "月")
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Order.postUser.Post.Spot.SpotName)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key.ToString(),
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }
                case 2:
                    {
                        var neworders = orders.Select(o => new WeekData()
                        {
                            year = o.Order.OrderDate.Year,
                            date = o.Order.OrderDate,
                            pay = o.ActualPay,
                            name = o.Order.postUser.Post.Spot.SpotName
                        }).ToList();
                        foreach (var l in neworders)
                        {
                            l.week = GetWeekOfYear(l.date);
                        }
                        list = neworders
                            .GroupBy(o => o.year + "年" + o.week + "th周")
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.name)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.pay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }
                case 3:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year + "/" + o.Order.OrderDate.Month + "/" + o.Order.OrderDate.Day)
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Order.postUser.Post.Spot.SpotName)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }

            }
            List<LineChartData> datas = new List<LineChartData>();
            foreach (var spot in spots)
            {
                List<double> ds = new List<double>();
                list.ForEach(l =>
                {
                    var pay = l.datas.Where(d => d.name == spot).SingleOrDefault();
                    if (pay == null)
                        ds.Add(0);
                    else
                        ds.Add(pay.data);
                });
                datas.Add(new LineChartData()
                {
                    data = ds,
                    name = spot
                });
            }

            return new LineChartVM()
            {
                chartDatas = datas,
                chartLabels = list.Select(l => l.label).ToList()

            };
        }

        /// <summary>
        /// 根据大类id返回该类下所有科目业绩
        /// </summary>
        /// <param name="sdate"></param>
        /// <param name="edate"></param>
        /// <param name="timeSpan"></param>
        /// <param name="categoryid"></param>
        /// <returns></returns>
        private async Task<LineChartVM> GetSubjectStackByCategory(DateTime sdate, DateTime edate, int timeSpan, int categoryid)
        {
            var orders = db.OrderDetails.Include(o => o.Product.Subject).Where(o => o.Product.Subject.CategoryId== categoryid)
                            .Where(o => o.Order.State != OrderState.已删除)
                            .Where(o => o.Order.OrderDate >= sdate)
                            .Where(o => o.Order.OrderDate <= edate);
            var subjects = orders.GroupBy(o => o.Product.Subject.Name)
                            .Select(g => g.Key).ToList();
            var list = new List<LineDataPoint>();
            switch (timeSpan)
            {
                case 0:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year)
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Product.Subject.Name)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key + "年"
                            }).ToList();
                        break;

                    }
                case 1:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year + "年" + o.Order.OrderDate.Month + "月")
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Product.Subject.Name)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key.ToString(),
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }
                case 2:
                    {
                        var neworders = orders.Select(o => new WeekData()
                        {
                            year = o.Order.OrderDate.Year,
                            date = o.Order.OrderDate,
                            pay = o.ActualPay,
                            name = o.Product.Subject.Name
                        }).ToList();
                        foreach (var l in neworders)
                        {
                            l.week = GetWeekOfYear(l.date);
                        }
                        list = neworders
                            .GroupBy(o => o.year + "年" + o.week + "th周")
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.name)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.pay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }
                case 3:
                    {
                        list = orders
                            .GroupBy(o => o.Order.OrderDate.Year + "/" + o.Order.OrderDate.Month + "/" + o.Order.OrderDate.Day)
                            .Select(g => new LineDataPoint()
                            {
                                datas = g.GroupBy(or => or.Product.Subject.Name)
                                            .Select(p => new LineData()
                                            {
                                                name = p.Key,
                                                data = p.Sum(d => d.ActualPay)
                                            }).ToList(),
                                label = g.Key
                            }).ToList();
                        break;

                    }

            }
            List<LineChartData> datas = new List<LineChartData>();
            foreach (var spot in subjects)
            {
                List<double> ds = new List<double>();
                list.ForEach(l =>
                {
                    var pay = l.datas.Where(d => d.name == spot).SingleOrDefault();
                    if (pay == null)
                        ds.Add(0);
                    else
                        ds.Add(pay.data);
                });
                datas.Add(new LineChartData()
                {
                    data = ds,
                    name = spot
                });
            }

            return new LineChartVM()
            {
                chartDatas = datas,
                chartLabels = list.Select(l => l.label).ToList()

            };
        }
        #endregion
    }

    public class WeekData
    {
        public int year { get; set; }
        public DateTime date { get; set; }
        public string name { get; set; }
        public double pay { get; set; }
        public int week { get; set; }
    }
}