﻿using MySql.Data.MySqlClient;
using PMS.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.DAL
{
    public static class CommentDAO
    {

        public static int Save(CommentDTO dto)
        {
            try
            {
                String sqlQuery = "";

                sqlQuery = String.Format("INSERT INTO Comments(UserId,ProductId,CommentText,CommentOn) VALUES('{0}','{1}','{2}',NOW())",
                    dto.UserID, dto.ProductID, dto.CommentText);


                using (DBHelper helper = new DBHelper())
                {
                    return helper.ExecuteQuery(sqlQuery);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }


        public static CommentDTO GetCommentById(int pid)
        {
            try
            {

                var query = String.Format("Select * from Comments Where CommentId={0}", pid);

                using (DBHelper helper = new DBHelper())
                {
                    var reader = helper.ExecuteReader(query);

                    CommentDTO dto = null;

                    if (reader.Read())
                    {
                        dto = FillDTO(reader);
                    }

                    return dto;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public static List<CommentDTO> GetAllComments()
        {
            try
            {
                using (DBHelper helper = new DBHelper())
                {
                    var query = @"Select q.CommentId,q.UserID,q.ProductID, q.CommentText, q.CommentOn, u.Name,u.PictureName 
                            from Comments q, Users u 
                            where q.UserId = u.UserID";

                    var reader = helper.ExecuteReader(query);
                    List<CommentDTO> list = new List<CommentDTO>();

                    while (reader.Read())
                    {
                        var dto = FillDTO(reader);
                        if (dto != null)
                        {
                            list.Add(dto);
                        }
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public static List<CommentDTO> GetTopComments(int topCount)
        {
            try
            {
                using (DBHelper helper = new DBHelper())
                {
                    var query = String.Format(@"SELECT q.CommentId,q.UserId,q.ProductId, q.CommentText, q.CommentOn, u.Name,u.PictureName 
                                    FROM Comments q, Users u
                                    WHERE q.UserId = u.UserID");
                    var reader = helper.ExecuteReader(query);
                    List<CommentDTO> list = new List<CommentDTO>();

                    while (reader.Read())
                    {
                        var dto = FillDTO(reader);
                        if (dto != null)
                        {
                            list.Add(dto);
                        }
                    }

                    return list;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        private static CommentDTO FillDTO(MySqlDataReader reader)
        {
            var dto = new CommentDTO();
            dto.CommentID = reader.GetInt32(reader.GetOrdinal("CommentId"));
            dto.UserID = reader.GetInt32(reader.GetOrdinal("UserID"));
            dto.ProductID = reader.GetInt32(reader.GetOrdinal("ProductID"));
            dto.CommentText = reader.GetString(reader.GetOrdinal("CommentText"));
            dto.CommentOn = reader.GetDateTime(reader.GetOrdinal("CommentOn"));

            dto.UserName = reader.GetString(reader.GetOrdinal("Name"));
            dto.PictureName = reader.GetString(reader.GetOrdinal("PictureName"));

            return dto;
        }
    }
}
