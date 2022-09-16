using DBLayer;
using DBLayer.DataProvider.SqlServer;
using DBLayer.Extensions;
using DBLayer.Interface;
using DBLayer.Mapping;
using DBLayerTest;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DbLayer.CoreTest
{
    [TestClass]
    public class LinqTest
    {
        [TestMethod]
        public void TestConnection()
        {
            try
            {
                var username = "superadmin";

                using var db = new DbPrm();
                var query = from p in db.PrmUser
                            from b in db.PrmEmp.Where(q => q.PrmUserId == p.Id).DefaultIfEmpty()
                            where p.UserName == username
                            orderby p.CreateDt descending
                            select new {p.Id,p.Name,p.UserName,b.Pinyin };

                var list = query.ToList();

                Assert.AreNotEqual(list.Count, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
    public class DbPrm : DBLayer.Data.DataConnection
    {
        public DbPrm() 
            : base(SqlServerTools.GetDataProvider(SqlServerVersion.v2016,
                SqlServerProvider.MicrosoftDataSqlClient),
                  "data source=192.168.1.105;initial catalog=IDaaS;persist security info=True;user id=sa;password=eq123!@#;TrustServerCertificate=true") { }

        public IQueryable<PrmUserInfo> PrmUser => this.GetTable<PrmUserInfo>().Where(w=>w.IsDelete == false);
        public IQueryable<PrmEmpInfo> PrmEmp => this.GetTable<PrmEmpInfo>().Where(w => w.IsDelete == false);
    }

    /// <summary>
    /// 基础业务模型
    /// </summary>
    public class BaseInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseInfo()
        {
            CreateDt = DateTime.Now;
            UpdateDt = DateTime.Now;
        }
        /// <summary>
        /// 主键
        /// </summary>
        [Column("ID")]
        [PrimaryKey]
        public virtual Guid Id { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [Column("IS_DELETE")]
        public bool IsDelete { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        [Column("CREATER")]
        public string Creater { get; set; } = string.Empty;
        /// <summary>
        /// 创建用户
        /// </summary>
        [Column("CREATE_USER_ID")]
        public Guid CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CREATE_DT")]
        public DateTime CreateDt { get; set; }
        /// <summary>
        /// 更新用户
        /// </summary>
        [Column("UPDATER")]
        public string Updater { get; set; } = string.Empty;
        /// <summary>
        /// 更新用户
        /// </summary>
        [Column("UPDATE_USER_ID")]
        public Guid UpdateUserId { get; set; }
        /// <summary>
        /// 更新时间 
        /// </summary>
        [Column("UPDATE_DT")]
        public DateTime UpdateDt { get; set; }
    }
    /// <summary>
    /// 用户列表
    /// </summary>
    [Table("PRM_USER")]
    public partial class PrmUserInfo : BaseInfo
    {
        /// <summary>
        /// 用户类型
        /// </summary>
        [Column("USER_TYPE")]
        public int UserType { get; set; } = 1;
        /// <summary>
        /// 用户名
        /// </summary>
        [Column("USER_NAME")]
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// 密码
        /// </summary>
        [Column("PASSWORD")]
        public string Password { get; set; } = string.Empty;
        /// <summary>
        /// 工号
        /// </summary>
        [Column("CODE")]
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// 身份证
        /// </summary>
        [Column("ID_CARD")]
        public string IdCard { get; set; } = string.Empty;
        /// <summary>
        /// 电话
        /// </summary>
        [Column("PHONE")]
        public string Phone { get; set; } = string.Empty;
        /// <summary>
        /// 邮件地址
        /// </summary>
        [Column("EMAIL")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 分组-EN
        /// </summary>
        [Column("GROUP_NAME")]
        public string GroupName { get; set; } = string.Empty;
        /// <summary>
        /// 名称
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK")]
        public string Remark { get; set; } = string.Empty;
        /// <summary>
        /// 模块
        /// </summary>
        [Column("MODULE")]
        public string Module { get; set; } = string.Empty;
        /// <summary>
        /// 是否激活状态
        /// </summary>
        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// 人脸是否收集
        /// </summary>
        [Column("IS_COLLECT_FACE")]
        public bool IsCollectFace { get; set; }
        /// <summary>
        /// 冻结时间
        /// </summary>
        [Column("FROZEN_DT")]
        public DateTime FrozenDt { get; set; } = DateTime.Now;
        /// <summary>
        /// 状态:1、正常,2、锁定,3冻结
        /// </summary>
        [Column("STATUS")]
        public int Status { get; set; } = 1;
        /// <summary>
        /// 编辑状态:1、可编辑,0、不可编辑
        /// </summary>
        [Column("IS_EDIT")]
        public bool IsEdit { get; set; } = false;
        /// <summary>
        /// 过期时间,空为不过期
        /// </summary>
        [Column("EXPIRED_DT")]
        public DateTime? ExpiredDt { get; set; }
        /// <summary>
        /// 密码强制过期时间
        /// </summary>
        [Column("PASS_EXPIRE_DT")]
        public DateTime PassExpireDT { get; set; } = DateTime.Now;
        /// <summary>
        /// 扩展编号
        /// </summary>
        [Column("EXT1")]
        public string Ext1 { get; set; } = string.Empty;
        /// <summary>
        /// 英文名称
        /// </summary>
        [Column("EXT2")]
        public string Ext2 { get; set; } = string.Empty;
        /// <summary>
        /// 中文名称
        /// </summary>
        [Column("EXT3")]
        public string Ext3 { get; set; } = string.Empty;
        /// <summary>
        /// 分组-EN
        /// </summary>
        [Column("EXT4")]
        public string Ext4 { get; set; } = string.Empty;
        /// <summary>
        /// 分组-CN
        /// </summary>
        [Column("EXT5")]
        public string Ext5 { get; set; } = string.Empty;
        /// <summary>
        /// 名称
        /// </summary>
        [Column("EXT6")]
        public string Ext6 { get; set; } = string.Empty;
    }

    /// <summary>
    /// 员工表
    /// </summary>
    [Table("PRM_EMP")]
    public partial class PrmEmpInfo : BaseInfo
    {
        /// <summary>
        /// 企业ID
        /// </summary>
        [Column("PRM_ORG_ID")]
        public Guid PrmOrgId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("PRM_USER_ID")]
        public Guid PrmUserId { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [Column("PHONE")]
        public string Phone { get; set; } = string.Empty;
        /// <summary>
        /// 名称
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 名称拼音
        /// </summary>
        [Column("PINYIN")]
        public string Pinyin { get; set; } = string.Empty;
        /// <summary>
        /// 邮件地址
        /// </summary>
        [Column("EMAIL")]
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// 状态:1、待确认;2、待激活;3、正常
        /// </summary>
        [Column("STATUS")]
        public int Status { get; set; }
        ///// <summary>
        ///// 昵称
        ///// </summary>
        //[Column("NICK_NAME")]
        //public string NickName { get; set; } = string.Empty;
        /// <summary>
        /// 工号
        /// </summary>
        [Column("CODE")]
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// 职位
        /// </summary>
        [Column("POSITION")]
        public string Position { get; set; } = string.Empty;
        /// <summary>
        /// 入职时间
        /// </summary>
        [Column("ENTRY_DT")]
        public DateTime EntryDt { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        [Column("DEPTS")]
        public string Depts { get; set; } = string.Empty;
        /// <summary>
        /// 角色
        /// </summary>
        [Column("ROLES")]
        public string Roles { get; set; } = string.Empty;
        /// <summary>
        /// 用户组
        /// </summary>
        [Column("GROUPS")]
        public string Groups { get; set; } = string.Empty;
        /// <summary>
        /// 扩展1
        /// </summary>
        [Column("EXT1")]
        public string Ext1 { get; set; } = string.Empty;
        /// <summary>
        /// 扩展2
        /// </summary>
        [Column("EXT2")]
        public string Ext2 { get; set; } = string.Empty;
        /// <summary>
        /// 扩展3
        /// </summary>
        [Column("EXT3")]
        public string Ext3 { get; set; } = string.Empty;
        /// <summary>
        /// 扩展4
        /// </summary>
        [Column("EXT4")]
        public string Ext4 { get; set; } = string.Empty;
        /// <summary>
        /// 扩展5
        /// </summary>
        [Column("EXT5")]
        public string Ext5 { get; set; } = string.Empty;
        /// <summary>
        /// 扩展6
        /// </summary>
        [Column("EXT6")]
        public string Ext6 { get; set; } = string.Empty;
    }
}
