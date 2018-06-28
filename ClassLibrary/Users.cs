//-----------------------------------------------------------------------
// <copyright file="Users.cs">
//     Author: hieuht
//     CreateDate: 29/06/2018 01:38:36
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace ClassLibrary
{
    public class Users
    {
        #region Private Properties
        private int _UserId;
        private string _UserName;
        private string _Password;
        private string _FullName;
        private string _Address;
        private string _Email;
        private string _Mobile;
        private byte _GenderId;
        private short _DefaultActionId;
        private DateTime _BirthDay;
        private DateTime _CrDateTime;
        private byte _UserStatusId;
        private byte _UserTypeId;
        private DatabaseAccess db;

        #endregion

        #region Public Properties

        //-----------------------------------------------------------------
        public Users()
        {
            db = new DatabaseAccess();
        }
        //-----------------------------------------------------------------        
        public Users(string providerName, string connectionString)
        {
            db = new DatabaseAccess(providerName, connectionString);
        }
        //-----------------------------------------------------------------
        ~Users()
        {

        }
        //-----------------------------------------------------------------
        public virtual void Dispose()
        {

        }
        //-----------------------------------------------------------------    
        public int UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                _UserId = value;
            }
        }

        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
            }
        }
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
            }
        }
        public string FullName
        {
            get
            {
                return _FullName;
            }
            set
            {
                _FullName = value;
            }
        }
        public string Address
        {
            get
            {
                return _Address;
            }
            set
            {
                _Address = value;
            }
        }
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
            }
        }
        public string Mobile
        {
            get
            {
                return _Mobile;
            }
            set
            {
                _Mobile = value;
            }
        }
        public byte GenderId
        {
            get
            {
                return _GenderId;
            }
            set
            {
                _GenderId = value;
            }
        }
        public short DefaultActionId
        {
            get
            {
                return _DefaultActionId;
            }
            set
            {
                _DefaultActionId = value;
            }
        }
        public DateTime BirthDay
        {
            get
            {
                return _BirthDay;
            }
            set
            {
                _BirthDay = value;
            }
        }
        public DateTime CrDateTime
        {
            get
            {
                return _CrDateTime;
            }
            set
            {
                _CrDateTime = value;
            }
        }

        public byte UserStatusId
        {
            get
            {
                return _UserStatusId;
            }
            set
            {
                _UserStatusId = value;
            }
        }
        public byte UserTypeId
        {
            get
            {
                return _UserTypeId;
            }
            set
            {
                _UserTypeId = value;
            }
        }


        #endregion

        #region Method
        /// <summary>
        /// Lấy danh sách đối tượng Users từ DbDataReader
        /// </summary>
        /// <param name="cmd">DbCommand</param>
        /// <returns>list</returns>
        private List<Users> Init(DbCommand cmd)
        {
            List<Users> listUsers = new List<Users>();
            try
            {
                using (DbDataReader reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        Users mUsers = new Users
                        {
                            UserId = reader.ReadAs<int>("UserId"),
                            UserName = reader.ReadAs<string>("UserName"),
                            Password = reader.ReadAs<string>("Password"),
                            FullName = reader.ReadAs<string>("FullName"),
                            Address = reader.ReadAs<string>("Address"),
                            Email = reader.ReadAs<string>("Email"),
                            Mobile = reader.ReadAs<string>("Mobile"),
                            GenderId = reader.ReadAs<byte>("GenderId"),
                            UserStatusId = reader.ReadAs<byte>("UserStatusId"),
                            UserTypeId = reader.ReadAs<byte>("UserTypeId"),
                            DefaultActionId = reader.ReadAs<short>("DefaultActionId"),
                            BirthDay = reader.ReadAs<DateTime>("BirthDay"),
                            CrDateTime = reader.ReadAs<DateTime>("CrDateTime"),
                        };
                        listUsers.Add(mUsers);
                    }
                }
                return listUsers;
            }
            catch (Exception err)
            {
                throw new Exception("Data error: " + err.Message);
            }
        }
        //-----------------------------------------------------------
        /// <summary>
        /// Thêm đối tượng Users
        /// </summary>
        /// <param name="sysMessageId"></param>
        /// <returns>sysMessageTypeId</returns>
        public byte Insert(ref int sysMessageId)
        {
            byte retVal = 0;
            try
            {
                retVal = InsertOrUpdate(ref sysMessageId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //--------------------------------------------------------------
        /// <summary>
        /// Cập nhật đối tượng Users
        /// </summary>
        /// <param name="sysMessageId"></param>
        /// <returns>sysMessageTypeId</returns>
        public byte Update(ref int sysMessageId)
        {
            byte retVal = 0;
            try
            {
                retVal = InsertOrUpdate(ref sysMessageId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //-----------------------------------------------------------
        /// <summary>
        /// Thêm/Sửa đối tượng Users
        /// </summary>
        /// <param name="sysMessageId"></param>
        /// <returns>sysMessageTypeId</returns>
        public byte InsertOrUpdate(ref int sysMessageId)
        {
            byte retVal = 0;
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("Users_InsertOrUpdate");
                db.AddInParameter(cmd, "@UserName", DbType.String, UserName);
                db.AddInParameter(cmd, "@Password", DbType.String, Password);
                db.AddInParameter(cmd, "@FullName", DbType.String, FullName);
                db.AddInParameter(cmd, "@Address", DbType.String, Address);
                db.AddInParameter(cmd, "@Email", DbType.String, Email);
                db.AddInParameter(cmd, "@Mobile", DbType.String, Mobile);
                db.AddInParameter(cmd, "@GenderId", DbType.Byte, GenderId);
                db.AddInParameter(cmd, "@UserStatusId", DbType.Byte, UserStatusId);
                db.AddInParameter(cmd, "@UserTypeId", DbType.Byte, UserTypeId);
                db.AddInParameter(cmd, "@DefaultActionId", DbType.Int16, DefaultActionId);
                db.AddInParameter(cmd, "@BirthDay", DbType.DateTime, BirthDay);
                db.AddInputOutputParameter(cmd, "@UserId", DbType.Int32, UserId);
                db.AddOutParameter(cmd, "@sysMessageId", DbType.Int32);
                db.AddOutParameter(cmd, "@SysMessageTypeId", DbType.Byte);
                db.ExecuteNonQuery(cmd);
                UserId = Convert.ToInt32(db.GetParameter(cmd, "@UserId").Value == null ? "0" : db.GetParameter(cmd, "@UserId").Value);
                sysMessageId = Convert.ToInt16(db.GetParameter(cmd, "@sysMessageId").Value == null ? "0" : db.GetParameter(cmd, "@sysMessageId").Value);
                retVal = Convert.ToByte(db.GetParameter(cmd, "@SysMessageTypeId").Value == null ? "0" : db.GetParameter(cmd, "@SysMessageTypeId").Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //-------------------------------------------------------------- 
        /// <summary>
        /// Xóa đối tượng Users
        /// </summary>
        /// <param name="sysMessageId">sysMessageId</param>
        /// <returns>sysMessageTypeId</returns>
        public byte Delete(ref int sysMessageId)
        {
            byte retVal = 0;
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("Users_Delete");
                db.AddInParameter(cmd, "@UserId", DbType.Int32, UserId);
                db.AddOutParameter(cmd, "@sysMessageId", DbType.Int32);
                db.AddOutParameter(cmd, "@SysMessageTypeId", DbType.Byte);
                db.ExecuteNonQuery(cmd);
                sysMessageId = Convert.ToInt16(db.GetParameter(cmd, "@sysMessageId").Value == null ? "0" : db.GetParameter(cmd, "@sysMessageId").Value);
                retVal = Convert.ToByte(db.GetParameter(cmd, "@SysMessageTypeId").Value == null ? "0" : db.GetParameter(cmd, "@SysMessageTypeId").Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //--------------------------------------------------------------     
        /// <summary>
        /// Trả về đối tượng Users theo điều kiện đầu vào
        /// </summary>
        /// <returns>Users</returns>
        public Users Get()
        {
            Users retVal = new Users();
            int rowCount = 0, pageSize = 20, pageNumber = 0;
            string dateFrom = string.Empty, dateTo = string.Empty, orderBy = string.Empty;
            try
            {
                List<Users> list = GetPage(dateFrom, dateTo, orderBy, pageSize, pageNumber, ref rowCount);
                if (list.Count > 0) retVal = list[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }


        //-------------------------------------------------------------- 
        /// <summary>
        /// Danh sách Users phân trang theo điều kiện đầu vào
        /// </summary>
        /// <param name="dateFrom">Từ ngày</param>
        /// <param name="dateTo">Đến ngày</param>
        /// <param name="orderBy">Sắp xếp theo</param>
        /// <param name="pageSize">Số records/trang</param>
        /// <param name="pageNumber">Số trang</param>
        /// <param name="rowCount">Tổng số records</param>
        /// <returns>listUsers</returns>
        public List<Users> GetPage(string dateFrom, string dateTo, string orderBy, int pageSize, int pageNumber, ref int rowCount)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("Users_GetPage");
                db.AddInParameter(cmd, "@UserId", DbType.Int32, UserId);
                db.AddInParameter(cmd, "@UserName", DbType.String, UserName);
                db.AddInParameter(cmd, "@Password", DbType.String, Password);
                db.AddInParameter(cmd, "@FullName", DbType.String, FullName);
                db.AddInParameter(cmd, "@Address", DbType.String, Address);
                db.AddInParameter(cmd, "@Email", DbType.String, Email);
                db.AddInParameter(cmd, "@Mobile", DbType.String, Mobile);
                db.AddInParameter(cmd, "@GenderId", DbType.Byte, GenderId);
                db.AddInParameter(cmd, "@UserStatusId", DbType.Byte, UserStatusId);
                db.AddInParameter(cmd, "@UserTypeId", DbType.Byte, UserTypeId);
                db.AddInParameter(cmd, "@DefaultActionId", DbType.Int16, DefaultActionId);
                db.AddInParameter(cmd, "@DateFrom", DbType.String, dateFrom);
                db.AddInParameter(cmd, "@DateTo", DbType.String, dateTo);
                db.AddInParameter(cmd, "@OrderBy", DbType.String, orderBy);
                db.AddInParameter(cmd, "@PageSize", DbType.Int32, pageSize);
                db.AddInParameter(cmd, "@PageNumber", DbType.Int32, pageNumber);
                db.AddOutParameter(cmd, "@RowCount", DbType.Int32);
                List<Users> listUsers = Init(cmd);
                rowCount = Convert.ToInt32(db.GetParameter(cmd, "@RowCount").Value == null ? "0" : db.GetParameter(cmd, "@RowCount").Value);
                return listUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //--------------------------------------------------------------
        /// <summary>
        /// Trả về UserName theo userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>UserName</returns>
        public static string Static_GetUserName(int userId)
        {
            Users mUsers = new Users
            {
                UserId = userId
            }.Get();
            return mUsers.UserName;
        }

        #endregion
    }
}

