using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telligent.Evolution.Extensibility.Version1;
using Telligent.Evolution.Extensibility.Rest.Version2;
using Telligent.Evolution.Extensibility.Rest.Entities.Version2;
using Telligent.Evolution.Extensibility.Api.Version1;
using Telligent.Evolution.Extensibility.Storage.Version1;
using TEApi = Telligent.Evolution.Extensibility.Api.Version1.PublicApi;
using System.IO;


namespace STSImage.Plugins
{
	public class ImageGalleryRestEndpoints: IPlugin, IRestEndpoints
	{

		#region IPlugin Members

		public string Name
		{
			get { return "Image Gallery REST API Endpoints";  }
		}

		public string Description
		{
			get { return "Adds support for Image Gallery REST endpoints."; }
		}

		public void Initialize()
		{
		}

		#endregion

		#region IRestEndpoints Members

		public void Register(IRestEndpointController controller)
		{

            #region ImageGallerys Endpoints

            controller.Add(2, "imagegallerys/imagegallery", HttpMethod.Post, (IRestRequest request) =>
            {
                var response = new RestApi.RestResponse();
                response.Name = "ImageGallery";

                try
                {
                    // Create
                    string name = request.Form["Name"] ?? string.Empty;

                    var group = TEApi.Groups.Get(new GroupsGetOptions { Id = Convert.ToInt32(request.PathParameters["GroupId"]) });
                    var ig = new InternalApi.ImageGallery();
                    if(group.Id.HasValue) {
                        ig.GroupId = group.Id.Value;
                    }
                    ig.Name = name;
                    ig.IsEnabled = true;
                    ig.AuthorUserId = request.UserId;

                    InternalApi.ImageService.AddUpdateImageGallery(ig);
                    ig = InternalApi.ImageService.GetImageGallery(ig.Id);

                    response.Data = new RestApi.ImageGallery(ig);
                }
                catch (Exception ex)
                {
                    response.Errors = new string[] { ex.Message };
                }

                return response;
            });

            controller.Add(2, "imagegallerys/addimage", HttpMethod.Post, (IRestRequest request) =>
            {
                var response = new RestApi.RestResponse();
                response.Name = "ImageGallery";
                string r = " === ";

                try
                {
                    // Create
                    string filePath = request.Form["filePath"] ?? string.Empty;
                    string name = request.Form["Name"] ?? string.Empty;
                    var group = TEApi.Groups.Get(new GroupsGetOptions { Id = Convert.ToInt32(request.PathParameters["GroupId"]) });
//                    string imgUrl = filePath;
                    int userId = TEApi.Users.AccessingUser.Id.Value;

                    r = r + " Before ListImageGallerys ";
                    var listIg = InternalApi.ImageService.ListImageGallerys(userId, 10, 0);
                    var ig = listIg.First<InternalApi.ImageGallery>();
                    r = r + " After ListImageGallerys ";

                    // Update to Database
                    var img = new InternalApi.Image();

                    r = r + " After Guid ";
                    // img.Id = new Guid();
                    img.Title = name;
                    img.FilePath = filePath;
                    img.Status = "A";
                    img.Degree = 0;
                    img.ImageGalleryId = ig.Id;
                    r = r + " Before AccessingUser ";
                    img.UserId = TEApi.Users.AccessingUser.Id.Value;
                    r = r + " Before AddUpdateImage ";
                    r = r +  InternalApi.ImageService.AddUpdateImage(img);
                    r = r + " After AddUpdateImage ";

                    /*
                    img = InternalApi.ImageService.GetImage(img.Id);
                    r = r + " After GetImage "; 
                    response.Data = new RestApi.Image(img, ig);
                    r = r + " After response Data ";
                     */
                }
                catch (Exception ex)
                {
                    response.Errors = new string[] { r + ex.Message };
                }

                return response;
            });

            controller.Add(2, "imagegallerys/deleteimage", HttpMethod.Post, (IRestRequest request) =>
            {
                var response = new RestApi.RestResponse();
                response.Name = "ImageGallery";

                try
                {
                    // Delete
                    var imageId = new Guid(request.Form["imageId"].ToUpper());

                    /*
                    var group = TEApi.Groups.Get(new GroupsGetOptions { Id = Convert.ToInt32(request.PathParameters["GroupId"]) });

                    var listIg = InternalApi.ImageService.ListImageGallerys(group.Id ?? 1, 10, 0);
                    var ig = listIg.First<InternalApi.ImageGallery>();
                    */

                    // Delete Image from Database
                    var img = InternalApi.ImageService.GetImage(imageId);
                    InternalApi.ImageService.DeleteImage(img);

                    response.Data = "Success";
                }
                catch (Exception ex)
                {
                    response.Errors = new string[] { ex.Message };
                }

                return response;
            });

            controller.Add(2, "imagegallerys/updatetitle", HttpMethod.Post, (IRestRequest request) =>
            {
                var response = new RestApi.RestResponse();
                response.Name = "ImageGallery";

                try
                {
                    // Delete
                    var imageId = new Guid(request.Form["imageId"].ToUpper());
                    var title = request.Form["title"];

                    /*
                    var group = TEApi.Groups.Get(new GroupsGetOptions { Id = Convert.ToInt32(request.PathParameters["GroupId"]) });

                    var listIg = InternalApi.ImageService.ListImageGallerys(group.Id ?? 1, 10, 0);
                    var ig = listIg.First<InternalApi.ImageGallery>();
                    */

                    // Delete Image from Database
                    var img = InternalApi.ImageService.GetImage(imageId);
                    InternalApi.ImageService.UpdateImageTitle(img, title);

                    response.Data = "Success";
                }
                catch (Exception ex)
                {
                    response.Errors = new string[] { ex.Message };
                }

                return response;
            });

            controller.Add(2, "imagegallerys/updatedegree", HttpMethod.Post, (IRestRequest request) =>
            {
                var response = new RestApi.RestResponse();
                response.Name = "ImageGallery";

                try
                {
                    // Delete
                    var imageId = new Guid(request.Form["imageId"].ToUpper());
                    int degree = Convert.ToInt32( request.Form["degree"]);
                  
              
                    var img = InternalApi.ImageService.GetImage(imageId);
                    InternalApi.ImageService.UpdateDegree(img, degree);

                    response.Data = "Success";
                }
                catch (Exception ex)
                {
                    response.Errors = new string[] { ex.Message };
                }

                return response;
            });

            #endregion

            /*
			#region Poll Endpoints

			controller.Add(2, "groups/{groupid}/polls", new { }, new { groupid = @"\d+" }, HttpMethod.Get, (IRestRequest request) =>
			{
				var response = new RestApi.RestResponse();
				response.Name = "Polls";

				try
				{
					int pageSize;
					int pageIndex;
					string sortBy = "Date";

					if (!int.TryParse(request.Request.QueryString["PageSize"], out pageSize))
						pageSize = 20;

					if (!int.TryParse(request.Request.QueryString["PageIndex"], out pageIndex))
						pageIndex = 0;

					if (request.Request.QueryString["SortBy"] != null)
						sortBy = request.Request.QueryString["SortBy"];

					if (sortBy == "TopPollsScore")
					{
						var group = TEApi.Groups.Get(new GroupsGetOptions { Id = Convert.ToInt32(request.PathParameters["groupid"]) });
						if (group == null || group.HasErrors())
							response.Data = new Telligent.Evolution.Extensibility.Rest.Entities.Version1.PagedList<RestApi.ImageGallery>();
						else
						{
							var scores = TEApi.CalculatedScores.List(Plugins.TopPollsScore.ScoreId, new CalculatedScoreListOptions { ApplicationId = group.ApplicationId, ContentTypeId = PublicApi.Polls.ContentTypeId, PageIndex = pageIndex, PageSize = pageSize, SortOrder = "Descending" });

							var polls = new List<RestApi.ImageGallery>();
							foreach (var score in scores)
							{
								if (score.Content != null)
								{
									var poll = RestApi.PollingService.GetPoll(score.Content.ContentId);
									if (poll != null)
										polls.Add(new RestApi.ImageGallery(poll));
								}
							}

							response.Data = new Telligent.Evolution.Extensibility.Rest.Entities.Version1.PagedList<RestApi.ImageGallery>(polls, scores.PageSize, scores.PageIndex, scores.TotalCount);
						}
					}
					else
					{
						var polls = RestApi.PollingService.ListPolls(Convert.ToInt32(request.PathParameters["groupid"]), pageSize, pageIndex);
						response.Data = new Telligent.Evolution.Extensibility.Rest.Entities.Version1.PagedList<RestApi.ImageGallery>(polls.Select(x => new RestApi.ImageGallery(x)), polls.PageSize, polls.PageIndex, polls.TotalCount);
					}
				}
				catch (Exception ex)
				{
					response.Errors = new string[] { ex.Message };
				}

				return response;
			});

			controller.Add(2, "polls/poll", HttpMethod.Get, (IRestRequest request) =>
			{
				var response = new RestApi.RestResponse();
				response.Name = "Poll";

				try
				{
					Guid pollId;
					if (!Guid.TryParse(request.Request.QueryString["Id"], out pollId))
						throw new ArgumentException("Id is required.");

					var poll = RestApi.PollingService.GetPoll(pollId);
					if (poll == null)
						throw new Exception("The poll does not exist.");
					
					response.Data = new RestApi.ImageGallery(poll);
				}
				catch (Exception ex)
				{
					response.Errors = new string[] { ex.Message };
				}

				return response;
			});

			controller.Add(2, "polls/poll", HttpMethod.Delete, (IRestRequest request) =>
			{
				var response = new RestApi.RestResponse();

				try
				{
					Guid pollId;
					if (!Guid.TryParse(request.Request.QueryString["Id"], out pollId))
						throw new ArgumentException("Id is required.");

					var poll = RestApi.PollingService.GetPoll(pollId);
					if (poll != null)
						RestApi.PollingService.DeletePoll(poll);
				}
				catch (Exception ex)
				{
					response.Errors = new string[] { ex.Message };
				}

				return response;
			});

			controller.Add(2, "polls/poll", HttpMethod.Post, (IRestRequest request) =>
			{
				var response = new RestApi.RestResponse();
				response.Name = "Poll";

				try
				{
					// Create
					int groupId;
					if (!int.TryParse(request.Form["GroupId"], out groupId))
						throw new ArgumentException("GroupId is required.");

					string name = request.Form["Name"] ?? string.Empty;
					string description = request.Form["Description"];
					bool hideResultsUntilVotingComplete = request.Form["HideResultsUntilVotingComplete"] == null ? false : Convert.ToBoolean(request.Form["HideResultsUntilVotingComplete"]);
					DateTime? votingEndDate = request.Form["VotingEndDate"] == null ? null : (DateTime?)RestApi.Formatting.FromUserTimeToUtc(DateTime.Parse(request.Form["VotingEndDate"]));

					var poll = new RestApi.Poll();
					poll.GroupId = groupId;
					poll.Name = name;
					poll.Description = description;
					poll.IsEnabled = true;
					poll.AuthorUserId = request.UserId;
					poll.HideResultsUntilVotingComplete = hideResultsUntilVotingComplete;
					poll.VotingEndDateUtc = votingEndDate;

					RestApi.PollingService.AddUpdatePoll(poll);
					poll = RestApi.PollingService.GetPoll(poll.Id);
					
					response.Data = new RestApi.ImageGallery(poll);
				}
				catch (Exception ex)
				{
					response.Errors = new string[] { ex.Message };
				}

				return response;
			});

			controller.Add(2, "polls/poll", HttpMethod.Put, (IRestRequest request) =>
			{
				var response = new RestApi.RestResponse();
				response.Name = "Poll";

				try
				{
					// Update
					Guid pollId;
					if (!Guid.TryParse(request.Request.QueryString["Id"], out pollId))
						throw new ArgumentException("Id is required.");

					string name = request.Form["Name"];
					string description = request.Form["Description"];

					var poll = RestApi.PollingService.GetPoll(pollId);
					if (poll == null)
						throw new Exception("The poll does not exist.");

					if (request.Form["HideResultsUntilVotingComplete"] != null)
						poll.HideResultsUntilVotingComplete = Convert.ToBoolean(request.Form["HideResultsUntilVotingComplete"]);

					if (request.Form["VotingEndDate"] != null)
						poll.VotingEndDateUtc = (DateTime?) RestApi.Formatting.FromUserTimeToUtc(Convert.ToDateTime(request.Form["VotingEndDate"]));
					else if (request.Form["ClearVotingEndDate"] != null && Convert.ToBoolean(request.Form["ClearVotingEndDate"]))
						poll.VotingEndDateUtc = null;
					
					if (name != null)
						poll.Name = name;

					if (description != null)
						poll.Description = description;

					RestApi.PollingService.AddUpdatePoll(poll);
					poll = RestApi.PollingService.GetPoll(poll.Id);

					response.Data = new RestApi.ImageGallery(poll);
				}
				catch (Exception ex)
				{
					response.Errors = new string[] { ex.Message };
				}

				return response;
			});

			#endregion

			#region Poll Answer Endpoints

			controller.Add(2, "polls/answer", HttpMethod.Get, (IRestRequest request) =>
			{
				var response = new RestApi.RestResponse();
				response.Name = "PollAnswer";

				try
				{
					Guid pollAnswerId;
					if (!Guid.TryParse(request.Request.QueryString["Id"], out pollAnswerId))
						throw new ArgumentException("Id is required.");

					var pollAnswer = RestApi.PollingService.GetPollAnswer(pollAnswerId);
					if (pollAnswer == null)
						throw new Exception("The poll answer does not exist.");

					var poll = RestApi.PollingService.GetPoll(pollAnswer.PollId);
					if (poll == null)
						throw new Exception("The poll does not exist.");

					response.Data = new RestApi.Image(pollAnswer, poll);
				}
				catch (Exception ex)
				{
					response.Errors = new string[] { ex.Message };
				}

				return response;
			});

			controller.Add(2, "polls/answer", HttpMethod.Delete, (IRestRequest request) =>
			{
				var response = new RestApi.RestResponse();

				try
				{
					Guid pollAnswerId;
					if (!Guid.TryParse(request.Request.QueryString["Id"], out pollAnswerId))
						throw new ArgumentException("Id is required.");

					var pollAnswer = RestApi.PollingService.GetPollAnswer(pollAnswerId);
					if (pollAnswer != null)
						RestApi.PollingService.DeletePollAnswer(pollAnswer);
				}
				catch (Exception ex)
				{
					response.Errors = new string[] { ex.Message };
				}

				return response;
			});

			controller.Add(2, "polls/answer", HttpMethod.Post, (IRestRequest request) =>
			{
				var response = new RestApi.RestResponse();
				response.Name = "PollAnswer";

				try
				{
					// Create
					Guid pollId;
					if (!Guid.TryParse(request.Form["PollId"], out pollId))
						throw new ArgumentException("PollId is required.");

					string name = request.Form["Name"] ?? string.Empty;

					var pollAnswer = new RestApi.Image();
					pollAnswer.PollId = pollId;
					pollAnswer.Name = name;

					RestApi.PollingService.AddUpdatePollAnswer(pollAnswer);
					pollAnswer = RestApi.PollingService.GetPollAnswer(pollAnswer.Id);

					var poll = RestApi.PollingService.GetPoll(pollAnswer.PollId);

					response.Data = new RestApi.Image(pollAnswer, poll);
				}
				catch (Exception ex)
				{
					response.Errors = new string[] { ex.Message };
				}

				return response;
			});

			controller.Add(2, "polls/answer", HttpMethod.Put, (IRestRequest request) =>
			{
				var response = new RestApi.RestResponse();
				response.Name = "PollAnswer";

				try
				{
					// Update
					Guid pollAnswerId;
					if (!Guid.TryParse(request.Request.QueryString["Id"], out pollAnswerId))
						throw new ArgumentException("Id is required.");

					string name = request.Form["Name"];

					var pollAnswer = RestApi.PollingService.GetPollAnswer(pollAnswerId);
					if (pollAnswer == null)
						throw new Exception("The poll answer does not exist.");

					if (name != null)
						pollAnswer.Name = name;

					RestApi.PollingService.AddUpdatePollAnswer(pollAnswer);
					pollAnswer = RestApi.PollingService.GetPollAnswer(pollAnswer.Id);

					var poll = RestApi.PollingService.GetPoll(pollAnswer.PollId);

					response.Data = new RestApi.Image(pollAnswer, poll);
				}
				catch (Exception ex)
				{
					response.Errors = new string[] { ex.Message };
				}

				return response;
			});

			#endregion

			#region Poll Voting Endpoints

			controller.Add(2, "polls/vote", HttpMethod.Get, (IRestRequest request) =>
			{
				var response = new RestApi.RestResponse();
				response.Name = "PollVote";

				try
				{
					Guid pollId;
					if (!Guid.TryParse(request.Request.QueryString["PollId"], out pollId))
						throw new ArgumentException("PollId is required.");

					var poll = RestApi.PollingService.GetPoll(pollId);
					if (poll == null)
						throw new Exception("The poll does not exist.");

					var vote = RestApi.PollingService.GetPollVote(pollId, request.UserId);
					if (vote != null)
						response.Data = new RestApi.PollVote(vote);
				}
				catch (Exception ex)
				{
					response.Errors = new string[] { ex.Message };
				}

				return response;
			});

			controller.Add(2, "polls/vote", HttpMethod.Delete, (IRestRequest request) =>
			{
				var response = new RestApi.RestResponse();

				try
				{
					Guid pollId;
					if (!Guid.TryParse(request.Form["PollId"], out pollId))
						throw new ArgumentException("PollId is required.");

					var vote = RestApi.PollingService.GetPollVote(pollId, request.UserId);
					if (vote != null)
						RestApi.PollingService.DeletePollVote(vote);
				}
				catch (Exception ex)
				{
					response.Errors = new string[] { ex.Message };
				}

				return response;
			});

			controller.Add(2, "polls/vote", HttpMethod.Post, (IRestRequest request) =>
			{
				var response = new RestApi.RestResponse();
				response.Name = "PollVote";

				try
				{
					// Create
					Guid pollId;
					if (!Guid.TryParse(request.Form["PollId"], out pollId))
						throw new ArgumentException("PollId is required.");

					Guid pollAnswerId;
					if (!Guid.TryParse(request.Form["PollAnswerId"], out pollAnswerId))
						throw new ArgumentException("PollAnswerId is required.");
					
					var pollVote = new RestApi.PollVote();
					pollVote.PollId = pollId;
					pollVote.PollAnswerId = pollAnswerId;
					pollVote.UserId = request.UserId;

					RestApi.PollingService.AddUpdatePollVote(pollVote);
					pollVote = RestApi.PollingService.GetPollVote(pollId, request.UserId);

					response.Data = new RestApi.PollVote(pollVote);
				}
				catch (Exception ex)
				{
					response.Errors = new string[] { ex.Message };
				}

				return response;
			});

			#endregion
            */
		}

		#endregion
	}
}
