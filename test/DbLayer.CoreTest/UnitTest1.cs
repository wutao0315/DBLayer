using DBLayer.Core;
using DBLayer.Core.Condition;
using DBLayer.Persistence;
using DBLayer.Persistence.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Specialized;
using DBLayer.Core.Extensions;
using DBLayer.Core.Interface;
using DBLayer.Persistence.PagerGenerator;
using DBLayer.Persistence.Generator;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MySqlX.XDevAPI.Common;
using System.Threading;
using System.Security.Claims;

namespace DbLayer.CoreTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestBaseEntity()
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory() + "/Config")
                 .AddJsonFile("dblayer.json", false, true);

            var config = builder.Build();

            IServiceCollection services = new ServiceCollection();
            services.Configure<DBLayerOption>(config.GetSection("dblayer"));
            services.AddLogging();


            services.AddDBLayerDefault("DbLayer.CoreTest");

            services.AddLogging((loggerBuilder) =>
            {
                loggerBuilder.ClearProviders();
                loggerBuilder.SetMinimumLevel(LogLevel.Debug);
                loggerBuilder.AddConsole();
            });

            services.AddTransient(provider => new ClaimsPrincipal());

            using var bsp = services.BuildServiceProvider();
            var service = bsp.GetService<IUcHelpRepository>();

            var userId = 1L;
            var text = "test";
            var username = "test";
            var job = new { WjPaId=1L };

            await service.GetEntityCountAsync<ParkArea>(w => w.Id == job.WjPaId);

            await service.UpdateEntityAsync(() => new ParkArea
            {
                PaWorkerId = userId,
                PaWorkerName = text,
                PaWorkerNo = username
            }, w => w.Id == job.WjPaId);


            var data = new UcHelp
            {
                Title = "title2",
                CodeValue = "value2",
                Content = "content2",
            };
            await service.InsertEntityAsync(() => data);

            var qty = await service.UpdateEntityAsync(() => new UcHelp
            {
                Title = "title2",
                CodeValue = "value2",
                Content = "content2",
            }, w => w.Id == 1);


            //Task.Run(async () => {

            //    while (true)
            //    {
            //        try
            //        {
            //            service.Uow.BeginTransaction();
            //            var qty = await service.UpdateEntityAsync(() => new UcHelp
            //            {
            //                Title = "title1",
            //                CodeValue = "value1",
            //                Content = "content1",
            //            }, w => w.Id == id);
            //            //var entity = await service.GetEntityAsync(w => w.Id == id);
            //            //var list = await service.GetEntityListAsync(w => w.Id == id);
            //            // await service.DeleteEntityAsync(w => w.Id == id);
            //            //var count = list.Count();
            //            //Assert.AreEqual(1, count);

            //            service.Uow.Commit();
            //        }
            //        catch (Exception ex)
            //        {
            //            service.Uow.Rollback();
            //        }
            //        Thread.Sleep(2000);
            //    }
            //});

            //Task.Run(async () => {
            //    while (true)
            //    {

            //        try
            //        {
            //            service.Uow.BeginTransaction();

            //            var qty = await service.UpdateEntityAsync(() => new UcHelp
            //            {
            //                Title = "title2",
            //                CodeValue = "value2",
            //                Content = "content2",
            //            }, w => w.Id == id);
            //            //var entity = await service.GetEntityAsync(w => w.Id == id);
            //            //var list = await service.GetEntityListAsync(w => w.Id == id);
            //            //await service.DeleteEntityAsync(w => w.Id == id);
            //            //var count = list.Count();
            //            //Assert.AreEqual(1, count);

            //            // uow.Commit();
            //            service.Uow.Rollback();
            //        }
            //        catch (Exception ex)
            //        {
            //            service.Uow.Rollback();
            //        }
            //        Thread.Sleep(2000);
            //    }

            //});

            Console.WriteLine("test");
            Console.ReadLine();
        }
        [TestMethod]
        public void TestIocConfigAddUpdateSelectDelete()
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory() + "/Config")
                 .AddJsonFile("dblayer.json", false, true);

            var config = builder.Build();

            IServiceCollection services = new ServiceCollection();
            services.Configure<DBLayerOption>(config.GetSection("dblayer"));
            services.AddLogging();


            services.AddDBLayerDefault("DbLayer.CoreTest");

            services.AddLogging((loggerBuilder) =>
            {
                loggerBuilder.ClearProviders();
                loggerBuilder.SetMinimumLevel(LogLevel.Debug);
                loggerBuilder.AddConsole();
            });


            using var bsp = services.BuildServiceProvider();
            var service = bsp.GetService<IUcDynamicRepository>();

            var myData = service.GetMyData();

            using var uow = bsp.GetService<IUnitOfWork>();
            try
            {
                uow.BeginTransaction();
                var id = service.InsertEntity(() => new UcDynamic
                {
                    Id = -1,
                    Type = 1,
                    Data = "",
                    CreateDt = DateTime.Now,
                    UserId = -1
                });
                service.UpdateEntity(() => new UcDynamic
                {
                    Data = "test",
                }, w => w.Id == id);

                var cmdText = "select a.*,a.data as DataContent from uc_dynamic a where a.id = @id";
                var entityData = service.GetEntity<UcDynamicExtInfo>(cmdText, new { id });

                var entity = service.GetEntity(w => w.Id == id && w.Data == "test");
                var list = service.GetEntityList(w => w.Data == "test");
                service.DeleteEntity(w => w.Id == id);

                var count = list.Count();
                Assert.AreEqual(1, count);

                uow.Commit();
            }
            catch
            {
                uow.Rollback();
            }

            //using (var bsp = services.BuildServiceProvider())
            //{
            //    var service = bsp.GetService<IUcHelpRepository>();

            //    var condition = new UcHelpCondition.Search
            //    {
            //        PageIndex = 1,
            //        PageSize = 20
            //    };

            //    var pagelist = await service.Seach(condition);
            //    var sys = new UcHelp
            //    {
            //        Title = "title",
            //        CodeValue = "value",
            //        Content = "content",
            //        Level = 1,
            //        IsEnable = true
            //    };
            //    var id = await service.InsertEntityAsync<UcHelp, long>(() => sys);


            //    //Task.Run(async () => {

            //    //    while (true)
            //    //    {
            //    //        try
            //    //        {
            //    //            service.Uow.BeginTransaction();
            //    //            var qty = await service.UpdateEntityAsync(() => new UcHelp
            //    //            {
            //    //                Title = "title1",
            //    //                CodeValue = "value1",
            //    //                Content = "content1",
            //    //            }, w => w.Id == id);
            //    //            //var entity = await service.GetEntityAsync(w => w.Id == id);
            //    //            //var list = await service.GetEntityListAsync(w => w.Id == id);
            //    //            // await service.DeleteEntityAsync(w => w.Id == id);
            //    //            //var count = list.Count();
            //    //            //Assert.AreEqual(1, count);

            //    //            service.Uow.Commit();
            //    //        }
            //    //        catch (Exception ex)
            //    //        {
            //    //            service.Uow.Rollback();
            //    //        }
            //    //        Thread.Sleep(2000);
            //    //    }
            //    //});

            //    //Task.Run(async () => {
            //    //    while (true)
            //    //    {

            //    //        try
            //    //        {
            //    //            service.Uow.BeginTransaction();

            //    //            var qty = await service.UpdateEntityAsync(() => new UcHelp
            //    //            {
            //    //                Title = "title2",
            //    //                CodeValue = "value2",
            //    //                Content = "content2",
            //    //            }, w => w.Id == id);
            //    //            //var entity = await service.GetEntityAsync(w => w.Id == id);
            //    //            //var list = await service.GetEntityListAsync(w => w.Id == id);
            //    //            //await service.DeleteEntityAsync(w => w.Id == id);
            //    //            //var count = list.Count();
            //    //            //Assert.AreEqual(1, count);

            //    //            // uow.Commit();
            //    //            service.Uow.Rollback();
            //    //        }
            //    //        catch (Exception ex)
            //    //        {
            //    //            service.Uow.Rollback();
            //    //        }
            //    //        Thread.Sleep(2000);
            //    //    }

            //    //});

            //    Console.WriteLine("test");
            //    Console.ReadLine();
            //}
        }

        [TestMethod]
        public async Task TestIocConfigAddUpdateSelectDelete2()
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory() + "/Config")
                 .AddJsonFile("dblayer_two.json", false, true);

            var config = builder.Build();

            IServiceCollection services = new ServiceCollection();
            services.Configure<DBLayerOption>(config.GetSection("dblayer"));
            services.AddLogging();

            services.AddDBLayerDefault("DbLayer.CoreTest");

            services.AddLogging((loggerBuilder) =>
            {
                loggerBuilder.ClearProviders();
                loggerBuilder.SetMinimumLevel(LogLevel.Debug);
                loggerBuilder.AddConsole();
            });

            //services.AddTransient<IUcDynamicRepository, UcDynamicRepository>();

            using var bsp = services.BuildServiceProvider();
            var service = bsp.GetService<IUcDynamicRepository>();

            var id = 261772383541661696L;
            var record = await service.GetEntityAsync<WkgJobRecord>(w => w.WjrId == id && w.WjrStatus > 1);

            //var myData = service.GetMyData();

            //using (var uow = bsp.GetService<IUnitOfWork>())
            //{
            //    try
            //    {
            //        uow.BeginTransaction();
            //        var id = service.InsertEntity(() => new UcDynamic
            //        {
            //            Id = -1,
            //            Type = 1,
            //            Data = "",
            //            CreateDt = DateTime.Now,
            //            UserId = -1
            //        });
            //        service.UpdateEntity(() => new UcDynamic
            //        {
            //            Data = "test",
            //        }, w => w.Id == id);


            //        var entity = service.GetEntity(w => w.Id == id && w.Data == "test");
            //        var list = service.GetEntityList(w => w.Data == "test", null);
            //        //service.DeleteEntity(w => w.Id == id);

            //        var count = list.Count();
            //        Assert.AreEqual(1, count);

            //        uow.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        uow.Rollback();
            //    }
            //}
        }

        [TestMethod]
        public void TestQueryable()
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory() + "/Config")
                 .AddJsonFile("dblayer_two.json", false, true);

            var config = builder.Build();

            IServiceCollection services = new ServiceCollection();
            services.Configure<DBLayerOption>(config.GetSection("dblayer"));
            services.AddLogging();

            services.AddDBLayerDefault("DbLayer.CoreTest");

            services.AddLogging((loggerBuilder) =>
            {
                loggerBuilder.ClearProviders();
                loggerBuilder.SetMinimumLevel(LogLevel.Debug);
                loggerBuilder.AddConsole();
            });

            //services.AddTransient<IUcDynamicRepository, UcDynamicRepository>();

            using var bsp = services.BuildServiceProvider();
            var service = bsp.GetService<IUcDynamicRepository>();

            var wkgJobRecord = service.Queryable<WkgJobRecord>();
           

            var id = 261772383541661696L;
            //var record = await service.GetEntityAsync<WkgJobRecord>(w => w.WjrId == id && w.WjrStatus > 1);
            var record = wkgJobRecord.FirstOrDefault(w => w.WjrId == id || w.WjrId > 1);
            var records = wkgJobRecord.Where(w => w.WjrId == id && w.WjrStatus > 1).OrderBy(w => w.WjrId).ToList();
            

        }

        [TestMethod]
        public async Task TestCachedProperty()
        {
            var aa = new { id=-1,name="aaa" };
            var aaList = aa.GetCachedProperties();
            foreach (var item in aaList)
            {
                if (item.Key.Name == nameof(aa.id)) 
                {
                    var id = item.Value.Getter(aa);
                }
                if (item.Key.Name == nameof(aa.name))
                {
                    var name = item.Value.Getter(aa);
                }
            }


            var ud = new UcDynamic();
            var propList = ud.GetCachedProperties();
            foreach (var item in propList)
            {
                if (item.Key.Name == nameof(UcDynamic.Id)) 
                {
                    var setData = 10000;
                    item.Value.Setter(ud, setData);
                    var id = item.Value.Getter(ud);
                    Assert.IsTrue(setData.ToString() ==  id.ToString());
                }

                if (item.Key.Name == nameof(UcDynamic.Type))
                {
                    var setData = 1;
                    item.Value.Setter(ud, setData);
                    var id = item.Value.Getter(ud);
                    Assert.IsTrue(setData.ToString() == id.ToString());
                }

                if (item.Key.Name == nameof(UcDynamic.Data))
                {
                    var setData = "aaaaa";
                    item.Value.Setter(ud, setData);
                    var id = item.Value.Getter(ud);
                    Assert.IsTrue(setData.ToString() == id.ToString());
                }

                if (item.Key.Name == nameof(UcDynamic.CreateDt))
                {
                    var setData = DateTime.Now;
                    item.Value.Setter(ud, setData);
                    var id = item.Value.Getter(ud);
                    Assert.IsTrue(setData.ToString() == id.ToString());
                }
            }

            var dto1 = ud.MapTo<UcDynamic, UcDynamicDto>();
            //var dto2 = ud.GetData<UcDynamicDto>();
            Console.WriteLine(dto1.ToString());

        }
    }
    public interface IUcDynamicRepository : IRepository<UcDynamic, long>
    {
        public UcDynamic GetMyData() 
        {
            var data = this.GetEntity(w=>w.Id>0);
            return data;
        }


    }
    public interface IUcHelpRepository : IRepository<UcHelp, long>
    {
        /// <summary>
        /// ��ҳ��ѯ
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <returns></returns>
        public async Task<IEnumerable<UcHelp>> Seach(UcHelpCondition.Search condition)
        {
            var page = new Pager<UcHelpCondition.Search>()
            {
                Condition = condition,
                Table = @"uc_help",
                Field = "*",
                WhereAction = (Where, Paramters) =>
                {
                    if (!string.IsNullOrEmpty(condition.Title))
                    {
                        Where.Append("AND title LIKE @title ");
                        Paramters.Add(this.CreateParameter("@title", string.Concat("%", condition.Title, "%")));
                    }

                    if (!string.IsNullOrEmpty(condition.Content))
                    {
                        Where.Append("AND content LIKE @content ");
                        Paramters.Add(this.CreateParameter("@content", string.Concat("%", condition.Content, "%")));
                    }

                    if (!string.IsNullOrEmpty(condition.CodeValue))
                    {
                        Where.Append("AND code_value LIKE @code_value ");
                        Paramters.Add(this.CreateParameter("@code_value", string.Concat("%", condition.CodeValue, "%")));
                    }

                    if (condition.Level!=null)
                    {
                        Where.Append("AND level = @level ");
                        Paramters.Add(this.CreateParameter("@level", condition.Level.Value));
                    }

                    if (condition.IsEnable != null)
                    {
                        Where.Append("AND is_enable LIKE @is_enable ");
                        Paramters.Add(this.CreateParameter("@is_enable", condition.IsEnable.Value));
                    }

                }
            };

            var result = await this.GetResultByPageAsync<UcHelp, UcHelpCondition.Search>(page);
            return result;
        }
    }

    public class UcHelpCondition
    {
        public class Search : BasePageCondition
        {
            /// <summary>
            /// ����
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// ����
            /// </summary>
            public string Content { get; set; }
            /// <summary>
            /// ����ֵ
            /// </summary>
            public string CodeValue { get; set; }
            /// <summary>
            /// �ȼ�:1:default,2:success,3:warning,3:danger
            /// </summary>
            public int? Level { get; set; }

            /// <summary>
            /// �Ƿ���Ч
            /// </summary>
            public bool? IsEnable { get; set; }
        }
    }
    /// <summary>
    /// �����������ݱ�
    /// UcHelp
    /// </summary>
    [Serializable]
    [DataTable("uc_help")]
    public class UcHelp : VirtulDelEntity<long>
    {
        /// <summary>
        /// ����
        /// </summary>
        [DataField("title")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// ����
        /// </summary>
        [DataField("content")]
        public string Content { get; set; } = string.Empty;
        /// <summary>
        /// ����ֵ
        /// </summary>
        [DataField("code_value")]
        public string CodeValue { get; set; } = string.Empty;
        /// <summary>
        /// �ȼ�:1:default,2:success,3:warning,3:danger
        /// </summary>
        [DataField("level")]
        public int Level { get; set; } = 1;

        /// <summary>
        /// �Ƿ���Ч
        /// </summary>
        [DataField("is_enable")]
        public bool IsEnable { get; set; } = true;
        

    }

    /// <summary>
    /// ͣ����
    /// ParkArea
    /// </summary>
    [Serializable]
    [DataTable("park_area")]
    public class ParkArea: VirtulDelEntity<long>
    {
        /// <summary>
        /// �շѱ�׼���
        /// </summary>
        [DataField("pa_pf_id")]
        public long PaPfId { get; set; } = 0;
        /// <summary>
        /// �շѱ�׼����
        /// </summary>
        [DataField("pa_pf_name")]
        public string PaPfName { get; set; } = string.Empty;
        /// <summary>
        /// ������֯�������
        /// </summary>
        [DataField("pa_addr_id")]
        public long PaAddrId { get; set; } = 0;
        /// <summary>
        /// ������֯��������
        /// </summary>
        [DataField("pa_addr_name")]
        public string PaAddrName { get; set; } = string.Empty;
        /// <summary>
        /// ���
        /// </summary>
        [DataField("pa_code")]
        public string PaCode { get; set; } = string.Empty;
        /// <summary>
        /// ͣ��������
        /// </summary>
        [DataField("pa_name")]
        public string PaName { get; set; } = string.Empty;
        /// <summary>
        /// ͣ�������ƿ�ݼ�
        /// </summary>
        [DataField("pa_name_key")]
        public string PaNameKey { get; set; } = string.Empty;
        /// <summary>
        /// �ܲ�λ��
        /// </summary>
        [DataField("pa_count")]
        public int PaCount { get; set; } = 0;
        /// <summary>
        /// �շѻ�����
        /// </summary>
        [DataField("pa_serial_no")]
        public string PaSerialNo { get; set; } = string.Empty;
        /// <summary>
        /// �շ�Ա���
        /// </summary>
        [DataField("pa_worker_id")]
        public long PaWorkerId { get; set; } = -1;
        /// <summary>
        /// �շ�Ա
        /// </summary>
        [DataField("pa_worker_name")]
        public string PaWorkerName { get; set; } = string.Empty;
        /// <summary>
        /// �շ�Ա����
        /// </summary>
        [DataField("pa_worker_no")]
        public string PaWorkerNo { get; set; } = string.Empty;
        /// <summary>
        /// ���·ѱ�׼
        /// </summary>
        [DataField("pa_fees_monthly")]
        public decimal PaFeesMonthly { get; set; } = 0;
        /// <summary>
        /// ���շ�ʱ��
        /// </summary>
        [DataField("pa_fees_free")]
        public int PaFeesFree { get; set; } = 0;
        /// <summary>
        /// ���ʱ���Ƿ�Ʒ�
        /// </summary>
        [DataField("pa_fees_free_cal")]
        public bool PaFeesFreeCal { get; set; } = false;
        /// <summary>
        /// ����
        /// </summary>
        [DataField("pa_lat")]
        public decimal PaLat { get; set; }
        /// <summary>
        /// γ��
        /// </summary>
        [DataField("pa_lng")]
        public decimal PaLng { get; set; }
        /// <summary>
        /// ���ű���
        /// </summary>
        [DataField("pa_zoom")]
        public int PaZoom { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        [DataField("pa_remark")]
        public string PaRemark { get; set; } = string.Empty;
        /// <summary>
        /// ����:1��ͣ����,2��ͣ����
        /// </summary>
        [DataField("pa_type")]
        public int PaType { get; set; } = 0;
        /// <summary>
        /// ��λ״̬
        /// </summary>
        [DataField("pa_status")]
        public int PaStatus { get; set; } = 0;
        /// <summary>
        /// �ش�״̬
        /// </summary>
        [DataField("pa_iot_status")]
        public int PaIotStatus { get; set; } = 0;
        /// <summary>
        /// ����ֵ
        /// </summary>
        [DataField("pa_sort")]
        public int PaSort { get; set; } = 0;
        /// <summary>
        /// ��С�Ʒ�ʱ��
        /// </summary>
        [DataField("pa_fees_min")]
        public int PaFeesMin { get; set; } = 0;
        /// <summary>
        /// �������
        /// </summary>
        [DataField("pa_wt_id")]
        public long PaWtId { get; set; } = 0;
        /// <summary>
        /// ��������
        /// </summary>
        [DataField("pa_wt_name")]
        public string PaWtName { get; set; } = string.Empty;
        /// <summary>
        /// ������
        /// </summary>
        [DataField("pa_creater")]
        public string PaCreater { get; set; } = string.Empty;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        [DataField("pa_createtime")]
        public DateTime PaCreatetime { get; set; } = DateTime.Now;
        /// <summary>
        /// �޸���
        /// </summary>
        [DataField("pa_updater")]
        public string PaUpdater { get; set; } = string.Empty;
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        [DataField("pa_updatetime")]
        public DateTime PaUpdatetime { get; set; } = DateTime.Now;
    }
    /// <summary>
    /// ǰ̨�˻���̬��Ϣ
    /// UcDynamic
    /// </summary>
    public class UcDynamicDto
    {
        /// <summary>
        /// ����
        /// </summary>
        public long Id { get; set; } = 0;
        /// <summary>
        /// ǰ̨�˻����
        /// </summary>
        public long? UserId { get; set; } = 0;
        /// <summary>
        /// ����:1������,2����ɫ,3���˵�״̬,4������,5����Ϣ,6������7���Ƿ����
        /// </summary>
        public int Type { get; set; } = 1;
        /// <summary>
        /// ��̬����
        /// </summary>
        public string Data { get; set; } = "";
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime? CreateDt { get; set; } = DateTime.Now;
    }
    /// <summary>
    /// ǰ̨�˻���̬��Ϣ
    /// UcDynamic
    /// </summary>
    [Serializable]
    [DataTable("uc_dynamic")]
    public class UcDynamic
    {
        /// <summary>
        /// ����
        /// </summary>
        [DataField("id", IsAuto = true, IsKey = true, KeyType = KeyType.MANUAL)]
        public long Id { get; set; } = 0;
        /// <summary>
        /// ǰ̨�˻����
        /// </summary>
        [DataField("user_id")]
        public long? UserId { get; set; } = 0;
        /// <summary>
        /// ����:1������,2����ɫ,3���˵�״̬,4������,5����Ϣ,6������7���Ƿ����
        /// </summary>
        [DataField("type")]
        public int Type { get; set; } = 1;
        /// <summary>
        /// ��̬����
        /// </summary>
        [DataField("data")]
        public string Data { get; set; } = "";
        /// <summary>
        /// ����ʱ��
        /// </summary>
        [DataField("create_dt")]
        public DateTime? CreateDt { get; set; } = DateTime.Now;

    }

    public class UcDynamicExtInfo : UcDynamic
    {
        /// <summary>
        /// ��̬����
        /// </summary>
        [DataField("data")]
        public string DataContent { get; set; } = "";
    }

    /// <summary>
    /// ͣ����¼
    /// WkgJobRecord
    /// </summary>
    [Serializable]
    [DataTable("wkg_job_record")]
    public class WkgJobRecord
    {
        /// <summary>
        /// ����
        /// </summary>
        [DataField("wjr_id", IsAuto = true, IsKey = true, KeyType = KeyType.MANUAL)]
        public long WjrId { get; set; } = -1;
        /// <summary>
        /// ͣ�������
        /// </summary>
        [DataField("wjr_pa_id")]
        public long WjrPaId { get; set; } = 0;
        /// <summary>
        /// ͣ��������
        /// </summary>
        [DataField("wjr_pa_name")]
        public string WjrPaName { get; set; } = string.Empty;
        /// <summary>
        /// ͣ��λ���
        /// </summary>
        [DataField("wjr_pas_id")]
        public long WjrPasId { get; set; } = 0;
        /// <summary>
        /// ͣ��λ���
        /// </summary>
        [DataField("wjr_pas_code")]
        public string WjrPasCode { get; set; } = string.Empty;
        /// <summary>
        /// ����:1ͣ����2������3��Ʊ
        /// </summary>
        [DataField("wjr_type")]
        public int WjrType { get; set; } = 1;
        /// <summary>
        /// �շ��㷨
        /// </summary>
        [DataField("wjr_fees")]
        public string WjrFees { get; set; } = string.Empty;
        /// <summary>
        /// Ӧ�ñ��
        /// </summary>
        [DataField("wjr_app_id")]
        public string WjrAppId { get; set; } = string.Empty;
        /// <summary>
        /// �������
        /// </summary>
        [DataField("wjr_vhe_id")]
        public long WjrVheId { get; set; } = -1;
        /// <summary>
        /// ���ƺ�
        /// </summary>
        [DataField("wjr_vhe_plate_no")]
        public string WjrVhePlateNo { get; set; } = string.Empty;
        /// <summary>
        /// �Ű�����
        /// </summary>
        [DataField("wjr_shifttime")]
        public DateTime WjrShifttime { get; set; } = DateTime.Now;
        /// <summary>
        /// �볡�������
        /// </summary>
        [DataField("wjr_in_wj_id")]
        public long WjrInWjId { get; set; } = 0;
        /// <summary>
        /// �볡ʱ��
        /// </summary>
        [DataField("wjr_in_time")]
        public DateTime WjrInTime { get; set; } = DateTime.Now;
        /// <summary>
        /// �볡���κ�
        /// </summary>
        [DataField("wjr_in_batch_no")]
        public string WjrInBatchNo { get; set; } = string.Empty;
        /// <summary>
        /// �볡��ˮ��
        /// </summary>
        [DataField("wjr_in_serial_no")]
        public int WjrInSerialNo { get; set; } = 0;
        /// <summary>
        /// �볡�������
        /// </summary>
        [DataField("wjr_in_team_id")]
        public long WjrInTeamId { get; set; } = -1;
        /// <summary>
        /// �볡��������
        /// </summary>
        [DataField("wjr_in_team_name")]
        public string WjrInTeamName { get; set; } = string.Empty;
        /// <summary>
        /// �볡�������
        /// </summary>
        [DataField("wjr_out_wj_id")]
        public long WjrOutWjId { get; set; } = 0;
        /// <summary>
        /// �볡ʱ��
        /// </summary>
        [DataField("wjr_out_time")]
        public DateTime? WjrOutTime { get; set; }
        /// <summary>
        /// �볡���κ�
        /// </summary>
        [DataField("wjr_out_batch_no")]
        public string WjrOutBatchNo { get; set; } = string.Empty;
        /// <summary>
        /// �볡��ˮ��
        /// </summary>
        [DataField("wjr_out_serial_no")]
        public int WjrOutSerialNo { get; set; } = 0;
        /// <summary>
        /// �볡�������
        /// </summary>
        [DataField("wjr_out_team_id")]
        public long WjrOutTeamId { get; set; } = -1;
        /// <summary>
        /// �볡��������
        /// </summary>
        [DataField("wjr_out_team_name")]
        public string WjrOutTeamName { get; set; } = string.Empty;
        /// <summary>
        /// ���ʱ��
        /// </summary>
        [DataField("wjr_park_free")]
        public int WjrParkFree { get; set; } = 0;
        /// <summary>
        /// ͣ��ʱ��
        /// </summary>
        [DataField("wjr_park_length")]
        public int WjrParkLength { get; set; } = 0;
        /// <summary>
        /// ͣ������:1����ͨ��2�����£�3������
        /// </summary>
        [DataField("wjr_park_type")]
        public int WjrParkType { get; set; } = 0;
        /// <summary>
        /// �������
        /// </summary>
        [DataField("wjr_vm_id")]
        public long WjrVmId { get; set; } = -1;
        /// <summary>
        /// ����֤��
        /// </summary>
        [DataField("wjr_vm_code")]
        public string WjrVmCode { get; set; } = string.Empty;
        /// <summary>
        /// ��Ƭ��ַ1
        /// </summary>
        [DataField("wjr_picture1")]
        public string WjrPicture1 { get; set; } = string.Empty;
        /// <summary>
        /// ��Ƭ��ַ2
        /// </summary>
        [DataField("wjr_picture2")]
        public string WjrPicture2 { get; set; } = string.Empty;
        /// <summary>
        /// ��Ƭ��ַ3
        /// </summary>
        [DataField("wjr_picture3")]
        public string WjrPicture3 { get; set; } = string.Empty;
        /// <summary>
        /// ��Ƭ��ַ4
        /// </summary>
        [DataField("wjr_picture4")]
        public string WjrPicture4 { get; set; } = string.Empty;
        /// <summary>
        /// ������
        /// </summary>
        [DataField("wjr_picture_count")]
        public int WjrPictureCount { get; set; } = 0;
        /// <summary>
        /// �ϴ�ʱ��
        /// </summary>
        [DataField("wjr_uploadtime")]
        public DateTime WjrUploadtime { get; set; } = DateTime.Now;
        /// <summary>
        /// ״̬:1�ڳ���2�Ѹ��볡��3δ���볡��4�ܸ��볡��5δ���°��볡
        /// </summary>
        [DataField("wjr_status")]
        public int WjrStatus { get; set; } = 0;
        /// <summary>
        /// ͣ�����������
        /// </summary>
        [DataField("wjr_pay_area")]
        public decimal WjrPayArea { get; set; } = 0;
        /// <summary>
        /// ͣ����Ӧ�ս��
        /// </summary>
        [DataField("wjr_pay_area_should")]
        public decimal WjrPayAreaShould { get; set; } = 0;
        /// <summary>
        /// ����ͣ��Ӧ�ս��
        /// </summary>
        [DataField("wjr_pay_should")]
        public decimal WjrPayShould { get; set; } = 0;
        /// <summary>
        /// ͣ����¼ʵ�ս��
        /// </summary>
        [DataField("wjr_pay_amount")]
        public decimal WjrPayAmount { get; set; } = 0;
        /// <summary>
        /// �������
        /// </summary>
        [DataField("wjr_pay_after")]
        public decimal WjrPayAfter { get; set; } = 0;
        /// <summary>
        /// ���½��
        /// </summary>
        [DataField("wjr_pay_monthly")]
        public decimal WjrPayMonthly { get; set; } = 0;
        /// <summary>
        /// Ԥ�������
        /// </summary>
        [DataField("wjr_advance_id")]
        public long WjrAdvanceId { get; set; } = 0;
        /// <summary>
        /// Ԥ����
        /// </summary>
        [DataField("wjr_advance_name")]
        public string WjrAdvanceName { get; set; } = string.Empty;
        /// <summary>
        /// Ԥ���˹���
        /// </summary>
        [DataField("wjr_advance_user")]
        public string WjrAdvanceUser { get; set; } = string.Empty;
        /// <summary>
        /// Ԥ�����
        /// </summary>
        [DataField("wjr_advance_pay")]
        public decimal WjrAdvancePay { get; set; } = 0;
        /// <summary>
        /// Ԥ������
        /// </summary>
        [DataField("wjr_advance_no")]
        public string WjrAdvanceNo { get; set; } = string.Empty;
        /// <summary>
        /// Ԥ��֧��ƽ̨������
        /// </summary>
        [DataField("wjr_advance_trade_no")]
        public string WjrAdvanceTradeNo { get; set; } = string.Empty;
        /// <summary>
        /// Ԥ��������:1���ֽ�2��΢�ţ�3��֧������
        /// </summary>
        [DataField("wjr_advance_type")]
        public int WjrAdvanceType { get; set; } = 0;
        /// <summary>
        /// Ԥ����ʱ��
        /// </summary>
        [DataField("wjr_advance_time")]
        public DateTime WjrAdvanceTime { get; set; } = DateTime.Now;
        /// <summary>
        /// Ԥ��״̬:1δ����2�Ѹ�
        /// </summary>
        [DataField("wjr_advance_status")]
        public int WjrAdvanceStatus { get; set; } = 0;
        /// <summary>
        /// ���������
        /// </summary>
        [DataField("wjr_settle_id")]
        public long WjrSettleId { get; set; } = 0;
        /// <summary>
        /// ������
        /// </summary>
        [DataField("wjr_settle_name")]
        public string WjrSettleName { get; set; } = string.Empty;
        /// <summary>
        /// �����˹���
        /// </summary>
        [DataField("wjr_settle_user")]
        public string WjrSettleUser { get; set; } = string.Empty;
        /// <summary>
        /// ������
        /// </summary>
        [DataField("wjr_settle_pay")]
        public decimal WjrSettlePay { get; set; } = 0;
        /// <summary>
        /// ���㶩��
        /// </summary>
        [DataField("wjr_settle_no")]
        public string WjrSettleNo { get; set; } = string.Empty;
        /// <summary>
        /// ����֧��ƽ̨������
        /// </summary>
        [DataField("wjr_settle_trade_no")]
        public string WjrSettleTradeNo { get; set; } = string.Empty;
        /// <summary>
        /// ��������:1���ֽ�2��΢�ţ�3��֧������
        /// </summary>
        [DataField("wjr_settle_type")]
        public int WjrSettleType { get; set; } = 0;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        [DataField("wjr_settle_time")]
        public DateTime? WjrSettleTime { get; set; }
        /// <summary>
        /// ����״̬:1δ����2�Ѹ�
        /// </summary>
        [DataField("wjr_settle_status")]
        public int WjrSettleStatus { get; set; } = 0;
        /// <summary>
        /// �Ƿ���
        /// </summary>
        [DataField("wjr_is_handle")]
        public bool WjrIsHandle { get; set; } = false;
        /// <summary>
        /// �Ƿ���
        /// </summary>
        [DataField("wjr_is_black")]
        public bool WjrIsBlack { get; set; } = false;
    }
}
