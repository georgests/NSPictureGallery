using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Telligent.Evolution.Extensibility.Api.Entities.Version1;
using TEApi = Telligent.Evolution.Extensibility.Api.Version1.PublicApi;

namespace STSImage.InternalApi
{
	internal static class ImageDataService
	{
		internal static string ConnectionString { get; set; }

		internal static void AddUpdateImageGallery(ImageGallery ig)
		{
			using (var connection = GetSqlConnection())
			{
                using (var command = CreateSprocCommand("[sts_image_gallery_AddUpdate]", connection))
				{
					command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = ig.Id;
					command.Parameters.Add("@Name", SqlDbType.NVarChar, 255).Value = ig.Name;
					command.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = ig.Description;
					command.Parameters.Add("@AuthorUserId", SqlDbType.Int).Value = ig.AuthorUserId;
					command.Parameters.Add("@GroupId", SqlDbType.Int).Value = ig.GroupId;
					command.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = ig.IsEnabled;
					command.Parameters.Add("@DateUtc", SqlDbType.DateTime).Value = DateTime.UtcNow;

					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();
				}
			}
		}

        internal static void AddUpdateImage(Image img)
        {
            using (var connection = GetSqlConnection())
            {
                using (var command = CreateSprocCommand("[sts_image_AddUpdate]", connection))
                {
                    command.Parameters.Add("@ImageId", SqlDbType.UniqueIdentifier).Value = img.Id;
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = img.UserId;
                    command.Parameters.Add("@FilePath", SqlDbType.NVarChar, 255).Value = img.FilePath;
                    command.Parameters.Add("@ImageTitle", SqlDbType.NVarChar).Value = img.Title;
                    command.Parameters.Add("@Degree", SqlDbType.Int).Value = img.Degree;
                    command.Parameters.Add("@ImageGalleryId", SqlDbType.UniqueIdentifier).Value = img.ImageGalleryId;
                    command.Parameters.Add("@DateUtc", SqlDbType.DateTime).Value = DateTime.UtcNow;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        internal static void ImageDelete(Image img)
        {
            using (var connection = GetSqlConnection())
            {
                using (var command = CreateSprocCommand("[sts_image_delete]", connection))
                {
                    command.Parameters.Add("@ImageId", SqlDbType.UniqueIdentifier).Value = img.Id;
                    command.Parameters.Add("@UserId", SqlDbType.NVarChar, 255).Value = img.UserId;
                    command.Parameters.Add("@ImageGalleryId", SqlDbType.UniqueIdentifier).Value = img.ImageGalleryId;
                    command.Parameters.Add("@DateUtc", SqlDbType.DateTime).Value = DateTime.UtcNow;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        internal static void ImageUpdateTitle(Image img)
        {
            using (var connection = GetSqlConnection())
            {
                using (var command = CreateSprocCommand("[sts_image_update_title]", connection))
                {
                    command.Parameters.Add("@ImageId", SqlDbType.UniqueIdentifier).Value = img.Id;
                    command.Parameters.Add("@UserId", SqlDbType.NVarChar, 255).Value = img.UserId;
                    command.Parameters.Add("@ImageGalleryId", SqlDbType.UniqueIdentifier).Value = img.ImageGalleryId;
                    command.Parameters.Add("@ImageTitle", SqlDbType.NVarChar, 255).Value = img.Title;
                    command.Parameters.Add("@DateUtc", SqlDbType.DateTime).Value = DateTime.UtcNow;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }


        internal static void ImageUpdateDegree(Image img)
        {
            using (var connection = GetSqlConnection())
            {
                using (var command = CreateSprocCommand("[sts_image_update_degree]", connection))
                {
                    command.Parameters.Add("@ImageId", SqlDbType.UniqueIdentifier).Value = img.Id;
                    command.Parameters.Add("@UserId", SqlDbType.NVarChar, 255).Value = img.UserId;
                    command.Parameters.Add("@ImageGalleryId", SqlDbType.UniqueIdentifier).Value = img.ImageGalleryId;
                    command.Parameters.Add("@Degree", SqlDbType.NVarChar, 255).Value = img.Degree;
                   
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        internal static void AddDefaultImageGallery(ImageGallery ig)
        {
            using (var connection = GetSqlConnection())
            {
                using (var command = CreateSprocCommand("[sts_image_gallery_AddDefaultGallery]", connection))
                {
                    command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = ig.Id;
                    command.Parameters.Add("@Name", SqlDbType.NVarChar, 255).Value = ig.Name;
                    command.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = ig.Description;
                    command.Parameters.Add("@AuthorUserId", SqlDbType.Int).Value = ig.AuthorUserId;
                    command.Parameters.Add("@GroupId", SqlDbType.Int).Value = ig.GroupId;
                    command.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = ig.IsEnabled;
                    command.Parameters.Add("@DateUtc", SqlDbType.DateTime).Value = DateTime.UtcNow;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        internal static ImageGallery GetImageGalleryAll(Guid imgGalleryId)
        {
            ImageGallery imgGallery = null;

            using (var connection = GetSqlConnection())
            {
                using (var command = CreateSprocCommand("[sts_Images_GetAll]", connection))
                {
                    command.Parameters.Add("@ImageGalleryId", SqlDbType.UniqueIdentifier).Value = imgGalleryId;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            imgGallery = PopulateImageGallery(reader);

                            reader.NextResult();
                            while (reader.Read())
                            {
                                imgGallery.Images.Add(PopulateImage(reader));
                            }
                        }
                    }

                    connection.Close();
                }
            }

            return imgGallery;
        }

        internal static Image GetImage(Guid imgId)
        {
            Image img = null;

            using (var connection = GetSqlConnection())
            {
                using (var command = CreateSprocCommand("[sts_Image_getById]", connection))
                {
                    command.Parameters.Add("@ImageId", SqlDbType.UniqueIdentifier).Value = imgId;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            img = PopulateImage(reader);
                        }
                    }

                    connection.Close();
                }
            }

            return img;
        }

        internal static ImageGallery GetImageGallery(Guid imgGalleryId)
        {
            ImageGallery imgGallery = null;
            int userId = TEApi.Users.AccessingUser.Id.Value;

            using (var connection = GetSqlConnection())
            {
                using (var command = CreateSprocCommand("[sts_Images_Get]", connection))
                {
                    command.Parameters.Add("@ImageGalleryId", SqlDbType.UniqueIdentifier).Value = imgGalleryId;
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            imgGallery = PopulateImageGallery(reader);

                            reader.NextResult();
                            while (reader.Read())
                            {
                                imgGallery.Images.Add(PopulateImage(reader));
                            }
                        }
                    }

                    connection.Close();
                }
            }

            return imgGallery;
        }

        internal static ImageGallery GetImageGalleryByCurrent(Guid imgGalleryId,int userId)
        {
            ImageGallery imgGallery = null;
          //  int userId = TEApi.Users.AccessingUser.Id.Value;

            using (var connection = GetSqlConnection())
            {
                using (var command = CreateSprocCommand("[sts_Images_Get]", connection))
                {
                    command.Parameters.Add("@ImageGalleryId", SqlDbType.UniqueIdentifier).Value = imgGalleryId;
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            imgGallery = PopulateImageGallery(reader);

                            reader.NextResult();
                            while (reader.Read())
                            {
                                imgGallery.Images.Add(PopulateImage(reader));
                            }
                        }
                    }

                    connection.Close();
                }
            }

            return imgGallery;
        }


        internal static PagedList<ImageGallery> GetImageGalleryList(int userId)
        {
            PagedList<ImageGallery> listImgGallery = new PagedList<ImageGallery>();

            using (var connection = GetSqlConnection())
            {
                using (var command = CreateSprocCommand("[sts_ImageGallerys_List]", connection))
                {
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            listImgGallery.Add(PopulateImageGallery(reader));
                        }
                    }

                    connection.Close();
                }
            }

            return listImgGallery;
        }

		internal static bool IsConnectionStringValid()
		{
			bool isValid = false;

			if (!string.IsNullOrEmpty(ConnectionString))
			{
				try
				{
					using (var connection = GetSqlConnection())
					{
						using (var command = new SqlCommand("SELECT IS_MEMBER('db_owner') As IsOwner", connection))
						{
							connection.Open();
							using (var reader = command.ExecuteReader())
							{
								isValid = reader.Read() && Convert.ToBoolean(reader["IsOwner"]);
							}
							connection.Close();
						}
					}
				}
				catch 
				{
					isValid = false;
				}
			}

			return isValid;
		}


		internal static void Install()
		{
			Install("install.sql");
		}

		internal static void Install(string fileName)
		{
			using (var connection = GetSqlConnection())
			{
				connection.Open();
				foreach (string statement in GetStatementsFromSqlBatch(EmbeddedResources.GetString("STSImage.Resources.Sql." + fileName)))
				{
					using (var command = new SqlCommand(statement, connection))
					{
						command.ExecuteNonQuery();
					}
				}
				connection.Close();
			}
		}

		internal static void UnInstall()
		{
			using (var connection = GetSqlConnection())
			{
				connection.Open();
                foreach (string statement in GetStatementsFromSqlBatch(EmbeddedResources.GetString("STSImage.Resources.Sql.uninstall.sql")))
				{
					using (var command = new SqlCommand(statement, connection))
					{
						command.ExecuteNonQuery();
					}
				}
				connection.Close();
			}
		}

		#region Population

		private static ImageGallery PopulateImageGallery(IDataReader reader)
		{
            ImageGallery img = new ImageGallery();

			img.AuthorUserId = Convert.ToInt32(reader["AuthorUserId"]);
			img.CreatedDateUtc = Convert.ToDateTime(reader["CreatedDateUtc"]);
			img.Description = reader["Description"] == DBNull.Value ? string.Empty : reader["Description"].ToString();
			img.GroupId = Convert.ToInt32(reader["GroupId"]);
            img.Id = Guid.Parse(reader["Id"].ToString());
			img.IsEnabled = Convert.ToBoolean(reader["IsEnabled"]);
			img.LastUpdatedDateUtc = Convert.ToDateTime(reader["LastUpdatedDateUtc"]);
			img.Name = reader["Name"].ToString();
			
			img.Images = new List<Image>();

			return img;
		}

		private static Image PopulateImage(IDataReader reader)
		{
			Image img = new Image();

            img.Id = Guid.Parse(reader["ImageId"].ToString());
            img.UserId = Convert.ToInt32(reader["UserId"]);
            img.ImageGalleryId = Guid.Parse(reader["ImageGalleryId"].ToString());
            img.FilePath = reader["FilePath"].ToString();
			img.Title = reader["Title"].ToString();
            img.Degree = reader["Degree"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Degree"]);
            img.Status = reader["Status"].ToString();

			return img;
		}

		#endregion

		#region Helpers

		private static IEnumerable<string> GetStatementsFromSqlBatch(string sqlBatch)
		{
			// This isn't as reliable as the SQL Server SDK, but works for most SQL batches and prevents another assembly reference
			foreach (string statement in Regex.Split(sqlBatch, @"^\s*GO\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline))
			{
				string sanitizedStatement = Regex.Replace(statement, @"(?:^SET\s+.*?$|\/\*.*?\*\/|--.*?$)", "\r\n", RegexOptions.IgnoreCase | RegexOptions.Multiline).Trim();
				if (sanitizedStatement.Length > 0)
					yield return sanitizedStatement;
			}				
		}

		private static SqlConnection GetSqlConnection()
		{
			return new SqlConnection(ConnectionString);
		}

		private static SqlCommand CreateSprocCommand(string sprocName, SqlConnection connection)
		{
			return new SqlCommand("dbo." + sprocName, connection) { CommandType = CommandType.StoredProcedure };
		}

		#endregion
	}
}
